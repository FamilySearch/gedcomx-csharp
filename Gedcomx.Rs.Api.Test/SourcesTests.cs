using System.Collections.Generic;
using System.Linq;
using System.Net;

using FamilySearch.Api.Ft;

using Gedcomx.Support;

using Gx.Rs.Api;
using Gx.Rs.Api.Util;

using NUnit.Framework;

namespace Gedcomx.Rs.Api.Test
{
    [TestFixture]
    public class SourcesTests
    {
        private FamilySearchFamilyTree tree;
        private List<GedcomxApplicationState> cleanup;

        [OneTimeSetUp]
        public void Initialize()
        {
            tree = new FamilySearchFamilyTree(true);
            tree.AuthenticateViaOAuth2Password(Resources.TestUserName, Resources.TestPassword, Resources.TestClientId);
            Assert.DoesNotThrow(() => tree.IfSuccessful());
            Assert.That(tree.CurrentAccessToken, Is.Not.Null);
            Assert.That(tree.CurrentAccessToken, Is.Not.Empty);
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
        public void TestCreateSourceDescription()
        {
            var state = tree.AddSourceDescription(TestBacking.GetCreateSourceDescription());
            cleanup.Add(state);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }

        [Test]
        public void TestCreateUserUploadedSource()
        {
            var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(person);
            var dataSource = new BasicDataSource("Sample Memory", MediaTypes.TEXT_PLAIN_TYPE, Resources.MemoryTXT);
            var sds = person.AddArtifact(dataSource);
            cleanup.Add(sds);
            var artifact = person.ReadArtifacts().SourceDescriptions.First();
            var memoryUri = artifact.GetLink("memory").Href;
            var state = tree.AddSourceDescription(TestBacking.GetCreateUserSourceDescription(memoryUri));
            cleanup.Add(state);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }

        [Test]
        public void TestReadSourceDescription()
        {
            var state = (SourceDescriptionState)tree.AddSourceDescription(TestBacking.GetCreateSourceDescription()).Get();
            cleanup.Add(state);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(state.SourceDescription, Is.Not.Null);
        }

        [Test]
        public void TestUpdateSourceDescription()
        {
            var description = (SourceDescriptionState)tree.AddSourceDescription(TestBacking.GetCreateSourceDescription()).Get();
            cleanup.Add(description);
            var state = description.Update(description.SourceDescription);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test]
        public void TestDeleteSourceDescription()
        {
            var state = tree.AddSourceDescription(TestBacking.GetCreateSourceDescription()).Delete();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test]
        public void TestDeleteCoupleRelationshipSourceReference()
        {
            var husband = (PersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(husband);
            var wife = tree.AddPerson(TestBacking.GetCreateFemalePerson());
            cleanup.Add(wife);
            var relationship = (RelationshipState)husband.AddSpouse(wife).Get();
            cleanup.Add(relationship);
            relationship.AddSourceReference(TestBacking.GetPersonSourceReference());
            relationship = (RelationshipState)relationship.Get();
            var state = relationship.DeleteSourceReference(relationship.SourceReference);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test]
        public void TestReadSourceReferences()
        {
            var source = (SourceDescriptionState)tree.AddSourceDescription(TestBacking.GetCreateSourceDescription()).Get();
            cleanup.Add(source);
            var person = tree.AddPerson(TestBacking.GetCreateMalePerson());
            cleanup.Add(person);
            var sourceRef = TestBacking.GetPersonSourceReference();
            sourceRef.DescriptionRef = source.GetSelfUri();
            person.AddSourceReference(sourceRef);
            var state = source.QueryAttachedReferences();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(state.Entity, Is.Not.Null);
            Assert.That(state.Entity.Persons, Is.Not.Null);
            Assert.That(state.Entity.Persons, Is.Not.Empty);
        }
    }
}
