using FamilySearch.Api;
using FamilySearch.Api.Ft;
using Gx.Rs.Api;
using Gx.Rs.Api.Options;
using Gx.Rs.Api.Util;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Gedcomx.Rs.Api.Test
{
    [TestFixture]
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
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
            Assert.IsNotNull(state.Entity);
            Assert.IsNotNull(state.Entity.Places);
            Assert.Greater(state.Entity.Places.Count, 0);
        }

        [Test]
        public void TestReadPlaceType()
        {
            var state = places.ReadPlaceTypeById("143");
            var vocab = state.GetVocabElement();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
            Assert.AreEqual("143", vocab.Id);
        }

        [Test]
        public void TestReadPlaceTypeGroups()
        {
            var state = places.ReadPlaceTypeGroups();
            var list = state.GetVocabElementList();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
            Assert.IsNotNull(list);
            Assert.IsNotNull(list.Elements);
            Assert.Greater(list.Elements.Count, 0);
        }

        [Test]
        public void TestReadPlaceTypeGroup()
        {
            var state = places.ReadPlaceTypeGroupById("1");
            var list = state.GetVocabElementList();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
            Assert.IsNotNull(list);
            Assert.IsNotNull(list.Elements);
            Assert.Greater(list.Elements.Count, 0);
            Assert.AreEqual("1", list.Id);
        }

        [Test]
        public void TestReadPlaceDescription()
        {
            var query = new GedcomxPlaceSearchQueryBuilder().Name("Utah").TypeId("47");
            var results = places.SearchForPlaces(query, QueryParameter.Count(30));
            var state = results.ReadPlaceDescription(results.Entity.Entries.First());

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
            Assert.IsNotNull(state.Entity);
            Assert.IsNotNull(state.Entity.Places);
            Assert.Greater(state.Entity.Places.Count, 0);
        }

        [Test]
        public void TestSearchForPlaces()
        {
            var query = new GedcomxPlaceSearchQueryBuilder().Name("Utah").TypeId("47");
            var state = places.SearchForPlaces(query, QueryParameter.Count(30));

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
            Assert.IsNotNull(state.Entity);
            Assert.IsNotNull(state.Entity.Entries);
            Assert.Greater(state.Entity.Entries.Count, 0);
        }

        [Test]
        public void TestSearchForPlacesDirectlyUnderAJurisdiction()
        {
            var query = new GedcomxPlaceSearchQueryBuilder().Name("Paris").ParentId("393946");
            var state = places.SearchForPlaces(query, QueryParameter.Count(30));

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
            Assert.IsNotNull(state.Entity);
            Assert.IsNotNull(state.Entity.Entries);
            Assert.Greater(state.Entity.Entries.Count, 0);
        }

        [Test]
        public void TestSearchForPlacesUnderAJurisdiction()
        {
            var query = new GedcomxPlaceSearchQueryBuilder().Name("Paris").ParentId("329", false);
            var state = places.SearchForPlaces(query, QueryParameter.Count(30));

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
            Assert.IsNotNull(state.Entity);
            Assert.IsNotNull(state.Entity.Entries);
            Assert.Greater(state.Entity.Entries.Count, 0);
        }

        [Test]
        public void TestReadPlace()
        {
            var query = new GedcomxPlaceSearchQueryBuilder().Name("Utah").TypeId("47");
            var results = places.SearchForPlaces(query, QueryParameter.Count(30));
            var description = (FamilySearchPlaceDescriptionState)results.ReadPlaceDescription(results.Entity.Entries.First());
            var state = description.ReadPlace();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
            Assert.IsNotNull(state.Place);
        }

        [Test]
        public void TestReadPlaceTypes()
        {
            var state = places.ReadPlaceTypes();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
        }

        [Test]
        public void TestReadPlaceGroup()
        {
            var state = places.ReadPlaceGroupById("30");

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
            Assert.IsNotNull(state.PlaceGroup);
            Assert.Greater(state.PlaceGroup.Count, 0);
        }
    }
}
