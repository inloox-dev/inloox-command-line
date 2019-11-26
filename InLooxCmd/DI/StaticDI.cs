using InLooxShared.Odata;
using InLooxShared.Services;

namespace InLooxCmd.DI
{
    public class StaticDI
    {
        public static OdClient GetDefaultClient()
        {
            var logger = new ConsoleLogger();
            var credentials = new JsonFileCredentials();
            credentials.LoadFromFile();

            var client = new OdClient(logger, credentials);
            return client;
        }
    }
}
