using FamilySearch.Api.Ft;
using Gedcomx.Support;
using Gx.Rs.Api;
using Gx.Rs.Api.Util;
using NUnit.Framework;
using System;
using System.Linq;
using System.Net;

namespace Gedcomx.Rs.Api.Test
{
    [TestFixture]
    public class SourcesTests
    {
        private FamilySearchFamilyTree tree;

        [TestFixtureSetUp]
        public void Initialize()
        {
            tree = new FamilySearchFamilyTree(true);
            tree.AuthenticateViaOAuth2Password(Resources.TestUserName, Resources.TestPassword, Resources.TestClientId);
            Assert.DoesNotThrow(() => tree.IfSuccessful());
            Assert.IsNotNullOrEmpty(tree.CurrentAccessToken);
        }

        [Test]
        public void TestCreateSourceDescription()
        {
            var state = tree.AddSourceDescription(TestBacking.GetCreateSourceDescription());

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.Created, state.Response.StatusCode);
        }

        [Test]
        public void TestCreateUserUploadedSource()
        {
            var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            var dataSource = new BasicDataSource("Sample Memory", MediaTypes.TEXT_PLAIN_TYPE, Resources.MemoryTXT);
            person.AddArtifact(dataSource);
            var artifact = person.ReadArtifacts().SourceDescriptions.First();
            var memoryUri = artifact.GetLink("memory").Href;
            var state = tree.AddSourceDescription(TestBacking.GetCreateUserSourceDescription(memoryUri));

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.Created, state.Response.StatusCode);
        }

        [Test]
        public void TestReadSourceDescription()
        {
            var state = (SourceDescriptionState)tree.AddSourceDescription(TestBacking.GetCreateSourceDescription()).Get();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
            Assert.IsNotNull(state.SourceDescription);
        }

        [Test]
        public void TestUpdateSourceDescription()
        {
            var description = (SourceDescriptionState)tree.AddSourceDescription(TestBacking.GetCreateSourceDescription()).Get();
            var state = description.Update(description.SourceDescription);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.NoContent, state.Response.StatusCode);
        }

        [Test]
        public void TestDeleteSourceDescription()
        {
            var state = tree.AddSourceDescription(TestBacking.GetCreateSourceDescription()).Delete();

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
            var source = (SourceDescriptionState)tree.AddSourceDescription(TestBacking.GetCreateSourceDescription()).Get();
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
