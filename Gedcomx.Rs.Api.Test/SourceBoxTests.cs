using FamilySearch.Api.Ft;
using Gx.Rs.Api;
using Gx.Source;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Gx.Rs.Api.Util;
using Gx.Records;
using Gedcomx.Support;
using FamilySearch.Api;
using Gx.Rs.Api.Options;

namespace Gedcomx.Rs.Api.Test
{
    [TestFixture]
    public class SourceBoxTests
    {
        private CollectionState collection;
        private CollectionsState subcollections;

        [TestFixtureSetUp]
        public void Initialize()
        {
            collection = new FamilySearchCollectionState(new Uri("https://sandbox.familysearch.org/platform/collections/sources"));
            collection.AuthenticateViaOAuth2Password(Resources.TestUserName, Resources.TestPassword, Resources.TestClientId);
            subcollections = (CollectionsState)collection.ReadSubcollections().Get();
        }

        [Test]
        [Ignore("Unable to query all sources for all collections at this time.")]
        public void TestReadAllSourcesOfAllUserDefinedCollectionsOfASpecificUser()
        {
            // Get the root collection
            var subcollection = subcollections.ReadCollection(subcollections.Entity.Collections.Single(x => string.IsNullOrEmpty(x.Title)));
            subcollection = (CollectionState)subcollection.Get();
            var state = subcollection.ReadSourceDescriptions();

            Assert.DoesNotThrow(() => subcollection.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, subcollection.Response.StatusCode);
            Assert.IsNotNull(subcollection.Entity.Collections);
            Assert.Greater(subcollection.Entity.Collections.Count, 0);
        }

        [Test]
        public void TestDeleteSourceDescriptionsFromAUserDefinedCollection()
        {
            var description = collection.AddSourceDescription(TestBacking.GetCreateSourceDescription());
            var state = description.Delete();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.NoContent, state.Response.StatusCode);
        }

        [Test]
        public void TestReadASpecificUsersSetOfUserDefinedCollections()
        {
            Assert.DoesNotThrow(() => subcollections.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, subcollections.Response.StatusCode);
            Assert.IsNotNull(subcollections.Collections);
            Assert.Greater(subcollections.Collections.Count, 0);
        }

        [Test]
        public void TestCreateUserDefinedCollection()
        {
            var state = collection.AddCollection(new Collection().SetTitle(Guid.NewGuid().ToString("n")));

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.Created, state.Response.StatusCode);

            state.Delete();
        }

        [Test]
        public void TestReadAPageOfTheSourcesInAUserDefinedCollection()
        {
            var subcollection = subcollections.ReadCollection(subcollections.Collections[0]);
            var state = subcollection.ReadSourceDescriptions();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
        }

        [Test]
        public void TestMoveSourcesToAUserDefinedCollection()
        {
            var description = (FamilySearchSourceDescriptionState)collection.AddSourceDescription(TestBacking.GetCreateSourceDescription()).Get();
            var subcollection = (CollectionState)collection.AddCollection(new Collection().SetTitle(Guid.NewGuid().ToString("n"))).Get();
            var state = description.MoveToCollection(subcollection);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.NoContent, state.Response.StatusCode);

            description.Delete();
            subcollection.Delete();
        }

        [Test]
        public void TestReadUserDefinedCollection()
        {
            var state = (CollectionState)collection.AddCollection(new Collection().SetTitle(Guid.NewGuid().ToString("n"))).Get();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);

            state.Delete();
        }

        [Test]
        public void TestUpdateUserDefinedCollection()
        {
            var subcollection = (CollectionState)collection.AddCollection(new Collection().SetTitle(Guid.NewGuid().ToString("n"))).Get();
            subcollection.Collection.Title = Guid.NewGuid().ToString("n");
            var state = subcollection.Update(subcollection.Collection);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.NoContent, state.Response.StatusCode);

            state.Delete();
        }

        [Test]
        public void TestDeleteUserDefinedCollection()
        {
            var subcollection = (CollectionState)collection.AddCollection(new Collection().SetTitle(Guid.NewGuid().ToString("n"))).Get();
            var state = subcollection.Delete();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.NoContent, state.Response.StatusCode);
        }
    }
}
