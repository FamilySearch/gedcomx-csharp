using FamilySearch.Api.Ft;
using Gx.Fs.Tree;
//using Gx.Rs.Api;
using Gx.Rs.Api.Options;
using NUnit.Framework;
using System.Net;

namespace Gedcomx.Rs.Api.Test
{
    [TestFixture]
    public class SearchAndMatchTests
    {
        private FamilySearchFamilyTree tree;

        [TestFixtureSetUp]
        public void Initialize()
        {
            tree = new FamilySearchFamilyTree(true);
            tree.AuthenticateViaOAuth2Password("sdktester", "1234sdkpass", "WCQY-7J1Q-GKVV-7DNM-SQ5M-9Q5H-JX3H-CMJK");
            Assert.DoesNotThrow(() => tree.IfSuccessful());
            Assert.IsNotNullOrEmpty(tree.CurrentAccessToken);
        }

        [Test]
        public void TestReadPersonPossibleDuplicates()
        {
            var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            var state = person.ReadMatches();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
            Assert.Greater(state.Results.Entries.Count, 0);
        }

        [Test]
        [Ignore("Service is unavailable (503). Need to revisit and validate.")]
        public void TestReadPersonRecordMatches()
        {
            var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            var query = new QueryParameter("collection", "https://familysearch.org/platform/collections/records");
            var state = person.ReadMatches(query);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
            Assert.Greater(state.Results.Entries.Count, 0);
        }

        [Test]
        [Ignore("Service is unavailable (503). Need to revisit and validate.")]
        public void TestReadAllMatchStatusTypesPersonRecordMatches()
        {
            var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            var query = new QueryParameter("collection", "https://familysearch.org/platform/collections/records");
            var statuses = new QueryParameter("status", "pending", "accepted", "rejected");
            var state = person.ReadMatches(query, statuses);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
            Assert.Greater(state.Results.Entries.Count, 0);
        }

        [Test]
        [Ignore("Service is unavailable (503). Need to revisit and validate.")]
        public void TestReadHigherConfidencePersonAcceptedRecordMatches()
        {
            var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            var query = new QueryParameter("collection", "https://familysearch.org/platform/collections/records");
            var statuses = new QueryParameter("status", "accepted");
            var confidence = new QueryParameter("confidence", "4");
            var state = person.ReadMatches(query, statuses, confidence);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
            Assert.Greater(state.Results.Entries.Count, 0);
        }

        [Test]
        public void TestUpdateMatchStatusForPersonRecordMatches()
        {
            var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson());
            var matches = person.ReadMatches();
            var collection = new QueryParameter("collection", "https://familysearch.org/platform/collections/records");
            var state = matches.UpdateMatchStatus(matches.Results.Entries[0], MatchStatus.Pending, collection);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.NoContent, state.Response.StatusCode);
        }
    }
}
