using InLooxShared.Interfaces;
using System;

namespace InLooxShared.Test.Services
{
    public class TestSettings : ISettings
    {
        public string AccessToken => Environment.GetEnvironmentVariable("InLooxAccessToken");

        public string EndPoint => "https://app.inlooxnow.de/";

    }
}
