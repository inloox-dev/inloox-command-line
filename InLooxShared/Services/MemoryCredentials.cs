using InLooxShared.Interfaces;

namespace InLooxShared.Services
{
    public class MemoryCredentials : IInLooxCredentials
    {
        public string Password;
        public string Username;

        public string GetPassword()
        {
            return Password;
        }

        public string GetUsername()
        {
            return Username;
        }
    }
}
