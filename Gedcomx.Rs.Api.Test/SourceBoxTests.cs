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
            collection = new CollectionState(new Uri("https://sandbox.familysearch.org/platform/collections/sources"));
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
        [Ignore("405 on all attempts to add source descriptions. Need clarification before going forward.")]
        public void TestDeleteSourceDescriptionsFromAUserDefinedCollection()
        {
            // Get the root collection
            var subcollection = subcollections.ReadCollection(subcollections.Entity.Collections.Single(x => x.Title == "asdf"));
            var descriptions = subcollection.ReadSourceDescriptions();
            var description = descriptions.AddSourceDescription(TestBacking.GetCreateSourceDescription());
            //var description = subcollection.AddArtifact(TestBacking.GetCreateSourceDescription(), new BasicDataSource("Sample Memory", MediaTypes.TEXT_PLAIN_TYPE, Resources.MemoryTXT));
            var state = description.Delete();

            Assert.DoesNotThrow(() => subcollection.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, subcollection.Response.StatusCode);
            Assert.IsNotNull(subcollection.Entity.Collections);
            Assert.Greater(subcollection.Entity.Collections.Count, 0);
        }
    }
}
