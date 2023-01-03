using System;

using FamilySearch.Api;

using Gedcomx.Model;

using Gx.Rs.Api;

using NUnit.Framework;

namespace Gedcomx.Rs.Api.Test
{
    [TestFixture, Category("AccountNeeded")]
    public class VocabulariesTests
    {
        private FamilySearchPlaces places;
        private VocabElementListState placeTypes;
        private VocabElementList list;

        [OneTimeSetUp]
        public void Initialize()
        {
            places = new FamilySearchStateFactory().NewPlacesState(new Uri("https://api-integ.familysearch.org/platform/collections/places"));
            places.AuthenticateViaOAuth2Password(Resources.TestUserName, Resources.TestPassword, Resources.TestClientId);
            places = (FamilySearchPlaces)places.Get();
            placeTypes = places.ReadPlaceTypes();
            list = placeTypes.GetVocabElementList();
        }

        [Test]
        public void TestReadVocabularyList()
        {
            var state = placeTypes;
            Assert.That(state, Is.Not.Null);
            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(list, Is.Not.Null);
            Assert.That(list.Title, Is.Not.Null);
            Assert.That(list.Description, Is.Not.Null);
            Assert.That(list.Elements, Is.Not.Null);
            var numPlaceTypes = list.Elements.Count;
            var randomNum = new Random().Next(0, numPlaceTypes + 1);
            var randomPlaceType = list.Elements[randomNum];
            Assert.That(randomPlaceType, Is.Not.Null);
            Assert.That(randomPlaceType.Id, Is.Not.Null);
            Assert.That(randomPlaceType.Uri, Is.Not.Null);
            Assert.That(randomPlaceType.Labels, Is.Not.Null);
            Assert.That(randomPlaceType.Descriptions, Is.Not.Null);
        }

        [Test]
        public void TestReadVocabularyTerm()
        {
            var numPlaceTypes = list.Elements.Count;
            var randomNum = new Random().Next(0, numPlaceTypes + 1);
            var randomPlaceType = list.Elements[randomNum];
            var state = places.ReadPlaceTypeById(randomPlaceType.Id);
            Assert.That(state, Is.Not.Null);
            var placeType = state.GetVocabElement();
            Assert.That(placeType, Is.Not.Null);
        }
    }
}
