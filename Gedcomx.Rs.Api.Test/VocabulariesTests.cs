using FamilySearch.Api;
using Gedcomx.Model;
using Gx.Common;
using Gx.Rs.Api;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gedcomx.Rs.Api.Test
{
    [TestFixture]
    public class VocabulariesTests
    {
        private FamilySearchPlaces places;

        [TestFixtureSetUp]
        public void Initialize()
        {
            places = new FamilySearchStateFactory().NewPlacesState(new Uri("https://sandbox.familysearch.org/platform/collections/places"));
            places.AuthenticateViaOAuth2Password("sdktester", "1234sdkpass", "WCQY-7J1Q-GKVV-7DNM-SQ5M-9Q5H-JX3H-CMJK");
            places = (FamilySearchPlaces)places.Get();
        }

        [Test]
        public void TestReadVocabularyList()
        {
            var state = places.ReadPlaceTypes();
            Assert.IsNotNull(state);
            Assert.DoesNotThrow(() => state.IfSuccessful());
            var placeTypes = state.GetVocabElementList();
            Assert.IsNotNull(placeTypes);
            Assert.IsNotNull(placeTypes.Title);
            Assert.IsNotNull(placeTypes.Description);
            Assert.IsNotNull(placeTypes.Elements);
            var numPlaceTypes = placeTypes.Elements.Count;
            var randomNum = new Random().Next(0, numPlaceTypes + 1);
            var randomPlaceType = placeTypes.Elements[randomNum];
            Assert.IsNotNull(randomPlaceType);
            Assert.IsNotNull(randomPlaceType.Id);
            Assert.IsNotNull(randomPlaceType.Uri);
            Assert.IsNotNull(randomPlaceType.Labels);
            Assert.IsNotNull(randomPlaceType.Descriptions);
        }

        [Test]
        public void TestReadVocabularyTerm()
        {
            var placeTypes = places.ReadPlaceTypes();
            var list = placeTypes.GetVocabElementList();
            var numPlaceTypes = list.Elements.Count;
            var randomNum = new Random().Next(0, numPlaceTypes + 1);
            var randomPlaceType = list.Elements[randomNum];
            var state = places.ReadPlaceTypeById(randomPlaceType.Id);
            Assert.IsNotNull(state);
            var placeType = state.GetVocabElement();
            Assert.IsNotNull(placeType);
        }
    }
}
