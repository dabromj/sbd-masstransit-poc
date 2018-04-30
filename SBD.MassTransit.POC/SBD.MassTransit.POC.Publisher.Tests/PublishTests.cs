using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SBD.MassTransit.POC.Publisher.Tests.Helpers;
using System.Net;
using System.Threading.Tasks;

namespace SBD.MassTransit.POC.Publisher.Tests
{
    [TestClass]
    public class PublishTests
    {
        private readonly string _publishUrl;

        /// <summary>
        /// Test constructor
        /// </summary>
        public PublishTests()
        {
            _publishUrl = "http://localhost/api/message";
        }

        [TestMethod]
        [DeploymentItem(@"Samples\SaveAgency_Valid.json", "Samples")]
        public async Task PublishMessage_Success()
        {
            //Grab JSON data from file
            var data = System.IO.File.ReadAllText(@"Samples\SaveAgency_Valid.json");
            //Create message
            var message = JObject.Parse(data);            
            //Submit request to in-memory server
            var response = await HttpRequestManager.SubmitRequest(_publishUrl, null, HttpMethod.Post, JsonConvert.SerializeObject(message));
            //Check if response code 200
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        [DeploymentItem(@"Samples\SaveAgencyScheduled_Valid.json", "Samples")]
        public async Task PublishMessageScheduled_Success()
        {
            //Grab JSON data from file
            var data = System.IO.File.ReadAllText(@"Samples\SaveAgencyScheduled_Valid.json");
            //Create message
            var message = JObject.Parse(data);
            //Submit request to in-memory server
            var response = await HttpRequestManager.SubmitRequest(_publishUrl, null, HttpMethod.Post, JsonConvert.SerializeObject(message));
            //Check if response code 200
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
