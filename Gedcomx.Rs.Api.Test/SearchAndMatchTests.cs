using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;

using FamilySearch.Api.Ft;
using FamilySearch.Api.Util;

using Gx.Fs.Tree;
using Gx.Rs.Api;
using Gx.Rs.Api.Options;
using Gx.Rs.Api.Util;

using NUnit.Framework;

namespace Gedcomx.Rs.Api.Test
{
    [TestFixture, Category("AccountNeeded")]
    public class SearchAndMatchTests
    {
        private FamilySearchFamilyTree tree;
        private List<GedcomxApplicationState> cleanup;

        [OneTimeSetUp]
        public void Initialize()
        {
            tree = new FamilySearchFamilyTree(true);
            tree.AuthenticateViaOAuth2Password(Resources.TestUserName, Resources.TestPassword, Resources.TestClientId);
            cleanup = new List<GedcomxApplicationState>();
            Assert.DoesNotThrow(() => tree.IfSuccessful());
            Assert.That(tree.CurrentAccessToken, Is.Not.Null);
            Assert.That(tree.CurrentAccessToken, Is.Not.Empty);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            foreach (var state in cleanup)
            {
                state.Delete();
            }
        }

        [Test]
        public void TestReadPersonPossibleDuplicates()
        {
            var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(person);
            var state = person.ReadMatches();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(state.Results.Entries, Is.Not.Empty);
        }

        [Test]
        public void TestReadPersonRecordMatches()
        {
            var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(person);
            var query = new QueryParameter("collection", "https://www.familysearch.org/platform/collections/records");
            var state = person.ReadMatches(query);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            Assert.IsNull(state.Results);
        }

        [Test]
        public void TestReadAllMatchStatusTypesPersonRecordMatches()
        {
            var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(person);
            var statuses = new QueryParameter("status", "pending", "accepted", "rejected");
            var state = person.ReadMatches(statuses);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(state.Results.Entries, Is.Not.Empty);
        }

        [Test]
        public void TestReadHigherConfidencePersonAcceptedRecordMatches()
        {
            var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(person);
            var statuses = new QueryParameter("status", "accepted");
            var confidence = new QueryParameter("confidence", "4");
            var state = person.ReadMatches(statuses, confidence);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(state.Results.Entries, Is.Not.Empty);
        }

        [Test]
        public void TestUpdateMatchStatusForPersonRecordMatches()
        {
            var collection = FamilySearchOptions.Collection("https://www.familysearch.org/platform/collections/records");
            var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson());
            cleanup.Add(person);
            var matches = person.ReadMatches();
            var state = matches.UpdateMatchStatus(matches.Results.Entries[0], MatchStatus.Accepted, collection);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test]
        public void TestReadPersonNotAMatchDeclarations()
        {
            var person1 = tree.AddPerson(TestBacking.GetCreateMalePerson());
            cleanup.Add(person1);
            var person2 = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(person2);
            Thread.Sleep(10000); // This is to ensure the matching system on the server has time to recognize the two new duplicates
            var matches = person2.ReadMatches();
            var entry = matches.Results.Entries.FirstOrDefault();
            var id = entry.Id;
            var match = tree.ReadPersonById(id);
            person2.AddNonMatch(match);
            var state = person2.ReadNonMatches();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void TestReadMatchScoresForPersons()
        {
            var query = new GedcomxPersonSearchQueryBuilder()
                .GivenName("GedcomX")
                .Surname("User")
                .Gender("Male")
                .BirthDate("June 1800")
                .BirthPlace("Provo, Utah, Utah, United States")
                .DeathDate("July 14, 1900")
                .DeathPlace("Provo, Utah, Utah, United States");
            var state = tree.SearchForPersonMatches(query);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Results, Is.Not.Null);
            Assert.That(state.Results.Entries, Is.Not.Null);
            Assert.That(state.Results.Entries, Is.Not.Empty);
            Assert.That(state.Results.Entries[0].Score, Is.GreaterThan(0));
        }

        [Test]
        public void TestSearchForPersonMatches()
        {
            var query = new GedcomxPersonSearchQueryBuilder()
                .FatherSurname("Heaton")
                .SpouseSurname("Cox")
                .Surname("Heaton")
                .GivenName("Israel")
                .BirthPlace("Orderville, UT")
                .DeathDate("29 August 1936")
                .DeathPlace("Kanab, Kane, UT")
                .SpouseGivenName("Charlotte")
                .MotherGivenName("Clarissa")
                .MotherSurname("Hoyt")
                .Gender("Male")
                .BirthDate("30 January 1880")
                .FatherGivenName("Jonathan");
            var state = tree.SearchForPersonMatches(query);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}
