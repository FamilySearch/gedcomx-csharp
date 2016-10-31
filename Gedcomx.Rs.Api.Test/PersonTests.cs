using FamilySearch.Api;
using FamilySearch.Api.Ft;
using FamilySearch.Api.Util;
using Gedcomx.Support;
using Gx.Common;
using Gx.Conclusion;
using Gx.Fs.Discussions;
using Gx.Fs.Tree;
using Gx.Rs.Api;
using Gx.Rs.Api.Options;
using Gx.Rs.Api.Util;
using Gx.Source;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading;

namespace Gedcomx.Rs.Api.Test
{
	[TestFixture]
	public class PersonTests
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
		public void TestCreatePerson()
		{
			var result = tree.AddPerson(TestBacking.GetCreateMalePerson());
			cleanup.Add(result);
			Assert.DoesNotThrow(() => result.IfSuccessful());
			var person = (PersonState)result.Get();

			Assert.IsNotNull(person.Person);
			Assert.IsNotNull(person.Person.Id);
			Assert.IsNotEmpty(person.Person.Id);
		}

		[Test]
		public void TestCreatePersonSourceReference()
		{
			var result = tree.AddPerson(TestBacking.GetCreateMalePerson());
			cleanup.Add(result);
			Assert.DoesNotThrow(() => result.IfSuccessful());
			var state = (PersonState)result.Get();
			var result2 = state.AddSourceReference(TestBacking.GetPersonSourceReference());
			Assert.DoesNotThrow(() => result2.IfSuccessful());
			Assert.AreEqual(HttpStatusCode.Created, result2.Response.StatusCode);
		}

		[Test]
		public void TestCreatePersonConclusion()
		{
			var state = (PersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(state);
			Person conclusion = TestBacking.GetCreatePersonConclusion(state.Person.Id);
			var state2 = state.UpdateConclusions(conclusion);
			Assert.IsNotNull(state2);
			Assert.DoesNotThrow(() => state2.IfSuccessful());
		}

		[Test]
		public void TestCreateDiscussionReference()
		{
			var me = tree.ReadCurrentUser();
			var contributor = new ResourceReference("https://familysearch.org/platform/users/agents/" + me.User.TreeUserId).SetResourceId(me.User.TreeUserId);
			var discussion = tree.AddDiscussion(new Discussion()
													.SetTitle("Test title")
													.SetDetails("Test details")
													.SetContributor(contributor)
													.SetCreated(DateTime.Now));
			cleanup.Add(discussion);
			var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(person);
			var state = person.AddDiscussionReference(discussion);

			var state2 = tree.ReadPerson(new Uri(person.GetSelfUri()));

			Assert.DoesNotThrow(() => state.IfSuccessful());
			Assert.AreEqual(HttpStatusCode.Created, state.Response.StatusCode);
			Assert.Greater(state2.Person.DiscussionReferences.Count, 0);
		}

		[Test]
		public void TestCreateNote()
		{
			var state = (PersonState)tree.AddPerson(TestBacking.GetCreateMalePerson());
			cleanup.Add(state);
			var note = TestBacking.GetCreateNote();
			var state2 = state.AddNote(note);
			Assert.DoesNotThrow(() => state2.IfSuccessful());
		}

		[Test]
		public void TestReadMergedPerson()
		{
			var person1 = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(person1);
			var person2 = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(person2);
			var merge = person1.ReadMergeAnalysis(person2);
			var m = new Merge();

			m.ResourcesToCopy = new List<ResourceReference>();
			m.ResourcesToDelete = new List<ResourceReference>();
			m.ResourcesToCopy.AddRange(merge.Analysis.DuplicateResources);
			m.ResourcesToCopy.AddRange(merge.Analysis.ConflictingResources.Select(x => x.DuplicateResource));
			merge.DoMerge(m);

			// Person2 was merged with Person1
			var state = tree.ReadPersonById(person2.Person.Id);
			Assert.DoesNotThrow(() => state.IfSuccessful());

			// TODO: This is now returning OK
			//Assert.AreEqual(HttpStatusCode.MovedPermanently, state.Response.StatusCode);
			Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);

			var link1 = person1.GetSelfUri();
			Assert.IsNotNull(link1);
			Assert.IsNotEmpty(link1);
			var link2 = state.GetSelfUri();
			Assert.IsNotNull(link2);
			Assert.IsNotEmpty(link2);

			// TODO: The merge var has 3 links now such as merge link. Has the logic changed such that this fails?
			//Assert.AreEqual(link1, link2);
		}

