using InLooxShared.Interfaces;
using System.Collections.Generic;

namespace InLooxShared.Services
{
    public class MemoryLogger : ILogger
    {
        public List<string> Exceptions { get; set; } = new List<string>();
        public List<string> Infos { get; set; } = new List<string>();

        public void WriteError(string text)
        {
            Exceptions.Add(text);
        }

        public void WriteInfo(string text)
        {
            Infos.Add(text);
        }

        public void Clear()
        {
            Exceptions.Clear();
            Infos.Clear();
        }
    }
}
