using InLooxShared.Interfaces;
using System.Collections.Generic;

namespace InLooxShared.Test.Services
{
    public class MockLogger : ILogger
    {
        public List<string> Exceptions { get; } = new List<string>();
        public List<string> Infos { get; } = new List<string>();

        public void WriteError(string text)
        {
            Exceptions.Add(text);
        }

        public void WriteInfo(string text)
        {
            Infos.Add(text);
        }
    }
}