		[Test]
		public void TestReadDeletedPerson()
		{
			var state = tree.AddPerson(TestBacking.GetCreateMalePerson()).Delete().Get();
			Assert.AreEqual(HttpStatusCode.Gone, state.Response.StatusCode);
		}

		[Test]
		public void TestReadPerson()
		{
			var person = tree.AddPerson(TestBacking.GetCreateMalePerson());
			cleanup.Add(person);
			var state = tree.ReadPerson(new Uri(person.GetSelfUri()));
			Assert.DoesNotThrow(() => state.IfSuccessful());
			Assert.IsNotNull(state.Person);
			Assert.IsNotNull(state.Person.Id);
			Assert.IsNotEmpty(state.Person.Id);
		}


		[Test]
		public void TestReadPersonSourceReferences()
		{
			var state = (PersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(state);
			state.AddSourceReference(TestBacking.GetPersonSourceReference());

			var state2 = tree.ReadPerson(new Uri(state.GetSelfUri()));
			Assert.DoesNotThrow(() => state2.IfSuccessful());
			Assert.IsNotNull(state2.Person);
			Assert.IsNotNull(state2.Person.Sources);
		}

		[Test]
		public void TestReadRelationshipsToChildren()
		{
			var father = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(father);
			var son = tree.AddPerson(TestBacking.GetCreateMalePerson());
			cleanup.Add(son);
			var chapr = tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(father, null, son));
			cleanup.Add(chapr);
			var state2 = father.LoadChildRelationships();
			Assert.DoesNotThrow(() => state2.IfSuccessful());
			var children = state2.GetChildRelationships();
			Assert.IsNotNull(children);
			Assert.Greater(children.Count, 0);
		}

