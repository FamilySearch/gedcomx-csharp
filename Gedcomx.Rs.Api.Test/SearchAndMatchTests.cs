using FamilySearch.Api.Ft;
using Gx.Fs.Tree;
using Gx.Rs.Api.Options;
using Gx.Rs.Api.Util;
using NUnit.Framework;
using System;
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
            tree.AuthenticateViaOAuth2Password(Resources.TestUserName, Resources.TestPassword, Resources.TestClientId);
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
        public void TestReadPersonRecordMatches()
        {
            var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            var state = person.ReadMatches();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
            Assert.Greater(state.Results.Entries.Count, 0);
        }

        [Test]
        public void TestReadAllMatchStatusTypesPersonRecordMatches()
        {
            var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            var statuses = new QueryParameter("status", "pending", "accepted", "rejected");
            var state = person.ReadMatches(statuses);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
            Assert.Greater(state.Results.Entries.Count, 0);
        }

        [Test]
        public void TestReadHigherConfidencePersonAcceptedRecordMatches()
        {
            var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            var statuses = new QueryParameter("status", "accepted");
            var confidence = new QueryParameter("confidence", "4");
            var state = person.ReadMatches(statuses, confidence);

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

        [Test]
        public void TestReadPersonNotAMatchDeclarations()
        {
            var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            var matches = person.ReadMatches();
            person.AddNonMatch(tree.ReadPersonById(matches.Results.Entries[0].Id));
            var state = person.ReadNonMatches();
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
            Assert.IsNotNull(state.Results);
            Assert.IsNotNull(state.Results.Entries);
            Assert.Greater(state.Results.Entries.Count, 0);
            Assert.Greater(state.Results.Entries[0].Score, 0);
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
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
        }
    }
}
