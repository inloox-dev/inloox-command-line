using InLooxShared.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace InLooxShared.Services
{
    public class JsonFileCredentials : ISettings
    {
        private const string FileName = "settings.json";

        public void LoadFromFile()
        {
            var content = File.ReadAllText(FileName);
            if (!(JsonConvert.DeserializeObject(content) is JObject obj))
                throw new JsonSerializationException($"Cant deserialize object, {nameof(obj)} is null");

            AccessToken = (string)obj["access_token"];
            EndPoint = (string)obj["endpoint"];
        }

        public string AccessToken { get; private set;}
        public string EndPoint { get; private set; }
    }
}