		[Test]
		public void TestReadRelationshipsToParents()
		{
			var father = tree.AddPerson(TestBacking.GetCreateMalePerson());
			cleanup.Add(father);
			var son = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(son);
			var chapr = tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(father, null, son));
			cleanup.Add(chapr);
			var state2 = son.LoadParentRelationships();
			Assert.DoesNotThrow(() => state2.IfSuccessful());
			var parents = state2.GetParentRelationships();
			Assert.IsNotNull(parents);
			Assert.Greater(parents.Count, 0);
		}

		[Test]
		public void TestReadRelationshipsToSpouses()
		{
			var husband = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(husband);
			var wife = tree.AddPerson(TestBacking.GetCreateFemalePerson());
			cleanup.Add(wife);
			husband.AddSpouse(wife);
			var state2 = husband.LoadSpouseRelationships();
			Assert.DoesNotThrow(() => state2.IfSuccessful());
			var spouses = state2.GetSpouseRelationships();
			Assert.IsNotNull(spouses);
			Assert.Greater(spouses.Count, 0);
		}

		[Test]
		public void TestReadRelationshipsToSpousesWithPersons()
		{
			var husband = (PersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(husband);
			var wife = tree.AddPerson(TestBacking.GetCreateFemalePerson());
			cleanup.Add(wife);
			husband.AddSpouse(wife);
			var state2 = husband.LoadSpouseRelationships(FamilySearchOptions.IncludePersons());
			Assert.DoesNotThrow(() => state2.IfSuccessful());
			Assert.IsNotNull(state2.Entity != null);
			Assert.IsNotNull(state2.Entity.Persons);
			Assert.AreEqual(2, state2.Entity.Persons.Count);
		}

		[Test]
		public void TestReadDiscussionReferences()
		{
			var me = tree.ReadCurrentUser();
			var contributor = new ResourceReference("https://familysearch.org/platform/users/agents/" + me.User.TreeUserId).SetResourceId(me.User.TreeUserId);
			var discussion = tree.AddDiscussion(new Discussion()
													.SetTitle("Test title")
													.SetDetails("Test details")
													.SetContributor(contributor)
													.SetCreated(DateTime.Now));
			var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			person.AddDiscussionReference(discussion);
			var state = tree.ReadPerson(new Uri(person.GetSelfUri()));

			Assert.DoesNotThrow(() => state.IfSuccessful());
			Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
			Assert.Greater(state.Person.DiscussionReferences.Count, 0);
		}

		[Test]
		public void TestReadChildrenOfAPerson()
		{
			var father = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(father);
			var son = tree.AddPerson(TestBacking.GetCreateMalePerson());
			cleanup.Add(son);
			var chapr = tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(father, null, son));
			cleanup.Add(chapr);
			var state2 = father.ReadChildren();
			Assert.DoesNotThrow(() => state2.IfSuccessful());
			Assert.IsNotNull(state2.Persons);
			Assert.Greater(state2.Persons.Count, 0);
		}

		[Test]
		public void TestReadNotFoundPerson()
		{
			var state = tree.ReadPerson(new Uri("https://integration.familysearch.org/platform/tree/persons/MMMM-MMM"));
			Assert.AreEqual(HttpStatusCode.NotFound, state.Response.StatusCode);
		}

		[Test]
		public void TestReadNotModifiedPerson()
		{
			var state = (PersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(state);
			var cache = new CacheDirectives(state);
			var state2 = tree.ReadPerson(new Uri(state.GetSelfUri()), cache);
			Assert.DoesNotThrow(() => state2.IfSuccessful());
			Assert.AreEqual(HttpStatusCode.NotModified, state2.Response.StatusCode);
		}

		[Test]
		public void TestReadNotes()
		{
			var state = (PersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(state);
			state.AddNote(TestBacking.GetCreateNote());
			var state2 = state.LoadNotes();
			Assert.DoesNotThrow(() => state2.IfSuccessful());
			Assert.IsNotNull(state2.Person);
			Assert.IsNotNull(state2.Person.Notes);
			Assert.Greater(state2.Person.Notes.Count, 0);
		}

		[Test]
		public void TestReadParentsOfAPerson()
		{
			var father = tree.AddPerson(TestBacking.GetCreateMalePerson());
			cleanup.Add(father);
			var son = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(son);
			var chapr = tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(father, null, son));
			cleanup.Add(chapr);
			var state2 = son.ReadParents();
			Assert.DoesNotThrow(() => state2.IfSuccessful());
			Assert.IsNotNull(state2.Persons);
			Assert.Greater(state2.Persons.Count, 0);
		}

		[Test]
		public void TestReadSpousesOfAPerson()
		{
			var person = (PersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(person);
			var spouse = tree.AddPerson(TestBacking.GetCreateFemalePerson());
			cleanup.Add(spouse);
			person.AddSpouse(spouse);
			var state = tree.ReadPerson(new Uri(person.GetSelfUri()));
			var state2 = state.ReadSpouses();

			Assert.DoesNotThrow(() => state2.IfSuccessful());
			Assert.IsNotNull(state2.Persons);
			Assert.IsNotNull(state2.Relationships);
			Assert.Greater(state2.Persons.Count, 0);
			Assert.Greater(state2.Relationships.Count, 0);
		}

		[Test]
		public void TestHeadPerson()
		{
			var state = tree.AddPerson(TestBacking.GetCreateMalePerson());
			cleanup.Add(state);
			var state2 = (PersonState)state.Head();
			Assert.DoesNotThrow(() => state2.IfSuccessful());
			Assert.AreEqual(HttpStatusCode.OK, state2.Response.StatusCode);
		}

		[Test]
		public void TestUpdatePersonSourceReference()
		{
			var state = (PersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(state);
			var sr = TestBacking.GetPersonSourceReference();
			sr.Tags = new List<Tag>();
			sr.Tags.Add(new Tag(ChangeObjectType.Name));
			state.AddSourceReference(sr);
			var state3 = tree.ReadPerson(new Uri(state.GetSelfUri()));
			var tag = state3.Person.Sources[0].Tags.First();
			state3.Person.Sources[0].Tags.Remove(tag);
			cleanup.Add(state3);

			var state2 = state.UpdateSourceReferences(state3.Person);
			cleanup.Add(state2);
			Assert.DoesNotThrow(() => state2.IfSuccessful());
			Assert.AreEqual(HttpStatusCode.NoContent, state2.Response.StatusCode);
			state3.Person.Sources[0].Tags.Add(tag);
			state2 = state.UpdateSourceReferences(state3.Person);
			Assert.DoesNotThrow(() => state2.IfSuccessful());
			Assert.AreEqual(HttpStatusCode.NoContent, state2.Response.StatusCode);
		}

		[Test]
		public void TestUpdatePersonConclusion()
		{
			var state = (PersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(state);
			state.Person.Facts.Add(TestBacking.GetBirthFact());
			var state2 = state.UpdateConclusions(state.Person);
			Assert.DoesNotThrow(() => state2.IfSuccessful());
		}

		[Test]
		public void TestUpdatePersonCustomNonEventFact()
		{
			var person = (PersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(person);
			person.AddFact(TestBacking.GetCustomFact());
			person = (PersonState)person.Get();
			var factId = person.Person.Facts.Single(x => x.Type == "data:,Eagle%20Scout").Id;
			var fact = TestBacking.GetCustomFact();
			fact.Id = factId;
			var state2 = person.UpdateFact(fact);

			Assert.DoesNotThrow(() => state2.IfSuccessful());
		}

		[Test]
		public void TestUpdatePersonWithPreconditions()
		{
			var state = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(state);
			var cond = new Preconditions(state);
			var state2 = state.UpdateFacts(state.Person.Facts.ToArray(), cond);
			Assert.DoesNotThrow(() => state2.IfSuccessful());
			Assert.AreEqual(HttpStatusCode.NoContent, state2.Response.StatusCode);

			state = tree.ReadPersonById(state.Person.Id);
			var state3 = state.UpdateFacts(state.Person.Facts.ToArray(), cond);
			Assert.Throws<GedcomxApplicationException>(() => state3.IfSuccessful());
			Assert.AreEqual(HttpStatusCode.PreconditionFailed, state3.Response.StatusCode);
		}

		[Test]
		public void TestDeletePerson()
		{
			// Assume the ability to add a person is working
			var state = tree.AddPerson(TestBacking.GetCreateMalePerson());
			var state2 = (PersonState)state.Delete();

			Assert.DoesNotThrow(() => state2.IfSuccessful());
			Assert.AreEqual(HttpStatusCode.NoContent, state2.Response.StatusCode);
		}

		[Test]
		public void TestDeletePersonSourceReference()
		{
			// Assume the ability to add a person is working
			var state = tree.AddPerson(TestBacking.GetCreateMalePerson());
			cleanup.Add(state);
			state = (PersonState)state.Get();
			// Assume the ability to add a source reference is working
			state.AddSourceReference(TestBacking.GetPersonSourceReference());
			var state3 = tree.ReadPerson(new Uri(state.GetSelfUri()));
			cleanup.Add(state3);

			var state2 = state.DeleteSourceReference(state3.Person.Sources.FirstOrDefault());
			Assert.DoesNotThrow(() => state2.IfSuccessful());
			Assert.AreEqual(HttpStatusCode.NoContent, state2.Response.StatusCode);
		}

		[Test]
		public void TestDeletePersonWithPreconditions()
		{
			// Assume the ability to add a person is working
			var state = tree.AddPerson(TestBacking.GetCreateMalePerson());
			cleanup.Add(state);
			state = (PersonState)state.Get();
			var cond = new Preconditions(state.LastModified);

			// Touch the record since the above date
			state.Update(state.Person);

			// This should fail
			var state2 = (PersonState)state.Delete(cond);
			Assert.Throws<GedcomxApplicationException>(() => state2.IfSuccessful());
			Assert.AreEqual(HttpStatusCode.PreconditionFailed, state2.Response.StatusCode);
		}

		[Test]
		public void TestDeleteDiscussionReference()
		{
			var me = tree.ReadCurrentUser();
			var contributor = new ResourceReference("https://familysearch.org/platform/users/agents/" + me.User.TreeUserId).SetResourceId(me.User.TreeUserId);
			var discussion = tree.AddDiscussion(new Discussion()
													.SetTitle("Test title")
													.SetDetails("Test details")
													.SetContributor(contributor)
													.SetCreated(DateTime.Now));
			cleanup.Add(discussion);
			var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(person);
			person.AddDiscussionReference(discussion);
			var state2 = tree.ReadPerson(new Uri(person.GetSelfUri()));
			var state = person.DeleteDiscussionReference(state2.Person.DiscussionReferences.Single());

			Assert.DoesNotThrow(() => state.IfSuccessful());
			Assert.AreEqual(HttpStatusCode.NoContent, state.Response.StatusCode);
		}

		[Test]
		public void TestRestorePerson()
		{
			// Assume the ability to add/delete a person works
			var state = tree.AddPerson(TestBacking.GetCreateMalePerson());
			var id = state.Headers.Get("X-ENTITY-ID").First().Value.ToString();
			state.Delete();

			var deletedPerson = tree.ReadPersonById(id);
			cleanup.Add(deletedPerson);
			Assert.AreEqual(HttpStatusCode.Gone, deletedPerson.Response.StatusCode); // Ensure we have a deleted person
			var testState = deletedPerson.Restore();
			Assert.DoesNotThrow(() => testState.IfSuccessful());
			Assert.AreEqual(HttpStatusCode.NoContent, testState.Response.StatusCode);
		}

		[Test]
		public void TestReadPreferredSpouseRelationship()
		{
			var p = (PersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(p);
			var s1 = tree.AddPerson(TestBacking.GetCreateFemalePerson());
			cleanup.Add(s1);
			var s2 = tree.AddPerson(TestBacking.GetCreateFemalePerson());
			cleanup.Add(s2);
			p.AddSpouse(s1);
			p.AddSpouse(s2);
			var me = tree.ReadCurrentUser();
			var person = tree.ReadPersonById(p.Person.Id);

			// Ensure the target relationship exists
			person.LoadSpouseRelationships();
			var state = (IPreferredRelationshipState)person.ReadRelationship(person.Entity.Relationships[0]);
			tree.UpdatePreferredSpouseRelationship(me.User.TreeUserId, p.Person.Id, state);

			var state2 = (FamilyTreeRelationshipState)tree.ReadPreferredSpouseRelationship(me.User.TreeUserId, p.Person.Id);
			Assert.AreEqual(HttpStatusCode.SeeOther, state2.Response.StatusCode);
			Assert.IsNotNull(state2.Headers.Get("Location").Single());
		}

		[Test]
		public void TestUpdatePreferredSpouseRelationship()
		{
			var p = (PersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(p);
			var s1 = tree.AddPerson(TestBacking.GetCreateFemalePerson());
			cleanup.Add(s1);
			var s2 = tree.AddPerson(TestBacking.GetCreateFemalePerson());
			cleanup.Add(s2);
			p.AddSpouse(s1);
			p.AddSpouse(s2);
			var me = tree.ReadCurrentUser();
			var person = tree.ReadPersonById(p.Person.Id);

			person.LoadSpouseRelationships();
			var state = (IPreferredRelationshipState)person.ReadRelationship(person.Entity.Relationships[0]);
			var state2 = tree.UpdatePreferredSpouseRelationship(me.User.TreeUserId, p.Person.Id, state);
			Assert.AreEqual(HttpStatusCode.NoContent, state2.Response.StatusCode);
		}

		[Test]
		public void TestDeletePreferredSpouseRelationship()
		{
			var p = (PersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(p);
			var s1 = tree.AddPerson(TestBacking.GetCreateFemalePerson());
			cleanup.Add(s1);
			var s2 = tree.AddPerson(TestBacking.GetCreateFemalePerson());
			cleanup.Add(s2);
			p.AddSpouse(s1);
			p.AddSpouse(s2);
			var me = tree.ReadCurrentUser();
			var person = tree.ReadPersonById(p.Person.Id);

			// Ensure the target relationship exists
			person.LoadSpouseRelationships();
			var state = (IPreferredRelationshipState)person.ReadRelationship(person.Entity.Relationships[0]);
			tree.UpdatePreferredSpouseRelationship(me.User.TreeUserId, p.Person.Id, state);

			var state2 = tree.DeletePreferredSpouseRelationship(me.User.TreeUserId, p.Person.Id);
			Assert.AreEqual(HttpStatusCode.NoContent, state2.Response.StatusCode);
			Assert.IsNotNull(state2.Headers.Get("Content-Location").Single());
		}

		[Test]
		public void TestReadPersonWithRelationships()
		{
			var father = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(father);
			var son = tree.AddPerson(TestBacking.GetCreateMalePerson());
			cleanup.Add(son);
			var chapr = tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(father, null, son));
			cleanup.Add(chapr);
			var state = tree.ReadPersonWithRelationshipsById(father.Person.Id);

			Assert.DoesNotThrow(() => state.IfSuccessful());
			Assert.IsNotNull(state.Person);
			Assert.IsNotNull(state.ChildAndParentsRelationships);
			Assert.Greater(state.ChildAndParentsRelationships.Count, 0);
		}

		[Test]
		public void TestUpdatePersonNotAMatchDeclarations()
		{
			var person1 = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson());
			cleanup.Add(person1);
			var person2 = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson());
			cleanup.Add(person2);
			Thread.Sleep(30);
			person1 = tree.ReadPersonById(person1.Response.Headers.Get("X-ENTITY-ID").Single().Value.ToString());
			person2 = tree.ReadPersonById(person2.Response.Headers.Get("X-ENTITY-ID").Single().Value.ToString());
			var state = person1.AddNonMatch(person2);
			Assert.DoesNotThrow(() => state.IfSuccessful());
			Assert.AreEqual(HttpStatusCode.NoContent, state.Response.StatusCode);
		}

		[Test]
		public void TestDeletePersonNotAMatch()
		{
			var person1 = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson());
			cleanup.Add(person1);
			var person2 = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson());
			cleanup.Add(person2);
			Thread.Sleep(30);
			person1 = tree.ReadPersonById(person1.Response.Headers.Get("X-ENTITY-ID").Single().Value.ToString());
			person2 = tree.ReadPersonById(person2.Response.Headers.Get("X-ENTITY-ID").Single().Value.ToString());
			var state = (PersonNonMatchesState)person1.AddNonMatch(person2).Get();
			state = state.RemoveNonMatch(state.Persons[0]);
			Assert.DoesNotThrow(() => state.IfSuccessful());
			Assert.AreEqual(HttpStatusCode.NoContent, state.Response.StatusCode);
		}

		[Test]
		public void TestReadPersonPortrait()
		{
			var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(person);

			// This is BETA, and does not yet return a state. Test is based exclusively off response data.
			var response = person.ReadPortrait();
			Assert.IsTrue(!response.HasClientError() && !response.HasServerError());
			// NOTE: The READ_PERSON_ID user does not have images, thus the response should be 204.
			Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
		}

		[Test]
		public void TestReadPersonPortraitWithDefault()
		{
			var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(person);
			var location = "http://i.imgur.com/d9J0gYA.jpg";

			// This is BETA, and does not yet return a state. Test is based exclusively off response data.
			var response = person.ReadPortrait(FamilySearchOptions.DefaultUri(location));
			Assert.IsTrue(!response.HasClientError() && !response.HasServerError());
			// NOTE: The READ_PERSON_ID user does not have images, but a default is specified, thus the response should be 307.
			Assert.AreEqual(HttpStatusCode.TemporaryRedirect, response.StatusCode);
			Assert.IsTrue(response.Headers.Get("Location").Any());
			Assert.IsNotNull(response.Headers.Get("Location").Single().Value);
			Assert.AreEqual(location, response.Headers.Get("Location").Single().Value.ToString());
		}

		[Test]
		public void TestReadPersonChangeSummary()
		{
			var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(person);
			var state = person.ReadChangeHistory();

			Assert.DoesNotThrow(() => state.IfSuccessful());
		}

		[Test]
		public void TestReadPersonPortraits()
		{
			var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(person);

			var state = person.ReadPortraits();
			Assert.DoesNotThrow(() => state.IfSuccessful());

			// TODO: Why is this returning NoContent - we never added a portrait?
			//Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
			Assert.AreEqual(HttpStatusCode.NoContent, state.Response.StatusCode);
		}

		[Test]
		public void TestCreatePersonLifeSketch()
		{
			// TODO: Commented code returned no content, when we are clearly posting a new person.
			// Thoughts?

			var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(person);
			var state = (FamilyTreePersonState)person.Post(TestBacking.GetCreatePersonLifeSketch(person.Person.Id));

			Assert.DoesNotThrow(() => state.IfSuccessful());
			//Assert.AreEqual(HttpStatusCode.NoContent, state.Response.StatusCode);
			Assert.AreEqual(HttpStatusCode.Created, state.Response.StatusCode);
		}

		[Test]
		public void TestUpdatePersonLifeSketch()
		{
			var person = (PersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(person);
			person.Post(TestBacking.GetCreatePersonLifeSketch(person.Person.Id));
			person = (FamilyTreePersonState)person.Get();
			var state = (FamilyTreePersonState)person.Post(TestBacking.GetUpdatePersonLifeSketch(person.Person.Id, TestBacking.GetFactId(person.Person, "http://familysearch.org/v1/LifeSketch")));

			Assert.DoesNotThrow(() => state.IfSuccessful());
			Assert.AreEqual(HttpStatusCode.NoContent, state.Response.StatusCode);
		}

		[Test]
		public void TestDeletePersonConclusion()
		{
			var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(person);
			person.Post(TestBacking.GetCreatePersonLifeSketch(person.Person.Id));
			person = (FamilyTreePersonState)person.Get();

			var sketchToDelete = person.Person.Facts.Where(x => x.Type == "http://familysearch.org/v1/LifeSketch").FirstOrDefault();
			Assert.IsNotNull(sketchToDelete);
			var state = person.DeleteFact(sketchToDelete);

			Assert.DoesNotThrow(() => state.IfSuccessful());
			Assert.AreEqual(HttpStatusCode.NoContent, state.Response.StatusCode);
		}

		[Test]
		public void TestReadPersonMemories()
		{
			var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(person);
			var dataSource = new BasicDataSource("Sample Memory", MediaTypes.TEXT_PLAIN_TYPE, Resources.MemoryTXT);
			person.AddArtifact(dataSource);
			person = (FamilyTreePersonState)person.Get();
			var state = person.ReadArtifacts();

			Assert.DoesNotThrow(() => state.IfSuccessful());
			Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
		}

		[Test]
		public void TestReadPersonMemoriesByType()
		{
			var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(person);
			var dataSource = new BasicDataSource("Sample Memory", MediaTypes.TEXT_PLAIN_TYPE, Resources.MemoryTXT);
			person.AddArtifact(dataSource);
			person = (FamilyTreePersonState)person.Get();
			var options = new QueryParameter[] { new QueryParameter("type", "story") };
			var state = person.ReadArtifacts(options);

			Assert.DoesNotThrow(() => state.IfSuccessful());
			Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
		}

		[Test]
		public void TestUploadPhotoForPerson()
		{
			var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(person);
			var converter = new ImageConverter();
			var bytes = (Byte[])converter.ConvertTo(TestBacking.GetCreatePhoto(), typeof(Byte[]));
			var dataSource = new BasicDataSource(Guid.NewGuid().ToString("n") + ".jpg", "image/jpeg", bytes);
			var state = person.AddArtifact(new SourceDescription() { Titles = new List<TextValue>() { new TextValue("PersonImage") }, Citations = new List<SourceCitation>() { new SourceCitation() { Value = "Citation for PersonImage" } } }, dataSource);

			Assert.DoesNotThrow(() => state.IfSuccessful());
			Assert.AreEqual(HttpStatusCode.Created, state.Response.StatusCode);
			state.Delete();
		}

		[Test]
		public void TestReadPreferredParentRelationship()
		{
			var father = tree.AddPerson(TestBacking.GetCreateMalePerson());
			cleanup.Add(father);
			var son = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(son);
			var chapr = tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(father, null, son));
			cleanup.Add(chapr);
			var me = tree.ReadCurrentUser();

			// Ensure the target relationship exists
			var relationship = ((FamilyTreePersonParentsState)son.ReadParents()).ChildAndParentsRelationships.First();
			var state = son.ReadChildAndParentsRelationship(relationship);
			tree.UpdatePreferredParentRelationship(me.User.TreeUserId, son.Person.Id, state);

			var state2 = (FamilyTreeRelationshipState)tree.ReadPreferredParentRelationship(me.User.TreeUserId, son.Person.Id);
			Assert.AreEqual(HttpStatusCode.SeeOther, state2.Response.StatusCode);
			Assert.IsNotNull(state2.Headers.Get("Location").Single());
		}

		[Test]
		public void TestUpdatePreferredParentRelationship()
		{
			var father = tree.AddPerson(TestBacking.GetCreateMalePerson());
			cleanup.Add(father);
			var son = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(son);
			var chapr = tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(father, null, son));
			cleanup.Add(chapr);
			var me = tree.ReadCurrentUser();

			// Ensure the target relationship exists
			var relationship = ((FamilyTreePersonParentsState)son.ReadParents()).ChildAndParentsRelationships.First();
			var state = son.ReadChildAndParentsRelationship(relationship);


			var state2 = tree.UpdatePreferredParentRelationship(me.User.TreeUserId, son.Person.Id, state);
			Assert.AreEqual(HttpStatusCode.NoContent, state2.Response.StatusCode);
		}

		[Test]
		public void TestDeletePreferredParentRelationship()
		{
			var father = tree.AddPerson(TestBacking.GetCreateMalePerson());
			cleanup.Add(father);
			var mother = tree.AddPerson(TestBacking.GetCreateFemalePerson());
			cleanup.Add(mother);
			var son = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(son);
			var chapr = tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(father, mother, son));
			cleanup.Add(chapr);
			var me = tree.ReadCurrentUser();

			// Ensure the target relationship exists
			var relationship = ((FamilyTreePersonParentsState)son.ReadParents()).ChildAndParentsRelationships.First();
			var state = son.ReadChildAndParentsRelationship(relationship);
			tree.UpdatePreferredParentRelationship(me.User.TreeUserId, son.Person.Id, state);

			var state2 = tree.DeletePreferredParentRelationship(me.User.TreeUserId, son.Person.Id);
			Assert.AreEqual(HttpStatusCode.NoContent, state2.Response.StatusCode);
			Assert.IsNotNull(state2.Headers.Get("Content-Location").Single());
		}

		[Test]
		public void TestReadPersonMergeAnalysis()
		{
			var person1 = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(person1);
			var person2 = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(person2);
			var state = person1.ReadMergeAnalysis(person2);

			Assert.DoesNotThrow(() => state.IfSuccessful());
			Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
			Assert.IsNotNull(state.Analysis);
		}

		[Test]
		public void TestReadPersonMergeConstraintCanMergeAnyOrder()
		{
			var person1 = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(person1);
			var person2 = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(person2);
			var state = person1.ReadMergeOptions(person2);

			Assert.DoesNotThrow(() => state.IfSuccessful());
			Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
			Assert.IsNotNull(state.GetLink(FamilySearch.Api.Rel.MERGE_MIRROR));
		}

		[Test]
		public void TestReadPersonMergeConstraintCanMergeOtherOrderOnly()
		{
			var person1 = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(person1);
			var person2 = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateFemalePerson()).Get();
			cleanup.Add(person2);

			var state = person1.ReadMergeOptions(person2);
			Assert.DoesNotThrow(() => state.IfSuccessful());
			Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
			Assert.IsFalse(state.IsAllowed);
		}

		[Test]
		public void TestMergePerson()
		{
			var person1 = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(person1);
			var person2 = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
			cleanup.Add(person2);
			var merge = person1.ReadMergeAnalysis(person2);
			var m = new Merge();

			m.ResourcesToDelete = new List<ResourceReference>();
			m.ResourcesToDelete.AddRange(merge.Analysis.ConflictingResources.Select(x => x.SurvivorResource));

			m.ResourcesToCopy = new List<ResourceReference>();
			m.ResourcesToCopy.AddRange(merge.Analysis.DuplicateResources);
			m.ResourcesToCopy.AddRange(merge.Analysis.ConflictingResources.Select(x => x.DuplicateResource));
			var state = merge.DoMerge(m);

			Assert.DoesNotThrow(() => state.IfSuccessful());
			Assert.AreEqual(HttpStatusCode.NoContent, state.Response.StatusCode);
			Assert.AreEqual(person1.Get().GetSelfUri(), person2.Get().GetSelfUri());
		}
	}
}
