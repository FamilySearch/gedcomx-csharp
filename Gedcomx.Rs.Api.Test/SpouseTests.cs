﻿using System.Collections.Generic;
using System.Net;

using FamilySearch.Api.Ft;

using Gx.Common;
using Gx.Rs.Api;
using Gx.Rs.Api.Options;

using NUnit.Framework;

namespace Gedcomx.Rs.Api.Test
{
    [TestFixture]
    public class SpouseTests
    {
        private FamilySearchFamilyTree tree;
        private List<GedcomxApplicationState> cleanup;

        [OneTimeSetUp]
        public void Initialize()
        {
            tree = new FamilySearchFamilyTree(true);
            tree.AuthenticateViaOAuth2Password(Resources.TestUserName, Resources.TestPassword, Resources.TestClientId);
            cleanup = new List<GedcomxApplicationState>();
            Assert.DoesNotThrow(() => tree.IfSuccessful());
            Assert.That(tree.CurrentAccessToken, Is.Not.Null);
            Assert.That(tree.CurrentAccessToken, Is.Not.Empty);
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
        public void TestCreateCoupleRelationship()
        {
            var husband = (PersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(husband);
            var wife = tree.AddPerson(TestBacking.GetCreateFemalePerson());
            cleanup.Add(wife);
            var state = husband.AddSpouse(wife);
            cleanup.Add(state);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }

        [Test]
        public void TestCreateCoupleRelationshipSourceReference()
        {
            var husband = (PersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(husband);
            var wife = tree.AddPerson(TestBacking.GetCreateFemalePerson());
            cleanup.Add(wife);
            var relationship = husband.AddSpouse(wife);
            cleanup.Add(relationship);
            var state = relationship.AddSourceReference(TestBacking.GetPersonSourceReference());

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }

        [Test]
        public void TestCreateCoupleRelationshipConclusion()
        {
            var husband = (PersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(husband);
            var wife = tree.AddPerson(TestBacking.GetCreateFemalePerson());
            cleanup.Add(wife);
            var relationship = husband.AddSpouse(wife);
            cleanup.Add(relationship);
            var state = relationship.AddFact(TestBacking.GetMarriageFact());

            Assert.DoesNotThrow(() => state.IfSuccessful());

            // TODO: likely this should now be created
            //Assert.AreEqual(HttpStatusCode.NoContent, state.Response.StatusCode);
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }

        [Test]
        public void TestCreateCoupleRelationshipNote()
        {
            var husband = (PersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(husband);
            var wife = tree.AddPerson(TestBacking.GetCreateFemalePerson());
            cleanup.Add(wife);
            var relationship = husband.AddSpouse(wife);
            cleanup.Add(relationship);
            var state = relationship.AddNote(TestBacking.GetCreateNote());

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }

        [Test]
        public void TestReadCoupleRelationship()
        {
            var husband = (PersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(husband);
            var wife = tree.AddPerson(TestBacking.GetCreateFemalePerson());
            cleanup.Add(wife);
            var relationship = husband.AddSpouse(wife);
            cleanup.Add(relationship);
            var state = (RelationshipState)relationship.Get();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(state.Relationship, Is.Not.Null);
        }

        [Test]
        public void TestReadCoupleRelationshipConditional()
        {
            var husband = (PersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(husband);
            var wife = tree.AddPerson(TestBacking.GetCreateFemalePerson());
            cleanup.Add(wife);
            var relationship = husband.AddSpouse(wife);
            cleanup.Add(relationship);
            var @get = (RelationshipState)relationship.Get();
            var cache = new CacheDirectives(@get);
            var state = relationship.Get(cache);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.NotModified));
        }

        [Test]
        public void TestReadCoupleRelationshipSourceReferences()
        {
            var husband = (PersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(husband);
            var wife = tree.AddPerson(TestBacking.GetCreateFemalePerson());
            cleanup.Add(wife);
            var relationship = husband.AddSpouse(wife);
            cleanup.Add(relationship);
            relationship.AddSourceReference(TestBacking.GetPersonSourceReference());
            var state = ((RelationshipState)relationship.Get()).LoadSourceReferences();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(state.SourceReference, Is.Not.Null);
        }

        [Test]
        public void TestReadCoupleRelationshipNotes()
        {
            var husband = (PersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(husband);
            var wife = tree.AddPerson(TestBacking.GetCreateFemalePerson());
            cleanup.Add(wife);
            var relationship = (RelationshipState)husband.AddSpouse(wife).Get();
            cleanup.Add(relationship);
            relationship.AddNote(TestBacking.GetCreateNote());
            var state = relationship.LoadNotes();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(state.Relationship.Notes, Is.Not.Null);
            Assert.That(state.Relationship.Notes, Has.Count.EqualTo(1));
        }

        [Test, Category("AccountNeeded")]
        public void TestReadNonExistentCoupleRelationship()
        {
            var person = tree.AddPerson(TestBacking.GetCreateMalePerson());
            cleanup.Add(person);
            var relationship = TestBacking.GetCreateInvalidRelationship();
            var state = person.ReadRelationship(relationship);

            Assert.Throws<GedcomxApplicationException>(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public void TestHeadCoupleRelationship()
        {
            var husband = (PersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(husband);
            var wife = tree.AddPerson(TestBacking.GetCreateFemalePerson());
            cleanup.Add(wife);
            var relationship = husband.AddSpouse(wife);
            cleanup.Add(relationship);

            var state = relationship.Head();
            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void TestUpdatePersonsOfACoupleRelationship()
        {
            var husband = (PersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(husband);
            var wife = tree.AddPerson(TestBacking.GetCreateFemalePerson());
            cleanup.Add(wife);
            var relationship = (RelationshipState)husband.AddSpouse(wife).Get();
            cleanup.Add(relationship);
            var wife2 = tree.AddPerson(TestBacking.GetCreateFemalePerson());
            cleanup.Add(wife2);
            relationship.Relationship.Person2 = new ResourceReference(wife2.GetSelfUri());
            var state = relationship.Post(relationship.Entity);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test]
        public void TestUpdateCoupleRelationshipConclusion()
        {
            var husband = (PersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(husband);
            var wife = tree.AddPerson(TestBacking.GetCreateFemalePerson());
            cleanup.Add(wife);
            var relationship = husband.AddSpouse(wife);
            cleanup.Add(relationship);
            var update = (RelationshipState)relationship.AddFact(TestBacking.GetMarriageFact()).Get();
            update = (RelationshipState)relationship.Get();
            update.Fact.Date.Original = "4 Apr 1930";
            update.Fact.Attribution = new Attribution()
            {
                ChangeMessage = "Change message2",
            };
            var state = relationship.UpdateFact(update.Fact);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test, Category("AccountNeeded")]
        public void TestUpdateIllegalCoupleRelationship()
        {
            var husband = (PersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(husband);
            var wife = tree.AddPerson(TestBacking.GetCreateFemalePerson());
            cleanup.Add(wife);
            var relationship = (RelationshipState)husband.AddSpouse(wife).Get();
            cleanup.Add(relationship);
            var invalid = tree.AddPerson(TestBacking.GetCreateMalePerson());
            cleanup.Add(invalid);
            relationship.Relationship.Person2 = new ResourceReference(invalid.GetSelfUri());
            var state = relationship.Post(relationship.Entity);

            Assert.Throws<GedcomxApplicationException>(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public void TestDeleteCoupleRelationship()
        {
            var husband = (PersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(husband);
            var wife = tree.AddPerson(TestBacking.GetCreateFemalePerson());
            cleanup.Add(wife);
            var relationship = husband.AddSpouse(wife);
            cleanup.Add(relationship);
            var state = relationship.Delete();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        // TODO: fact.Fact is null because this call returns null GetLink(Rel.CONCLUSIONS)
        [Test]
        public void TestDeleteCoupleRelationshipConclusion()
        {
            var husband = (PersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(husband);
            var wife = tree.AddPerson(TestBacking.GetCreateFemalePerson());
            cleanup.Add(wife);
            var relationship = husband.AddSpouse(wife);
            cleanup.Add(relationship);
            var fact = (RelationshipState)relationship.AddFact(TestBacking.GetMarriageFact()).Get();
            var state2 = (RelationshipState)relationship.Get();

            var state = state2.DeleteFact(state2.Fact);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test]
        public void TestRestoreCoupleRelationship()
        {
            var husband = (PersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(husband);
            var wife = tree.AddPerson(TestBacking.GetCreateFemalePerson());
            cleanup.Add(wife);
            var relationship = (FamilyTreeRelationshipState)husband.AddSpouse(wife).Get();
            cleanup.Add(relationship);
            relationship = (FamilyTreeRelationshipState)relationship.Delete().IfSuccessful().Get();
            var state = relationship.Restore();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }
    }
}
