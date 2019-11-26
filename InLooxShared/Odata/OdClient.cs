using Default;
using InLoox.ODataClient;
using InLooxShared.Interfaces;
using System;

namespace InLooxShared.Odata
{
    public class OdClient
    {
        public string Token { get; set; }
        public Uri EndPointOdata { get; set; }

        private readonly ILogger _logger;
        private readonly IInLooxCredentials _inlooxCredentials;

        public OdClient(ILogger logger, IInLooxCredentials credentials)
        {
            _logger = logger;
            _inlooxCredentials = credentials;
            Tasks = new TaskClient(this);
        }

        public TaskClient Tasks { get; }

        public bool Logon()
        {
            var endPoint = new Uri("https://app.inlooxnow.com/");
            EndPointOdata = new Uri(endPoint, "odata/");

            var username = _inlooxCredentials.GetUsername();
            var password = _inlooxCredentials.GetPassword();

            var tokenResult = ODataBasics.GetToken(endPoint, username, password)
                .Result;

            if (!string.IsNullOrWhiteSpace(tokenResult.AccessToken))
            {
                Token = tokenResult.AccessToken;
                return true;
            }

            var text = $"cant login '{tokenResult.Error}' description: '{tokenResult.ErrorDescription}'";
            _logger.WriteError(text);
            return false;
        }

        public Container GetContext()
        {
            return ODataBasics.GetInLooxContext(EndPointOdata, Token);
        }

        public ILogger GetLogger()
        {
            return _logger;
        }
    }
}
