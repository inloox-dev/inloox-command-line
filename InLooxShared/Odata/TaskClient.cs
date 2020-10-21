using Default;
using InLoox.ODataClient.Data.BusinessObjects;
using InLoox.ODataClient.Extensions;
using Microsoft.OData.Client;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace InLooxShared.Odata
{
    public class TaskClient : PatchClient<WorkPackageView>
    {
        public TaskClient(OdClient client) : base(client)
        {
        }

        public async Task Delete(string name)
        {
            var ctx = _client.GetContext();
            var query = GetEntityQuery(ctx, name);
            var list = await query.ExecuteAsync();
            ctx.DeleteObject(list.First());
        }

        protected override DataServiceQuery<WorkPackageView> GetEntityQuery(Container ctx, string name)
        {
            var query = ctx.workpackageview.Where(k => k.Name == name)
                .ToDataServiceQuery();
            return query;
        }

        protected override async Task<Tuple<string, object>> HandleProperties(Container ctx, WorkPackageView task,
            MemberInfo prop, string value)
        {
            switch (prop.Name)
            {
                case nameof(WorkPackageView.GroupName):
                    {
                        var groupQuery = ctx.group.Where(k => k.Name == value);
                        var group = await groupQuery.FirstOrDefaultSq();
                        if (group == null)
                            _client.Logger.WriteError($"group '{value}' not found, skipping");
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
                            _client.Logger.WriteError($"project with name '{value}' not found, skipping");
                        else
                            return new Tuple<string, object>(nameof(WorkPackageView.ProjectId),
                                project.ProjectId);
                        break;
                    }
                case nameof(WorkPackageView.PlanningReservationStatusName) when task.ProjectId == null:
                    _client.Logger.WriteError("task needs a project");
                    return null;
                case nameof(WorkPackageView.PlanningReservationStatusName):
                    {
                        var status = await ctx.planningreservationstatus
                            .Where(k => k.ProjectId == task.ProjectId && k.Name == value)
                            .FirstOrDefaultSq();

                        if (status == null)
                            _client.Logger.WriteError($"task status '{value}' not found, skipping");
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
