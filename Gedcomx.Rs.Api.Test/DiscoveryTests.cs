using FamilySearch.Api;
using Gx.Rs.Api;
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
    public class DiscoveryTests
    {
        private CollectionState collection;
        private FamilySearchStateFactory factory;

        [OneTimeSetUp]
        public void Initialize()
        {
            factory = new FamilySearchStateFactory();
            collection = factory.NewCollectionState(new Uri("https://integration.familysearch.org/platform/collection"));
            collection.AuthenticateViaOAuth2Password(Resources.TestUserName, Resources.TestPassword, Resources.TestClientId);
        }

        [Test]
        public void TestReadFamilySearchCollections()
        {
            var state = collection.ReadSubcollections();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
            Assert.IsNotNull(state.Entity);
            Assert.IsNotNull(state.Entity.Collections);
            Assert.Greater(state.Entity.Collections.Count, 0);
        }

        [Test]
        public void TestReadFamilyTreeCollection()
        {
            var state = new CollectionState(new Uri(collection.GetLink("family-tree").Href));

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
            Assert.IsNotNull(state.Links);
            Assert.Greater(state.Links.Count, 0);
            Assert.IsNotNull(state.Entity);
            Assert.IsNotNull(state.Entity.Collections);
            Assert.Greater(state.Entity.Collections.Count, 0);
        }

        [Test]
        public void TestReadDateAuthority()
        {
            var subcollections = collection.ReadSubcollections();
            var state = factory.NewCollectionState(new Uri(subcollections.Collections.Single(x => x.Id == "FSDA").GetLink("self").Href));

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
            Assert.IsNotNull(state.Links);
            Assert.Greater(state.Links.Count, 0);
            Assert.IsNotNull(state.Entity);
            Assert.IsNotNull(state.Entity.Collections);
            Assert.Greater(state.Entity.Collections.Count, 0);
        }

        [Test]
        public void TestReadRootCollection()
        {
            Assert.DoesNotThrow(() => collection.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, collection.Response.StatusCode);
            Assert.IsNotNull(collection.Links);
            Assert.Greater(collection.Links.Count, 0);
            Assert.IsNotNull(collection.Entity);
            Assert.IsNotNull(collection.Entity.Collections);
            Assert.Greater(collection.Entity.Collections.Count, 0);
        }

        [Test]
        public void TestFamilySearchDiscussions()
        {
            var state = factory.NewCollectionState(new Uri(collection.GetLink("discussions").Href));

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
            Assert.IsNotNull(state.Links);
            Assert.Greater(state.Links.Count, 0);
            Assert.IsNotNull(state.Entity);
            Assert.IsNotNull(state.Entity.Collections);
            Assert.Greater(state.Entity.Collections.Count, 0);
        }

        [Test]
        public void TestReadFamilySearchHistoricalRecordsArchive()
        {
            var subcollections = collection.ReadSubcollections();
            var state = factory.NewCollectionState(new Uri(subcollections.Collections.Single(x => x.Id == "FSHRA").GetLink("self").Href));

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
            Assert.IsNotNull(state.Links);
            Assert.Greater(state.Links.Count, 0);
            Assert.IsNotNull(state.Entity);
            Assert.IsNotNull(state.Entity.Collections);
            Assert.Greater(state.Entity.Collections.Count, 0);
        }

        [Test]
        public void TestFamilySearchMemories()
        {
            var state = factory.NewCollectionState(new Uri(collection.GetLink("memories").Href));

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
            Assert.IsNotNull(state.Links);
            Assert.Greater(state.Links.Count, 0);
            Assert.IsNotNull(state.Entity);
            Assert.IsNotNull(state.Entity.Collections);
            Assert.Greater(state.Entity.Collections.Count, 0);
        }

        [Test]
        public void TestReadPlaceAuthority()
        {
            var state = factory.NewCollectionState(new Uri(collection.GetLink("places").Href));

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
            Assert.IsNotNull(state.Links);
            Assert.Greater(state.Links.Count, 0);
            Assert.IsNotNull(state.Entity);
            Assert.IsNotNull(state.Entity.Collections);
            Assert.Greater(state.Entity.Collections.Count, 0);
        }

        [Test]
        public void TestReadUserDefinedSources()
        {
            var state = factory.NewCollectionState(new Uri(collection.GetLink("source-box").Href));

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
            Assert.IsNotNull(state.Links);
            Assert.Greater(state.Links.Count, 0);
            Assert.IsNotNull(state.Entity);
            Assert.IsNotNull(state.Entity.Collections);
            Assert.Greater(state.Entity.Collections.Count, 0);
        }
    }
}
