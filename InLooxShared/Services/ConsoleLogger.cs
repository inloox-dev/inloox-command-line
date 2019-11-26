using InLooxShared.Interfaces;
using System;

namespace InLooxShared.Services
{
    public class ConsoleLogger : ILogger
    {
        public void WriteError(string text)
        {
            var defaultColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: " + text);
            Console.ForegroundColor = defaultColor;
        }

        public void WriteInfo(string text)
        {
            var defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Info: " + text);
            Console.ForegroundColor = defaultColor;
        }
    }
}
