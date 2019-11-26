using ConsoleTables;
using InLoox.ODataClient.Data.BusinessObjects;
using InLoox.ODataClient.Extensions;
using InLooxCmd.Definitions;
using InLooxCmd.DI;
using InLooxShared.Basics;
using InLooxShared.Definitions;
using InLooxShared.Odata;
using InLooxShared.Reflection;
using ManyConsole;
using Microsoft.OData.Client;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace InLooxCmd.CmdCommands
{
    public class ReadCommand : ConsoleCommand
    {
        Entity Entity { get; set; }
        string[] Columns { get; set; }

        public ReadCommand()
        {
            IsCommand("list", "");

            HasLongDescription("");

            HasOption("c|columns=", "columns separated by comma",
                t => Columns = t?.Split(','));

            AllowsAnyAdditionalArguments("entity type: project, task, ... defaults to project");

            Entity = Entity.Project;
        }

        public override int Run(string[] remainingArguments)
        {
            try
            {
                if (remainingArguments.Length > 0)
                    Entity = EnumParser.ParseFuzzy<Entity>(remainingArguments[0]);

                if (Columns == null)
                    Columns = GetDefaultColumns(Entity);

                var defaultcolor = Console.ForegroundColor;
                var client = StaticDI.GetDefaultClient();

                if (!client.Logon())
                {
                    Console.WriteLine("Username password wrong");
                    return ProcessResult.Error;
                }

                var query = GetQuery(client);
                Task.Run(() => PrintQuery(query)).Wait();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("done.");
                Console.ForegroundColor = defaultcolor;

                return ProcessResult.Success;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.StackTrace);

                return ProcessResult.Error;
            }
        }

        private static string[] GetDefaultColumns(Entity entity)
        {
            switch (entity)
            {
                case Entity.Project:
                    return new string[] { nameof(ProjectView.Name), nameof(ProjectView.StartDate) };
                case Entity.Task:
                    return new string[] { nameof(WorkPackageView.Name), nameof(WorkPackageView.StartDateTime) };
                case Entity.TimeTracking:
                    return new string[] { nameof(InLoox.ODataClient.Data.BusinessObjects.Action.DisplayName),
                        nameof(InLoox.ODataClient.Data.BusinessObjects.Action.StartDateTime) };
                default:
                    throw new NotImplementedException();
            }
        }

        private DataServiceQuery GetQuery(OdClient client)
        {
            var ctx = client.GetContext();
            switch (Entity)
            {
                case Entity.Project:
                    return ctx.projectview.OrderBy(k => k.Name)
                        .ToDataServiceQuery();
                case Entity.Task:
                    return ctx.workpackageview.OrderBy(k => k.Name)
                        .ToDataServiceQuery();
                case Entity.TimeTracking:
                    return ctx.actionview.OrderBy(k => k.DisplayName)
                        .ToDataServiceQuery();
                default:
                    throw new NotImplementedException();
            }
        }

        private async Task PrintQuery(DataServiceQuery query)
        {
            Console.WriteLine(Entity);
            var list = await query.ExecuteAsync();

            var elementType = query.GetType().GenericTypeArguments[0];

            var props = Columns.Select(k => Info.GetProperty(elementType, k))
                .Where(k => k != null);

            var table = new ConsoleTable(props.Select(k => k.Name).ToArray());
            foreach (var wk in list)
            {
                var line = props.Select(k => k.GetValue(wk));
                table.AddRow(line.ToArray());
            }

            table.Write();
        }
    }
}
