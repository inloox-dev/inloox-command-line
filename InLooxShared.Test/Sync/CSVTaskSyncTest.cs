using NUnit.Framework;
using System.IO;

namespace InLooxShared.Test.Sync
{
    public class CsvTaskSync
    {
        [Test]
        public void CSVTaskSync_Sync()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Mappe1.csv");
            //var sync = new Sync.CSVTaskSync();
            //sync.Run(path);
        }
    }
}