using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Gx.Rs.Api;
using Gx.Rs.Api.Util;

namespace Gedcomx.Rs.Api.Test
{
    [TestFixture]
    public class PersonSearchTests
    {
        private static readonly String INTEGRATION_URI = "https://integration.familysearch.org/platform/collections/tree";
        private CollectionState collection;

        [OneTimeSetUp]
        public void Initialize()
        {
            collection = new CollectionState(new Uri(INTEGRATION_URI));
            collection.AuthenticateViaOAuth2Password(Resources.TestUserName, Resources.TestPassword, Resources.TestClientId);
            Assert.DoesNotThrow(() => collection.IfSuccessful());
            Assert.IsNotNull(collection.CurrentAccessToken);
			Assert.IsNotEmpty(collection.CurrentAccessToken);
		}

        [Test]
        public void TestReadNextPageOfSearchResults()
        {
            GedcomxPersonSearchQueryBuilder query = new GedcomxPersonSearchQueryBuilder()
                .Name("John Smith")
                .BirthDate("1 January 1900")
                .FatherName("Peter Smith");

            PersonSearchResultsState results = collection.SearchForPersons(query);

            var state = (PersonSearchResultsState)results.ReadNextPage();
            Assert.DoesNotThrow(() => state.IfSuccessful());
            PersonState person = state.ReadPerson(results.Results.Entries.FirstOrDefault());

            Assert.IsNotNull(person);
            Assert.DoesNotThrow(() => person.IfSuccessful());
        }

        [Test]
        public void TestSearchPersons()
        {
            GedcomxPersonSearchQueryBuilder query = new GedcomxPersonSearchQueryBuilder()
                .MotherGivenName("Clarissa")
                .FatherSurname("Heaton")
                .MotherSurname("Hoyt")
                .Surname("Heaton")
                .GivenName("Israel")
                .FatherGivenName("Jonathan");

            var state = collection.SearchForPersons(query);

            Assert.DoesNotThrow(() => state.IfSuccessful());
        }

        [Test]
        public void TestSearchPersonsWithWarningsAndErrors()
        {
            GedcomxPersonSearchQueryBuilder query = new GedcomxPersonSearchQueryBuilder()
                .Param("givenNameMisspelled", "Israel");
            var state = collection.SearchForPersons(query);

            Assert.IsTrue(state.Warnings.Count > 0);
        }
    }
}
