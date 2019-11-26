using Default;
using InLoox.ODataClient;
using InLoox.ODataClient.Data.BusinessObjects;
using InLoox.ODataClient.Extensions;
using InLooxShared.Reflection;
using Microsoft.OData.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace InLooxShared.Odata
{
    public class TaskClient
    {
        private readonly OdClient _client;
        public TaskClient(OdClient client)
        {
            _client = client;
        }

        public async Task AddOrUpdateByName(IDictionary<string, string> dict)
        {
            if (!dict.ContainsKey("Name"))
                throw new ArgumentException(nameof(dict) + " should contain 'Name'");

            var name = dict["Name"];
            var ctx = _client.GetContext();
            var query = GetTaskQuery(ctx, name);
            var dsWk = await ODataBasics.GetDSCollection(query);
            var task = dsWk.FirstOrDefault();

            if (task == null)
            {
                task = new WorkPackageView();
                dsWk.Add(task);
                task.Name = name;
            }

            await UpdateTask(ctx, task, dict);

            await ctx.SaveChangesAsync(SaveChangesOptions.PostOnlySetProperties);
        }

        private async Task UpdateTask(Container ctx, WorkPackageView task, IDictionary<string, string> dict)
        {
            foreach (var entry in dict)
            {
                var prop = Info.GetProperty<WorkPackageView>(entry.Key);
                if (prop == null)
                    continue;
                try
                {
                    var ret = await RedirectPropertyTask(ctx, task, prop, entry.Value);
                    if (ret != null)
                    {
                        prop = Info.GetProperty<WorkPackageView>(ret.Item1);
                        prop.SetValue(task, ret.Item2);
                    }
                    else
                    {
                        var convertedValue = StringToObject.Parse(prop.PropertyType, entry.Value);
                        prop = Info.GetProperty<WorkPackageView>(entry.Key);
                        prop.SetValue(task, convertedValue);
                    }
                }
                catch (TargetException ex)
                {
                    _client.GetLogger().WriteError($"TargetException: {ex.Message}");
                }
            }
        }

        private static DataServiceQuery<WorkPackageView> GetTaskQuery(Container ctx, string name)
        {
            var query = ctx.workpackageview.Where(k => k.Name == name)
                .ToDataServiceQuery();
            return query;
        }

        public async Task Delete(string name)
        {
            var ctx = _client.GetContext();
            var query = GetTaskQuery(ctx, name);
            var list = await query.ExecuteAsync();
            ctx.DeleteObject(list.First());
        }

        // todo refactor:

        private async Task<Tuple<string, object>> RedirectPropertyTask(Container ctx, WorkPackageView task,
            MemberInfo prop, string value)
        {
            switch (prop.Name)
            {
                case nameof(WorkPackageView.GroupName):
                    {
                        var groups = await ctx.group.ExecuteAsync();
                        var group = groups.FirstOrDefault(k => k.Name == value);
                        if (group == null)
                            _client.GetLogger().WriteError($"group '{value}' not found, skipping");
                        else
                            return new Tuple<string, object>(nameof(WorkPackageView.GroupId), group.GroupId);
                        break;
                    }
                case nameof(WorkPackageView.ProjectName):
                    {
                        var taskName = value;
                        var project = await ctx.projectview.Where(k => k.Name == taskName)
                            .FirstOrDefaultSq();

                        if (project == null)
                            _client.GetLogger().WriteError($"project with name '{value}' not found, skipping");
                        else
                            return new Tuple<string, object>(nameof(WorkPackageView.ProjectId),
                                project.ProjectId);
                        break;
                    }
                case nameof(WorkPackageView.PlanningReservationStatusName) when task.ProjectId == null:
                    _client.GetLogger().WriteError("task needs a project");
                    return null;
                case nameof(WorkPackageView.PlanningReservationStatusName):
                    {
                        var status = await ctx.planningreservationstatus
                            .Where(k => k.ProjectId == task.ProjectId && k.Name == value)
                            .FirstOrDefaultSq();

                        if (status == null)
                            _client.GetLogger().WriteError($"task status '{value}' not found, skipping");
                        else
                            return new Tuple<string, object>(nameof(WorkPackageView.PlanningReservationStatusId),
                                status.PlanningReservationStatusId);
                        break;
                    }
                default:
                    return null;
            }

            return null;
        }
    }
}
