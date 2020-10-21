using InLooxCmd.DI;
using InLooxShared.Basics;
using InLooxShared.Definitions;
using InLooxShared.Sync;
using ManyConsole;
using System;

namespace InLooxCmd.CmdCommands
{
    public class ImportCsvCommand : ConsoleCommand
    {
        private const int SuccessState = 0;
        private const int FailureState = 2;

        public string FileLocation { get; set; }
        public bool KeepConsoleOpen { get; set; }
        public Entity Entity { get; set; }

        public ImportCsvCommand()
        {
            IsCommand("import-csv", "import a csv utility to InLoox");

            HasLongDescription("Import a csv file and create/update InLoox objects");

            HasRequiredOption("f|file=", "The full path of the csv file to import",
                p => FileLocation = p);

            HasOption("k|keep-open", "keeps console open",
                t => KeepConsoleOpen = t == null || Convert.ToBoolean(t));

            HasOption("e|entity=", "entity to list: Project, Task or TimeTracking",
                t => Entity = Enum.Parse<Entity>(t ?? nameof(Entity.Task)));
        }

        public override int Run(string[] remainingArguments)
        {
            try
            {
                if (remainingArguments.Length > 0)
                    Entity = EnumParser.ParseFuzzy<Entity>(remainingArguments[0]);

                var defaultColor = Console.ForegroundColor;

                var client = StaticDI.GetDefaultClient();
                if (!client.Logon())
                {
                    Console.WriteLine("Username password wrong");
                    return FailureState;
                }

                var sync = new CsvSync(client, Entity);
                try
                {
                    sync.Run(FileLocation).Wait();
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error:");
                    TraverseExceptions(ex);
                    Console.ForegroundColor = defaultColor;
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("done.");
                Console.ForegroundColor = defaultColor;

                if (KeepConsoleOpen)
                    Console.ReadLine();

                return SuccessState;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.StackTrace);

                return FailureState;
            }
        }

        private void TraverseExceptions(Exception ex)
        {
            Console.WriteLine(ex.Message);
            if (ex.InnerException != null)
                TraverseExceptions(ex.InnerException);
        }
    }
}
