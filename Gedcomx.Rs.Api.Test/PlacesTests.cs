using FamilySearch.Api;
using FamilySearch.Api.Ft;
using Gx.Rs.Api;
using Gx.Rs.Api.Options;
using Gx.Rs.Api.Util;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gedcomx.Rs.Api.Test
{
    [TestFixture]
    public class PlacesTests
    {
        private FamilySearchPlaces places;

        [TestFixtureSetUp]
        public void Initialize()
        {

            places = new FamilySearchPlaces(true);
            places.AuthenticateViaOAuth2Password("sdktester", "1234sdkpass", "WCQY-7J1Q-GKVV-7DNM-SQ5M-9Q5H-JX3H-CMJK");
        }

        [Test]
        public void TestReadPlaceDescriptionChildren()
        {
            var query = new GedcomxPlaceSearchQueryBuilder().Name("Utah").TypeId("47");
            var results = places.SearchForPlaces(query, QueryParameter.Count(30));
            var description = results.ReadPlaceDescription(results.Entity.Entries.First());
            var state = description.ReadChildren();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.IsNotNull(state.Entity);
            Assert.IsNotNull(state.Entity.Places);
            Assert.Greater(state.Entity.Places.Count, 0);
        }
    }
}
