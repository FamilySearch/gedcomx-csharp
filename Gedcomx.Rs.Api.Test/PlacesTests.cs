using System.Linq;
using System.Net;

using FamilySearch.Api;

using Gx.Rs.Api.Options;
using Gx.Rs.Api.Util;

using NUnit.Framework;

namespace Gedcomx.Rs.Api.Test
{
    [TestFixture, Category("AccountNeeded")]
    public class PlacesTests
    {
        private FamilySearchPlaces places;

        [OneTimeSetUp]
        public void Initialize()
        {

            places = new FamilySearchPlaces(true);
            places.AuthenticateViaOAuth2Password(Resources.TestUserName, Resources.TestPassword, Resources.TestClientId);
        }

        [Test]
        public void TestReadPlaceDescriptionChildren()
        {
            var query = new GedcomxPlaceSearchQueryBuilder().Name("Utah").TypeId("47");
            var results = places.SearchForPlaces(query, QueryParameter.Count(30));
            var description = results.ReadPlaceDescription(results.Entity.Entries.First());
            var state = description.ReadChildren();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(state.Entity, Is.Not.Null);
            Assert.That(state.Entity.Places, Is.Not.Null);
            Assert.That(state.Entity.Places, Is.Not.Empty);
        }

        [Test]
        public void TestReadPlaceType()
        {
            var state = places.ReadPlaceTypeById("143");
            var vocab = state.GetVocabElement();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(vocab.Id, Is.EqualTo("143"));
        }

        [Test]
        public void TestReadPlaceTypeGroups()
        {
            var state = places.ReadPlaceTypeGroups();
            var list = state.GetVocabElementList();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(list, Is.Not.Null);
            Assert.That(list.Elements, Is.Not.Null);
            Assert.That(list.Elements, Is.Not.Empty);
        }

        [Test]
        public void TestReadPlaceTypeGroup()
        {
            var state = places.ReadPlaceTypeGroupById("1");
            var list = state.GetVocabElementList();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(list, Is.Not.Null);
            Assert.That(list.Elements, Is.Not.Null);
            Assert.That(list.Elements, Is.Not.Empty);
            Assert.That(list.Id, Is.EqualTo("1"));
        }

        [Test]
        public void TestReadPlaceDescription()
        {
            var query = new GedcomxPlaceSearchQueryBuilder().Name("Utah").TypeId("47");
            var results = places.SearchForPlaces(query, QueryParameter.Count(30));
            var state = results.ReadPlaceDescription(results.Entity.Entries.First());

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(state.Entity, Is.Not.Null);
            Assert.That(state.Entity.Places, Is.Not.Null);
            Assert.That(state.Entity.Places, Is.Not.Empty);
        }

        [Test]
        public void TestSearchForPlaces()
        {
            var query = new GedcomxPlaceSearchQueryBuilder().Name("Utah").TypeId("47");
            var state = places.SearchForPlaces(query, QueryParameter.Count(30));

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(state.Entity, Is.Not.Null);
            Assert.That(state.Entity.Entries, Is.Not.Null);
            Assert.That(state.Entity.Entries, Is.Not.Empty);
        }

        [Test]
        public void TestSearchForPlacesDirectlyUnderAJurisdiction()
        {
            var query = new GedcomxPlaceSearchQueryBuilder().Name("Paris").ParentId("393946");
            var state = places.SearchForPlaces(query, QueryParameter.Count(30));

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(state.Entity, Is.Not.Null);
            Assert.That(state.Entity.Entries, Is.Not.Null);
            Assert.That(state.Entity.Entries, Is.Not.Empty);
        }

        [Test]
        public void TestSearchForPlacesUnderAJurisdiction()
        {
            var query = new GedcomxPlaceSearchQueryBuilder().Name("Paris").ParentId("329", false);
            var state = places.SearchForPlaces(query, QueryParameter.Count(30));

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(state.Entity, Is.Not.Null);
            Assert.That(state.Entity.Entries, Is.Not.Null);
            Assert.That(state.Entity.Entries, Is.Not.Empty);
        }

        [Test]
        public void TestReadPlace()
        {
            var query = new GedcomxPlaceSearchQueryBuilder().Name("Utah").TypeId("47");
            var results = places.SearchForPlaces(query, QueryParameter.Count(30));
            var description = (FamilySearchPlaceDescriptionState)results.ReadPlaceDescription(results.Entity.Entries.First());
            var state = description.ReadPlace();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(state.Place, Is.Not.Null);
        }

        [Test]
        public void TestReadPlaceTypes()
        {
            var state = places.ReadPlaceTypes();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void TestReadPlaceGroup()
        {
            var state = places.ReadPlaceGroupById("30");

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(state.PlaceGroup, Is.Not.Null);
            Assert.That(state.PlaceGroup, Is.Not.Empty);
        }
    }
}
