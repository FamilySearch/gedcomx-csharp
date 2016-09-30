using FamilySearch.Api.Ft;
using Gx.Fs;
using Gx.Fs.Tree;
using Gx.Rs.Api;
using Gx.Types;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gedcomx.Rs.Api.Test
{
	[TestFixture]
	public class ParentsAndChildrenTest
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
			Assert.IsNotNull(tree.CurrentAccessToken);
			Assert.IsNotEmpty(tree.CurrentAccessToken);
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
		public void TestCreateChildAndParentsRelationship()
		{
			var father = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(father);
			var mother = tree.AddPerson(TestBacking.GetCreateFemalePerson());
			cleanup.Add(mother);
			var son = tree.AddPerson(TestBacking.GetCreateMalePerson());
			cleanup.Add(son);
			var state = tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(father, mother, son));
			cleanup.Add(state);

			Assert.DoesNotThrow(() => state.IfSuccessful());
			Assert.AreEqual(HttpStatusCode.Created, state.Response.StatusCode);
		}

		[Test]
		public void TestCreateChildAndParentsRelationshipSourceReference()
		{
			var father = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(father);
			var mother = tree.AddPerson(TestBacking.GetCreateFemalePerson());
			cleanup.Add(mother);
			var son = tree.AddPerson(TestBacking.GetCreateMalePerson());
			cleanup.Add(son);
			var relationship = tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(father, mother, son));
			cleanup.Add(relationship);
			var state = relationship.AddSourceReference(TestBacking.GetPersonSourceReference());

			Assert.DoesNotThrow(() => state.IfSuccessful());
			Assert.AreEqual(HttpStatusCode.Created, state.Response.StatusCode);
		}

		[Test]
		public void TestCreateChildAndParentsRelationshipConclusion()
		{
			var father = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(father);
			var mother = tree.AddPerson(TestBacking.GetCreateFemalePerson());
			cleanup.Add(mother);
			var son = tree.AddPerson(TestBacking.GetCreateMalePerson());
			cleanup.Add(son);
			var relationship = (ChildAndParentsRelationshipState)tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(father, mother, son)).Get();
			cleanup.Add(relationship);
			var state = relationship.AddFatherFact(TestBacking.GetBiologicalParentFact());

			Assert.DoesNotThrow(() => state.IfSuccessful());

			// TODO: likely this should now be created
			//Assert.AreEqual(HttpStatusCode.NoContent, state.Response.StatusCode);
			Assert.AreEqual(HttpStatusCode.Created, state.Response.StatusCode);
		}

		[Test]
		public void TestCreateChildAndParentsRelationshipNote()
		{
			var father = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(father);
			var mother = tree.AddPerson(TestBacking.GetCreateFemalePerson());
			cleanup.Add(mother);
			var son = tree.AddPerson(TestBacking.GetCreateMalePerson());
			cleanup.Add(son);
			var relationship = (ChildAndParentsRelationshipState)tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(father, mother, son)).Get();
			cleanup.Add(relationship);
			var state = relationship.AddNote(TestBacking.GetCreateNote());

			Assert.DoesNotThrow(() => state.IfSuccessful());
			Assert.AreEqual(HttpStatusCode.Created, state.Response.StatusCode);
		}

		[Test]
		public void TestReadChildAndParentsRelationship()
		{
			var father = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(father);
			var mother = tree.AddPerson(TestBacking.GetCreateFemalePerson());
			cleanup.Add(mother);
			var son = tree.AddPerson(TestBacking.GetCreateMalePerson());
			cleanup.Add(son);
			var relationship = tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(father, mother, son));
			cleanup.Add(relationship);
			var state = relationship.Get();

			Assert.DoesNotThrow(() => state.IfSuccessful());
			Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
		}

		[Test]
		public void TestReadChildAndParentsRelationshipSourceReferences()
		{
			var father = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(father);
			var mother = tree.AddPerson(TestBacking.GetCreateFemalePerson());
			cleanup.Add(mother);
			var son = tree.AddPerson(TestBacking.GetCreateMalePerson());
			cleanup.Add(son);
			var temp = TestBacking.GetCreateChildAndParentsRelationship(father, mother, son);
			temp.AddSource(TestBacking.GetPersonSourceReference());
			var relationship = (ChildAndParentsRelationshipState)tree.AddChildAndParentsRelationship(temp).Get();
			cleanup.Add(relationship);
			var state = relationship.LoadSourceReferences();

			var relationship2 = (ChildAndParentsRelationshipState)relationship.Get();
			
			Assert.DoesNotThrow(() => state.IfSuccessful());
			Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
		}

		[Test]
		public void TestReadChildAndParentsRelationshipNotes()
		{
			var father = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			var mother = tree.AddPerson(TestBacking.GetCreateFemalePerson());
			var son = tree.AddPerson(TestBacking.GetCreateMalePerson());
			var relationship = (ChildAndParentsRelationshipState)tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(father, mother, son)).Get();
			var state = relationship.LoadNotes();

			Assert.DoesNotThrow(() => state.IfSuccessful());
			Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
		}

		[Test]
		public void TestUpdateChildAndParentsRelationship()
		{
			var father = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(father);
			var mother = tree.AddPerson(TestBacking.GetCreateFemalePerson());
			cleanup.Add(mother);
			var son = tree.AddPerson(TestBacking.GetCreateMalePerson());
			cleanup.Add(son);
			var relationship = (ChildAndParentsRelationshipState)tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(father, mother, son)).Get();
			cleanup.Add(relationship);
			var newFather = tree.AddPerson(TestBacking.GetCreateMalePerson());
			cleanup.Add(newFather);
			var state = relationship.UpdateFather(newFather);

			Assert.DoesNotThrow(() => state.IfSuccessful());
			Assert.AreEqual(HttpStatusCode.NoContent, state.Response.StatusCode);
		}

		[Test]
		public void TestUpdateChildAndParentsRelationshipConclusion()
		{
			var father = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(father);
			var mother = tree.AddPerson(TestBacking.GetCreateFemalePerson());
			cleanup.Add(mother);
			var son = tree.AddPerson(TestBacking.GetCreateMalePerson());
			cleanup.Add(son);
			var relationship = (ChildAndParentsRelationshipState)tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(father, mother, son)).Get();
			cleanup.Add(relationship);
			var update = relationship.AddFatherFact(TestBacking.GetBiologicalParentFact());
			relationship = (ChildAndParentsRelationshipState)relationship.Get();
			relationship.LoadConclusions();
			relationship.FatherFact.KnownType = FactType.AdoptiveParent;
			var state = relationship.UpdateFatherFact(relationship.FatherFact);

			Assert.DoesNotThrow(() => state.IfSuccessful());
			Assert.AreEqual(HttpStatusCode.NoContent, state.Response.StatusCode);
		}

		[Test]
		public void TestDeleteChildAndParentsRelationship()
		{
			var father = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(father);
			var mother = tree.AddPerson(TestBacking.GetCreateFemalePerson());
			cleanup.Add(mother);
			var son = tree.AddPerson(TestBacking.GetCreateMalePerson());
			cleanup.Add(son);
			var relationship = (ChildAndParentsRelationshipState)tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(father, mother, son)).Get();
			var state = relationship.Delete();

			Assert.DoesNotThrow(() => state.IfSuccessful());
			Assert.AreEqual(HttpStatusCode.NoContent, state.Response.StatusCode);
		}

		[Test]
		public void TestDeleteChildAndParentsRelationshipSourceReference()
		{
			var father = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(father);
			var mother = tree.AddPerson(TestBacking.GetCreateFemalePerson());
			cleanup.Add(mother);
			var son = tree.AddPerson(TestBacking.GetCreateMalePerson());
			cleanup.Add(son);
			var temp = TestBacking.GetCreateChildAndParentsRelationship(father, mother, son);
			temp.AddSource(TestBacking.GetPersonSourceReference());
			var relationship = (ChildAndParentsRelationshipState)tree.AddChildAndParentsRelationship(temp).Get();
			cleanup.Add(relationship);
			
			var state = relationship.DeleteSourceReference(relationship.SourceReference);

			Assert.DoesNotThrow(() => state.IfSuccessful());
			Assert.AreEqual(HttpStatusCode.NoContent, state.Response.StatusCode);
		}

		[Test]
		public void TestRestoreChildAndParentsRelationship()
		{
			var father = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(father);
			var mother = tree.AddPerson(TestBacking.GetCreateFemalePerson());
			cleanup.Add(mother);
			var son = tree.AddPerson(TestBacking.GetCreateMalePerson());
			cleanup.Add(son);
			var relationship = (ChildAndParentsRelationshipState)tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(father, mother, son)).Get();
			cleanup.Add(relationship);
			relationship = (ChildAndParentsRelationshipState)relationship.Delete().IfSuccessful().Get();
			var state = relationship.Restore();

			Assert.DoesNotThrow(() => state.IfSuccessful());
			Assert.AreEqual(HttpStatusCode.NoContent, state.Response.StatusCode);
		}

		[Test]
		public void TestDeleteChildAndParentsRelationshipParent()
		{
			var father = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(father);
			var mother = tree.AddPerson(TestBacking.GetCreateFemalePerson());
			cleanup.Add(mother);
			var son = tree.AddPerson(TestBacking.GetCreateMalePerson());
			cleanup.Add(son);
			var relationship = (ChildAndParentsRelationshipState)tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(father, mother, son)).Get();
			cleanup.Add(relationship);
			var state = relationship.DeleteFather();

			Assert.DoesNotThrow(() => state.IfSuccessful());
			Assert.AreEqual(HttpStatusCode.NoContent, state.Response.StatusCode);
		}

		[Test]
		public void TestDeleteChildAndParentsRelationshipConclusion()
		{
			var father = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(father);
			var mother = tree.AddPerson(TestBacking.GetCreateFemalePerson());
			cleanup.Add(mother);
			var son = tree.AddPerson(TestBacking.GetCreateMalePerson());
			cleanup.Add(son);
			var relationship = (ChildAndParentsRelationshipState)tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(father, mother, son)).Get();
			cleanup.Add(relationship);
			relationship.AddFatherFact(TestBacking.GetBiologicalParentFact());
			relationship = (ChildAndParentsRelationshipState)relationship.Get();
			var state = relationship.DeleteFact(relationship.FatherFact);

			Assert.DoesNotThrow(() => state.IfSuccessful());
			Assert.AreEqual(HttpStatusCode.NoContent, state.Response.StatusCode);
		}
	}
}
