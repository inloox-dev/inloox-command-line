using ConsoleTables;
using InLoox.ODataClient.Data.BusinessObjects;
using InLooxCmd.Definitions;
using InLooxShared.Basics;
using InLooxShared.Definitions;
using InLooxShared.Reflection;
using ManyConsole;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InLooxCmd.CmdCommands
{
    public class PropertiesCommand : ConsoleCommand
    {
        public Entity Entity { get; private set; }
        public bool PrintType { get; private set; }
        public bool PrintAccess { get; private set; }

        public PropertiesCommand()
        {
            IsCommand("print-properties", "print properties of entities");

            HasOption("e|entity:", "entity to list: Project, Task or TimeTracking",
                t => Entity = EnumParser.ParseFuzzy<Entity>(t ?? nameof(Entity.Project)));

            HasOption("t|print-type:", "print type",
                k => PrintType = k == null || Convert.ToBoolean(k));

            HasOption("w|print-writeable:", "print if field is writeable",
                k => PrintAccess = k == null || Convert.ToBoolean(k));
        }

        public override int Run(string[] remainingArguments)
        {
            try
            {
                PrintPropeties(Entity);
                return ProcessResult.Success;
            }
            catch (Exception ex)
            {
                Exceptions.PrintException(ex);
                return ProcessResult.Error;
            }
        }

        private void PrintPropeties(Entity entity)
        {
            switch (entity)
            {
                case Entity.Project:
                    PrintProperties<ProjectView>();
                    break;
                case Entity.TimeTracking:
                    PrintProperties<ActionView>();
                    break;
                case Entity.Task:
                    PrintProperties<WorkPackageView>();
                    break;
            }
        }

        private void PrintProperties<T>()
        {
            var props = new List<string> { "Name" };
            if (PrintType)
                props.Add("Type");
            if (PrintAccess)
                props.Add("Writeable");

            var table = new ConsoleTable(props.ToArray());

            var properties = Info.GetPublicProperties(typeof(T));

            foreach (var prop in properties.OrderBy(k => k.Name))
            {
                var line = new List<string> { prop.Name };
                if (PrintType)
                    line.Add(prop.PropertyType.ToString());
                if (PrintAccess)
                    line.Add(prop.CanWrite.ToString());

                table.AddRow(line.ToArray());
            }

            table.Write();
        }
    }
}
