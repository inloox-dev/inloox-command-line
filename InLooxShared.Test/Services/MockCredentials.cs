using InLooxShared.Interfaces;

namespace InLooxShared.Test.Services
{
    public class MockCredentials : IInLooxCredentials
    {
        public string GetUsername()
        {
            return "<REPLACE_USERNAME>";
        }

        public string GetPassword()
        {
            return "<REPLACE_PASSWORD>";
        }
    }
}
