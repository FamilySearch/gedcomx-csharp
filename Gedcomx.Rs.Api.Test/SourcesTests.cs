using FamilySearch.Api.Ft;
using Gedcomx.Support;
using Gx.Rs.Api;
using NUnit.Framework;
using System;
using System.Linq;
using System.Net;

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
            var memoryUri = artifact.GetLink("memory").Href;
            var state = tree.AddSourceDescription(TestBacking.GetCreateUserSourceDescription(memoryUri, CONTRIBUTOR_RESOURCE_ID));

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.Created, state.Response.StatusCode);
        }

        [Test]
        public void TestReadSourceDescription()
        {
            var state = (SourceDescriptionState)tree.AddSourceDescription(TestBacking.GetCreateSourceDescription(CONTRIBUTOR_RESOURCE_ID)).Get();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
            Assert.IsNotNull(state.SourceDescription);
        }

        [Test]
        public void TestUpdateSourceDescription()
        {
            var description = (SourceDescriptionState)tree.AddSourceDescription(TestBacking.GetCreateSourceDescription(CONTRIBUTOR_RESOURCE_ID)).Get();
            var state = description.Update(description.SourceDescription);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.NoContent, state.Response.StatusCode);
        }

        [Test]
        public void TestDeleteSourceDescription()
        {
            var state = tree.AddSourceDescription(TestBacking.GetCreateSourceDescription(CONTRIBUTOR_RESOURCE_ID)).Delete();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.NoContent, state.Response.StatusCode);
        }

        [Test]
        public void TestDeleteCoupleRelationshipSourceReference()
        {
            var husband = (PersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            var wife = tree.AddPerson(TestBacking.GetCreateFemalePerson());
            var relationship = (RelationshipState)husband.AddSpouse(wife).Get();
            relationship.AddSourceReference(TestBacking.GetPersonSourceReference());
            relationship.LoadSourceReferences();
            var state = relationship.DeleteSourceReference(relationship.SourceReference);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.NoContent, state.Response.StatusCode);
        }

        [Test]
        public void TestReadSourceReferences()
        {
            var source = (SourceDescriptionState)tree.AddSourceDescription(TestBacking.GetCreateSourceDescription(CONTRIBUTOR_RESOURCE_ID)).Get();
            var person = tree.AddPerson(TestBacking.GetCreateMalePerson());
            var sourceRef = TestBacking.GetPersonSourceReference();
            sourceRef.DescriptionRef = source.GetSelfUri();
            person.AddSourceReference(sourceRef);
            var state = source.QueryAttachedReferences();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
            Assert.IsNotNull(state.Entity);
            Assert.IsNotNull(state.Entity.Persons);
            Assert.Greater(state.Entity.Persons.Count, 0);
        }
    }
}
