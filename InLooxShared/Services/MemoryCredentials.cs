using InLooxShared.Interfaces;

namespace InLooxShared.Services
{
    public class MemoryCredentials : ISettings
    {
        public string AccessToken { get; set; }

        public string EndPoint { get; set; }
    }
}
