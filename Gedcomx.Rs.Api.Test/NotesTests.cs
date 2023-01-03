using System.Collections.Generic;
using System.Net;

using FamilySearch.Api.Ft;

using Gx.Common;
using Gx.Rs.Api;

using NUnit.Framework;

namespace Gedcomx.Rs.Api.Test
{
    [TestFixture]
    public class NotesTests
    {
        private FamilySearchFamilyTree tree;
        private List<GedcomxApplicationState> cleanup;

        [OneTimeSetUp]
        public void Initialize()
        {
            tree = new FamilySearchFamilyTree(true);
            tree.AuthenticateViaOAuth2Password(Resources.TestUserName, Resources.TestPassword, Resources.TestClientId);
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
        public void TestReadNote()
        {
            var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(person);
            person.AddNote(new Note().SetText("This is a note.").SetSubject("This is a note."));
            var notes = person.LoadNotes();
            var state = notes.ReadNote(notes.Entity.Persons[0].Notes[0]);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(state.Person, Is.Not.Null);
            Assert.That(state.Person.Notes, Is.Not.Null);
            Assert.That(state.Person.Notes, Is.Not.Empty);
        }

        [Test]
        public void TestDeleteNote()
        {
            var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(person);
            person.AddNote(new Note().SetText("This is a note.").SetSubject("This is a note."));
            var notes = person.LoadNotes();
            var state = notes.DeleteNote(notes.Entity.Persons[0].Notes[0]);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test]
        public void TestUpdateCoupleRelationshipNote()
        {
            var husband = (PersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(husband);
            var wife = tree.AddPerson(TestBacking.GetCreateFemalePerson());
            cleanup.Add(wife);
            var relationship = (RelationshipState)husband.AddSpouse(wife).Get();
            cleanup.Add(relationship);
            relationship.AddNote(TestBacking.GetCreateNote());
            var notes = relationship.LoadNotes();
            var state = relationship.UpdateNote(TestBacking.GetCreateNote());

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }

        [Test]
        public void TestUpdateNote()
        {
            var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(person);
            person.AddNote(TestBacking.GetCreateNote());
            var notes = person.LoadNotes();
            var state = person.UpdateNote(TestBacking.GetCreateNote());

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }

        [Test, Category("AccountNeeded")]
        public void TestUpdateChildAndParentsRelationshipNotes()
        {
            var father = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(father);
            var mother = tree.AddPerson(TestBacking.GetCreateFemalePerson());
            cleanup.Add(mother);
            var son = tree.AddPerson(TestBacking.GetCreateMalePerson());
            cleanup.Add(son);
            var relationship = (ChildAndParentsRelationshipState)tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(father, mother, son)).Get();
            cleanup.Add(relationship);
            relationship.AddNote(TestBacking.GetCreateNote());
            var notes = relationship.LoadNotes();
            var state = relationship.UpdateNote(TestBacking.GetCreateNote());

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }

        [Test, Category("AccountNeeded")]
        public void TestDeleteChildAndParentsRelationshipNotes()
        {
            var father = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(father);
            var mother = tree.AddPerson(TestBacking.GetCreateFemalePerson());
            cleanup.Add(mother);
            var son = tree.AddPerson(TestBacking.GetCreateMalePerson());
            cleanup.Add(son);
            var relationship = (ChildAndParentsRelationshipState)tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(father, mother, son)).Get();
            cleanup.Add(relationship);
            relationship.AddNote(TestBacking.GetCreateNote());
            var notes = relationship.LoadNotes();
            var state = relationship.DeleteNote(notes.Note);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test]
        public void TestDeleteCoupleRelationshipNotes()
        {
            var husband = (PersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(husband);
            var wife = tree.AddPerson(TestBacking.GetCreateFemalePerson());
            cleanup.Add(wife);
            var relationship = (RelationshipState)husband.AddSpouse(wife).Get();
            cleanup.Add(relationship);
            relationship.AddNote(TestBacking.GetCreateNote());
            var notes = relationship.LoadNotes();
            var state = relationship.DeleteNote(notes.Note);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }
    }
}
