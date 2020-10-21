using Default;
using InLoox.ODataClient;
using InLooxShared.Reflection;
using Microsoft.OData.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace InLooxShared.Odata
{
    public abstract class PatchClient<T> where T : new()
    {
        protected readonly OdClient _client;

        protected PatchClient(OdClient client)
        {
            _client = client;
        }

        public async Task AddOrUpdateByName(IDictionary<string, string> dict)
        {
            if (!dict.ContainsKey("Name"))
                throw new ArgumentException(nameof(dict) + " should contain 'Name'");

            var name = dict["Name"];
            var ctx = _client.GetContext();
            var query = GetEntityQuery(ctx, name);
            var dataCollection = await ODataBasics.GetDSCollection(query);
            var entity = dataCollection.FirstOrDefault();

            if (entity == null)
            {
                entity = new T();
                dataCollection.Add(entity);
            }

            await Update(ctx, entity, dict);

            await ctx.SaveChangesAsync(SaveChangesOptions.PostOnlySetProperties);
        }

        protected abstract DataServiceQuery<T> GetEntityQuery(Container ctx, string name);

        protected abstract Task<Tuple<string, object>> HandleProperties(Container ctx, T task,
           MemberInfo prop, string value);

        private async Task Update(Container ctx, T entity, IDictionary<string, string> dict)
        {
            foreach (var entry in dict)
            {
                var prop = Info.GetProperty<T>(entry.Key);
                if (prop == null)
                    continue;
                try
                {
                    var ret = await HandleProperties(ctx, entity, prop, entry.Value);
                    if (ret != null)
                    {
                        prop = Info.GetProperty<T>(ret.Item1);
                        prop.SetValue(entity, ret.Item2);
                    }
                    else
                    {
                        var convertedValue = StringToObject.Parse(prop.PropertyType, entry.Value);
                        prop = Info.GetProperty<T>(entry.Key);
                        prop.SetValue(entity, convertedValue);
                    }
                }
                catch (TargetException ex)
                {
                    _client.Logger.WriteError($"TargetException: {ex.Message}");
                }
            }
        }
    }
}
