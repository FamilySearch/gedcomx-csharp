using System;
using System.Linq;
using System.Net;

using FamilySearch.Api;

using Gx.Rs.Api;

using NUnit.Framework;

namespace Gedcomx.Rs.Api.Test
{
    [TestFixture]
    public class DiscoveryTests
    {
        private CollectionState collection;
        private FamilySearchStateFactory factory;

        [OneTimeSetUp]
        public void Initialize()
        {
            factory = new FamilySearchStateFactory();
            collection = factory.NewCollectionState(new Uri("https://api-integ.familysearch.org/platform/collection"));
            collection.AuthenticateViaOAuth2Password(Resources.TestUserName, Resources.TestPassword, Resources.TestClientId);
        }

        [Test]
        public void TestReadFamilySearchCollections()
        {
            var state = collection.ReadSubcollections();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(state.Entity, Is.Not.Null);
            Assert.That(state.Entity.Collections, Is.Not.Null);
            Assert.That(state.Entity.Collections, Is.Not.Empty);
        }

        [Test]
        public void TestReadFamilyTreeCollection()
        {
            var state = new CollectionState(new Uri(collection.GetLink("family-tree").Href));

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(state.AnyLinks, Is.True);
            Assert.That(state.Entity, Is.Not.Null);
            Assert.That(state.Entity.Collections, Is.Not.Null);
            Assert.That(state.Entity.Collections, Is.Not.Empty);
        }

        [Test]
        public void TestReadDateAuthority()
        {
            var subcollections = collection.ReadSubcollections();
            var state = factory.NewCollectionState(new Uri(subcollections.Collections.Single(x => x.Id == "FSDA").GetLink("self").Href));

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(state.AnyLinks, Is.True);
            Assert.That(state.Entity, Is.Not.Null);
            Assert.That(state.Entity.Collections, Is.Not.Null);
            Assert.That(state.Entity.Collections, Is.Not.Empty);
        }

        [Test]
        public void TestReadRootCollection()
        {
            Assert.DoesNotThrow(() => collection.IfSuccessful());
            Assert.That(collection.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(collection.AnyLinks, Is.True);
            Assert.That(collection.Entity, Is.Not.Null);
            Assert.That(collection.Entity.Collections, Is.Not.Null);
            Assert.That(collection.Entity.Collections, Is.Not.Empty);
        }

        [Test]
        public void TestFamilySearchDiscussions()
        {
            var state = factory.NewCollectionState(new Uri(collection.GetLink("discussions").Href));

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(state.AnyLinks, Is.True);
            Assert.That(state.Entity, Is.Not.Null);
            Assert.That(state.Entity.Collections, Is.Not.Null);
            Assert.That(state.Entity.Collections, Is.Not.Empty);
        }

        [Test, Category("AccountNeeded")]
        public void TestReadFamilySearchHistoricalRecordsArchive()
        {
            var subcollections = collection.ReadSubcollections();
            var state = factory.NewCollectionState(new Uri(subcollections.Collections.Single(x => x.Id == "FSHRA").GetLink("self").Href));

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(state.AnyLinks, Is.True);
            Assert.That(state.Entity, Is.Not.Null);
            Assert.That(state.Entity.Collections, Is.Not.Null);
            Assert.That(state.Entity.Collections, Is.Not.Empty);
        }

        [Test]
        public void TestFamilySearchMemories()
        {
            var state = factory.NewCollectionState(new Uri(collection.GetLink("memories").Href));

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(state.AnyLinks, Is.True);
            Assert.That(state.Entity, Is.Not.Null);
            Assert.That(state.Entity.Collections, Is.Not.Null);
            Assert.That(state.Entity.Collections, Is.Not.Empty);
        }

        [Test]
        public void TestReadPlaceAuthority()
        {
            var state = factory.NewCollectionState(new Uri(collection.GetLink("places").Href));

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(state.AnyLinks, Is.True);
            Assert.That(state.Entity, Is.Not.Null);
            Assert.That(state.Entity.Collections, Is.Not.Null);
            Assert.That(state.Entity.Collections, Is.Not.Empty);
        }

        [Test]
        public void TestReadUserDefinedSources()
        {
            var state = factory.NewCollectionState(new Uri(collection.GetLink("source-box").Href));

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(state.AnyLinks, Is.True);
            Assert.That(state.Entity, Is.Not.Null);
            Assert.That(state.Entity.Collections, Is.Not.Null);
            Assert.That(state.Entity.Collections, Is.Not.Empty);
        }
    }
}
