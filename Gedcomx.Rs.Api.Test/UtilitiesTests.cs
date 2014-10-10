using FamilySearch.Api.Util;
using Gx.Rs.Api;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gx.Rs.Api.Util;
using Gedcomx.Support;
using Newtonsoft.Json.Linq;
using FamilySearch.Api.Ft;
using Newtonsoft.Json;

namespace Gedcomx.Rs.Api.Test
{
    [TestFixture]
    public class UtilitiesTests
    {
        [Test]
        public void TestReadPersonWithMultiplePendingModificationsActivated()
        {
            var tree = new FamilySearchFamilyTree(true);
            var features = new List<String>();

            tree.AuthenticateViaOAuth2Password("sdktester", "1234sdkpass", "WCQY-7J1Q-GKVV-7DNM-SQ5M-9Q5H-JX3H-CMJK");

            // Get all the features that are pending
            IRestRequest request = new RestRequest()
                .Accept(MediaTypes.APPLICATION_JSON_TYPE)
                .Build("https://sandbox.familysearch.org/platform/pending-modifications", Method.GET);
            IRestResponse response = tree.Client.Handle(request);

            // Get each pending feature
            foreach (var kvp in JsonConvert.DeserializeObject<IDictionary<String, JToken>>(response.Content).Where(x => x.Key == "features"))
            {
                foreach (var feature in kvp.Value.ToArray().SelectMany(x => x.ToObject<IDictionary<String, JToken>>().Where(y => y.Key == "name").Select(z => z.Value.ToString())))
                {
                    features.Add(feature);
                }
            }

            // Add every pending feature to the tree's current client
            tree.Client.AddFilter(new ExperimentsFilter(features.ToArray()));

            var state = tree.AddPerson(TestBacking.GetCreateMalePerson());

            // Ensure a response came back
            Assert.IsNotNull(state);
            var requestedFeatures = String.Join(",", state.Request.GetHeaders().Get("X-FS-Feature-Tag").Select(x => x.Value.ToString()));
            // Ensure each requested feature was found in the request headers
            Assert.IsTrue(features.TrueForAll(x => requestedFeatures.Contains(x)));
        }

        [Test]
        public void TestReadPersonWithPendingModificationActivated()
        {
            var tree = new FamilySearchFamilyTree(true);
            // The default client is assumed to add a single pending feature (if it doesn't, this test will fail)
            var state = tree.AuthenticateViaOAuth2Password("sdktester", "1234sdkpass", "WCQY-7J1Q-GKVV-7DNM-SQ5M-9Q5H-JX3H-CMJK");

            Assert.IsNotNull(state);
            var requestedFeatures = String.Join(",", state.Request.GetHeaders().Get("X-FS-Feature-Tag").Select(x => x.Value.ToString()));
            Assert.IsNotNull(requestedFeatures);
            Assert.AreEqual(-1, requestedFeatures.IndexOf(","));
            Assert.Greater(requestedFeatures.Length, 0);
        }
    }
}
