using System;
using InLooxShared.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace InLooxShared.Services
{
    public class JsonFileCredentials : IInLooxCredentials
    {
        private const string FileName = "credentials.json";
        private string _password = "";
        private string _username = "";

        public void LoadFromFile()
        {
            var content = File.ReadAllText(FileName);
            if (!(JsonConvert.DeserializeObject(content) is JObject obj))
                throw new NullReferenceException($"Cant deserialize object, {nameof(obj)} is null");

            _username = (string)obj["username"];
            _password = (string)obj["password"];
        }

        public string GetPassword()
        {
            return _password;
        }

        public string GetUsername()
        {
            return _username;
        }
    }
}
