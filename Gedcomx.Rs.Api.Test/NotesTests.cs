using FamilySearch.Api.Ft;
using Gx.Common;
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
    public class NotesTests
    {
        private FamilySearchFamilyTree tree;

        [TestFixtureSetUp]
        public void Initialize()
        {
            tree = new FamilySearchFamilyTree(true);
            tree.AuthenticateViaOAuth2Password(Resources.TestUserName, Resources.TestPassword, Resources.TestClientId);
        }
        
        [Test]
        public void TestReadNote()
        {
            var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            person.AddNote(new Note().SetText("This is a note.").SetSubject("This is a note."));
            var notes = person.LoadNotes();
            var state = notes.ReadNote(notes.Entity.Persons[0].Notes[0]);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
            Assert.IsNotNull(state.Person);
            Assert.IsNotNull(state.Person.Notes);
            Assert.Greater(state.Person.Notes.Count, 0);

            person.Delete();
        }

        [Test]
        public void TestDeleteNote()
        {
            var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            person.AddNote(new Note().SetText("This is a note.").SetSubject("This is a note."));
            var notes = person.LoadNotes();
            var state = notes.DeleteNote(notes.Entity.Persons[0].Notes[0]);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.NoContent, state.Response.StatusCode);

            person.Delete();
        }

        [Test]
        public void TestUpdateCoupleRelationshipNote()
        {
            var husband = (PersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            var wife = tree.AddPerson(TestBacking.GetCreateFemalePerson());
            var relationship = (RelationshipState)husband.AddSpouse(wife).Get();
            relationship.AddNote(TestBacking.GetCreateNote());
            var notes = relationship.LoadNotes();
            var state = relationship.UpdateNote(notes.Note);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.NoContent, state.Response.StatusCode);

            husband.Delete();
            wife.Delete();
        }

        [Test]
        public void TestUpdateNote()
        {
            var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            person.AddNote(TestBacking.GetCreateNote());
            var notes = person.LoadNotes();
            var state = person.UpdateNote(notes.Entity.Persons[0].Notes[0]);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.NoContent, state.Response.StatusCode);

            person.Delete();
        }

        [Test]
        public void TestUpdateChildAndParentsRelationshipNotes()
        {
            var father = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            var mother = tree.AddPerson(TestBacking.GetCreateFemalePerson());
            var son = tree.AddPerson(TestBacking.GetCreateMalePerson());
            var relationship = (ChildAndParentsRelationshipState)tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(father, mother, son)).Get();
            relationship.AddNote(TestBacking.GetCreateNote());
            var notes = relationship.LoadNotes();
            var state = relationship.UpdateNote(notes.Note);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.NoContent, state.Response.StatusCode);

            father.Delete();
            mother.Delete();
            son.Delete();
        }

        [Test]
        public void TestDeleteChildAndParentsRelationshipNotes()
        {
            var father = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            var mother = tree.AddPerson(TestBacking.GetCreateFemalePerson());
            var son = tree.AddPerson(TestBacking.GetCreateMalePerson());
            var relationship = (ChildAndParentsRelationshipState)tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(father, mother, son)).Get();
            relationship.AddNote(TestBacking.GetCreateNote());
            var notes = relationship.LoadNotes();
            var state = relationship.DeleteNote(notes.Note);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.NoContent, state.Response.StatusCode);

            father.Delete();
            mother.Delete();
            son.Delete();
        }

        [Test]
        public void TestDeleteCoupleRelationshipNotes()
        {
            var husband = (PersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            var wife = tree.AddPerson(TestBacking.GetCreateFemalePerson());
            var relationship = (RelationshipState)husband.AddSpouse(wife).Get();
            relationship.AddNote(TestBacking.GetCreateNote());
            var notes = relationship.LoadNotes();
            var state = relationship.DeleteNote(notes.Note);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.NoContent, state.Response.StatusCode);

            husband.Delete();
            wife.Delete();
        }
    }
}
