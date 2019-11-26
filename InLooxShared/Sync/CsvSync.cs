using CsvHelper;
using InLooxShared.Definitions;
using InLooxShared.Odata;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace InLooxShared.Sync
{
    public class CsvSync
    {
        private readonly OdClient _client;
        private readonly Entity _entity;

        public CsvSync(OdClient client, Entity entity)
        {
            _client = client;
            _entity = entity;
        }

        public async Task Run(string filePath)
        {
            var rows = GetRows(filePath);
            foreach (var row in rows)
            {
                var dict = row.ToDictionary(k => k.Key, k => k.Value as string);
                await AddOrUpdate(dict);
            }
        }

        public async Task AddOrUpdate(Dictionary<string, string> dict)
        {
            switch (_entity)
            {
                case Entity.Task:
                    await _client.Tasks.AddOrUpdateByName(dict);
                    break;
                case Entity.Project:
                    throw new NotImplementedException();
                case Entity.TimeTracking:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }

        private static IEnumerable<IDictionary<string, object>> GetRows(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader))
            {
                var records = csv.GetRecords<dynamic>()
                    .Cast<IDictionary<string, object>>()
                    .ToList();

                return records;
            }
        }
    }
}
