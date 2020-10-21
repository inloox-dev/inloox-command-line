using Default;
using InLoox.ODataClient;
using InLooxShared.Interfaces;
using System;

namespace InLooxShared.Odata
{
    public class OdClient
    {
        public string AccessToken { get; }
        public Uri EndPointOdata { get; set; }
        public TaskClient Tasks { get; }
        public ProjectClient Projects { get; }
        public ILogger Logger { get; }

        private readonly ISettings _settings;

        public OdClient(ILogger logger, ISettings settings)
        {
            Logger = logger;
            _settings = settings;
            AccessToken = _settings.AccessToken;
            Tasks = new TaskClient(this);
            Projects = new ProjectClient(this);

            var endPoint = new Uri(_settings.EndPoint);
            EndPointOdata = new Uri(endPoint, "odata/");
        }

        public bool Logon()
        {
            return true;
        }

        public Container GetContext()
        {
            return ODataBasics.GetInLooxContext(EndPointOdata, AccessToken);
        }
    }
}
