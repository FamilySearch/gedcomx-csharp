using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using FamilySearch.Api;

using Gx.Records;
using Gx.Rs.Api;

using NUnit.Framework;

namespace Gedcomx.Rs.Api.Test
{
    [TestFixture]
    public class SourceBoxTests
    {
        private CollectionState collection;
        private CollectionsState subcollections;
        private List<GedcomxApplicationState> cleanup;

        [OneTimeSetUp]
        public void Initialize()
        {
            collection = new FamilySearchCollectionState(new Uri("https://api-integ.familysearch.org/platform/collections/sources"));
            collection.AuthenticateViaOAuth2Password(Resources.TestUserName, Resources.TestPassword, Resources.TestClientId);
            subcollections = (CollectionsState)collection.ReadSubcollections().Get();
            cleanup = new List<GedcomxApplicationState>();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            foreach (var state in cleanup)
            {
                state.Delete();
            }
        }

        [Test]
        public void TestReadAllSourcesOfAllUserDefinedCollectionsOfASpecificUser()
        {
            // Get the root collection
            var subcollection = subcollections.ReadCollection(subcollections.Entity.Collections.Single(x => string.IsNullOrEmpty(x.Title)));
            subcollection = (CollectionState)subcollection.Get();
            var state = subcollection.ReadSourceDescriptions();

            Assert.DoesNotThrow(() => subcollection.IfSuccessful());
            Assert.That(subcollection.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(subcollection.Entity.Collections, Is.Not.Null);
            Assert.That(subcollection.Entity.Collections, Is.Not.Empty);
        }

        [Test]
        public void TestDeleteSourceDescriptionsFromAUserDefinedCollection()
        {
            var description = collection.AddSourceDescription(TestBacking.GetCreateSourceDescription());
            cleanup.Add(description);
            var state = description.Delete();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test]
        public void TestReadASpecificUsersSetOfUserDefinedCollections()
        {
            Assert.DoesNotThrow(() => subcollections.IfSuccessful());
            Assert.That(subcollections.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(subcollections.Collections, Is.Not.Null);
            Assert.That(subcollections.Collections, Is.Not.Empty);
        }

        [Test]
        public void TestCreateUserDefinedCollection()
        {
            var state = collection.AddCollection(new Collection().SetTitle(Guid.NewGuid().ToString("n")));
            cleanup.Add(state);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }

        [Test]
        public void TestReadAPageOfTheSourcesInAUserDefinedCollection()
        {
            var subcollection = subcollections.ReadCollection(subcollections.Collections[0]);
            var state = subcollection.ReadSourceDescriptions();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void TestMoveSourcesToAUserDefinedCollection()
        {
            var description = (FamilySearchSourceDescriptionState)collection.AddSourceDescription(TestBacking.GetCreateSourceDescription()).Get();
            cleanup.Add(description);
            var subcollection = (CollectionState)collection.AddCollection(new Collection().SetTitle(Guid.NewGuid().ToString("n"))).Get();
            cleanup.Add(subcollection);
            var state = description.MoveToCollection(subcollection);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test]
        public void TestReadUserDefinedCollection()
        {
            var state = (CollectionState)collection.AddCollection(new Collection().SetTitle(Guid.NewGuid().ToString("n"))).Get();
            cleanup.Add(state);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            state.Delete();
        }

        [Test]
        public void TestUpdateUserDefinedCollection()
        {
            var subcollection = (CollectionState)collection.AddCollection(new Collection().SetTitle(Guid.NewGuid().ToString("n"))).Get();
            cleanup.Add(subcollection);
            subcollection.Collection.Title = Guid.NewGuid().ToString("n");
            var state = subcollection.Update(subcollection.Collection);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test]
        public void TestDeleteUserDefinedCollection()
        {
            var subcollection = (CollectionState)collection.AddCollection(new Collection().SetTitle(Guid.NewGuid().ToString("n"))).Get();
            var state = subcollection.Delete();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }
    }
}
