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
        private VocabElementListState placeTypes;
        private VocabElementList list;

        [OneTimeSetUp]
        public void Initialize()
        {
            places = new FamilySearchStateFactory().NewPlacesState(new Uri("https://integration.familysearch.org/platform/collections/places"));
            places.AuthenticateViaOAuth2Password(Resources.TestUserName, Resources.TestPassword, Resources.TestClientId);
            places = (FamilySearchPlaces)places.Get();
            placeTypes = places.ReadPlaceTypes();
            list = placeTypes.GetVocabElementList();
        }

        [Test]
        public void TestReadVocabularyList()
        {
            var state = placeTypes;
            Assert.IsNotNull(state);
            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.IsNotNull(list);
            Assert.IsNotNull(list.Title);
            Assert.IsNotNull(list.Description);
            Assert.IsNotNull(list.Elements);
            var numPlaceTypes = list.Elements.Count;
            var randomNum = new Random().Next(0, numPlaceTypes + 1);
            var randomPlaceType = list.Elements[randomNum];
            Assert.IsNotNull(randomPlaceType);
            Assert.IsNotNull(randomPlaceType.Id);
            Assert.IsNotNull(randomPlaceType.Uri);
            Assert.IsNotNull(randomPlaceType.Labels);
            Assert.IsNotNull(randomPlaceType.Descriptions);
        }

        [Test]
        public void TestReadVocabularyTerm()
        {
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
