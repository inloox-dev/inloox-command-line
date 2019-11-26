using InLoox.ODataClient.Data.BusinessObjects;
using InLooxShared.Odata;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using InLooxShared.Test.Services;

namespace InLooxShared.Test.Odata
{
    public class ClientTest
    {
        class TestSetup
        {
            public MockLogger Logger { get; set; }
            public OdClient Client { get; set; }
        }

        TestSetup GetClient()
        {
            var logger = new MockLogger();
            var mockCredentials = new MockCredentials();
            var client = new OdClient(logger, mockCredentials);

            return new TestSetup
            {
                Logger = logger,
                Client = client
            };
        }

        [Test]
        public void InLooxClient_Logon()
        {
            var setup = GetClient();
            var res = setup.Client.Logon();

            Assert.IsTrue(res);
            Assert.IsTrue(!string.IsNullOrWhiteSpace(setup.Client.Token));
            Assert.AreEqual(0, setup.Logger.Exceptions.Count);
        }

        [Test]
        public async Task InLooxClient_AddTask_Simple()
        {
            var setup = GetClient();
            setup.Client.Logon();

            var dict = new Dictionary<string, string>
            {
                {"Name","InLoox Test 1" },
                {"WorkAmount","11" }
            };

            await setup.Client.Tasks.AddOrUpdateByName(dict);

            Assert.AreEqual(0, setup.Logger.Exceptions.Count);
        }

        [Test]
        public async Task InLooxClient_AddTask_Update()
        {
            var setup = GetClient();
            setup.Client.Logon();

            var name = "InLoox Test 1.11";
            var dict = new Dictionary<string, string>
            {
                {"Name", name},
                {"WorkAmount","11" }
            };

            await setup.Client.Tasks.AddOrUpdateByName(dict);

            dict["WorkAmount"] = "12";
            dict["Name"] = name;
            await setup.Client.Tasks.AddOrUpdateByName(dict);

            Assert.AreEqual(0, setup.Logger.Exceptions.Count);
        }


        [Test]
        public async Task InLooxClient_RemoveTask()
        {
            var setup = GetClient();
            setup.Client.Logon();

            var name = "InLoox Test 1.13";
            var dict = new Dictionary<string, string>
            {
                {"Name", name},
                {"WorkAmount","11" }
            };

            await setup.Client.Tasks.AddOrUpdateByName(dict);
            await setup.Client.Tasks.Delete(name);

            Assert.AreEqual(0, setup.Logger.Exceptions.Count);
        }

        [Test]
        public async Task InLooxClient_AddTask_WithGroup()
        {
            var setup = GetClient();
            setup.Client.Logon();

            var dict = new Dictionary<string, string>
            {
                {nameof(WorkPackageView.Name),"InLoox Test 1" },
                {nameof(WorkPackageView.GroupName),"Wartung" }
            };

            await setup.Client.Tasks.AddOrUpdateByName(dict);

            Assert.AreEqual(0, setup.Logger.Exceptions.Count);
        }

        [Test]
        public async Task InLooxClient_AddTask_WithStatus()
        {
            var setup = GetClient();
            setup.Client.Logon();

            var dict = new Dictionary<string, string>
            {
                {nameof(WorkPackageView.Name),"InLoox Test 1" },
                {nameof(WorkPackageView.ProjectName),"InLoox Testphase" },
                {nameof(WorkPackageView.PlanningReservationStatusName),"Erledigt" }
            };

            await setup.Client.Tasks.AddOrUpdateByName(dict);

            Assert.AreEqual(0, setup.Logger.Exceptions.Count);
        }

        [Test]
        public async Task InLooxClient_AddTask_StartDateTime()
        {
            var setup = GetClient();
            setup.Client.Logon();

            var dict = new Dictionary<string, string>
            {
                {nameof(WorkPackageView.Name),"InLoox Test 1" },
                {nameof(WorkPackageView.StartDateTime),"10.11.2019 14:51:00+2" },
            };

            await setup.Client.Tasks.AddOrUpdateByName(dict);

            Assert.AreEqual(0, setup.Logger.Exceptions.Count);
        }
    }
}
