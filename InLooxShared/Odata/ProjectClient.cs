using Default;
using InLoox.ODataClient.Data.BusinessObjects;
using InLoox.ODataClient.Extensions;
using Microsoft.OData.Client;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace InLooxShared.Odata
{
    public class ProjectClient : PatchClient<ProjectView>
    {

        public ProjectClient(OdClient client) : base(client)
        {
        }

        protected override DataServiceQuery<ProjectView> GetEntityQuery(Container ctx, string name)
        {
            var query = ctx.projectview.Where(k => k.Name == name)
                .ToDataServiceQuery();
            return query;
        }

        protected override async Task<Tuple<string, object>> HandleProperties(Container ctx, ProjectView task,
            MemberInfo prop, string value)
        {
            await Task.Delay(10);

            switch (prop.Name)
            {
                default:
                    return null;
            }
        }
    }
}
