using FamilySearch.Api.Ft;
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
    public class SourcesTests
    {
        private readonly String CONTRIBUTOR_RESOURCE_ID = "MM6M-8QJ";
        private FamilySearchFamilyTree tree;

        [TestFixtureSetUp]
        public void Initialize()
        {
            tree = new FamilySearchFamilyTree(true);
            tree.AuthenticateViaOAuth2Password("sdktester", "1234sdkpass", "WCQY-7J1Q-GKVV-7DNM-SQ5M-9Q5H-JX3H-CMJK");
            Assert.DoesNotThrow(() => tree.IfSuccessful());
            Assert.IsNotNullOrEmpty(tree.CurrentAccessToken);
        }

        [Test]
        public void TestCreateSourceDescription()
        {
            var state = tree.AddSourceDescription(TestBacking.GetCreateSourceDescription(CONTRIBUTOR_RESOURCE_ID));

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.Created, state.Response.StatusCode);
        }

        [Test]
        public void TestCreateUserUploadedSource()
        {
            var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            var dataSource = TestBacking.GetDataSource("Sample Memory", MediaTypes.TEXT_PLAIN_TYPE, Resources.PersonMemory);
            person.AddArtifact(dataSource);
            var artifact = person.ReadArtifacts().SourceDescriptions.First();
            //"https://familysearch.org/platform/memories/memories/12345"
            var memoryUri = artifact.GetLink("memory").Href;
            var state = tree.AddSourceDescription(TestBacking.GetCreateUserSourceDescription(memoryUri, CONTRIBUTOR_RESOURCE_ID));

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.Created, state.Response.StatusCode);
        }
    }
}
