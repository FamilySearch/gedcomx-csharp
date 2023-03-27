using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading;

using FamilySearch.Api;
using FamilySearch.Api.Ft;
using FamilySearch.Api.Util;

using Gedcomx.Support;

using Gx.Common;
using Gx.Fs.Discussions;
using Gx.Fs.Tree;
using Gx.Model.Collections;
using Gx.Rs.Api;
using Gx.Rs.Api.Options;
using Gx.Rs.Api.Util;
using Gx.Source;

using NUnit.Framework;

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
        public void TestCreatePerson()
        {
            var result = tree.AddPerson(TestBacking.GetCreateMalePerson());
            cleanup.Add(result);
            Assert.DoesNotThrow(() => result.IfSuccessful());
            var person = (PersonState)result.Get();

            Assert.That(person.Person, Is.Not.Null);
            Assert.That(person.Person.Id, Is.Not.Null);
            Assert.That(person.Person.Id, Is.Not.Empty);
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
            Assert.That(result2.Response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }

        [Test]
        public void TestCreatePersonConclusion()
        {
            var state = (PersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(state);
            var conclusion = TestBacking.GetCreatePersonConclusion(state.Person.Id);
            var state2 = state.UpdateConclusions(conclusion);
            Assert.That(state2, Is.Not.Null);
            Assert.DoesNotThrow(() => state2.IfSuccessful());
        }

        [Test]
        public void TestCreateDiscussionReference()
        {
            var me = tree.ReadCurrentUser();
            var contributor = new ResourceReference("https://www.familysearch.org/platform/users/agents/" + me.User.TreeUserId).SetResourceId(me.User.TreeUserId);
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
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(state2.Person.AnyDiscussionReferences(), Is.True);
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

        [Test, Category("AccountNeeded")]
        public void TestReadMergedPerson()
        {
            var person1 = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(person1);
            var person2 = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(person2);
            var merge = person1.ReadMergeAnalysis(person2);
            var m = new Merge
            {
                ResourcesToCopy = new List<ResourceReference>(),
                ResourcesToDelete = new List<ResourceReference>()
            };
            m.ResourcesToCopy.AddRange(merge.Analysis.DuplicateResources);
            m.ResourcesToCopy.AddRange(merge.Analysis.ConflictingResources.Select(x => x.DuplicateResource));
            merge.DoMerge(m);

            // Person2 was merged with Person1
            var state = tree.ReadPersonById(person2.Person.Id);
            Assert.DoesNotThrow(() => state.IfSuccessful());

            // TODO: This is now returning OK
            //Assert.AreEqual(HttpStatusCode.MovedPermanently, state.Response.StatusCode);
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var link1 = person1.GetSelfUri();
            Assert.That(link1, Is.Not.Null);
            Assert.That(link1, Is.Not.Empty);
            var link2 = state.GetSelfUri();
            Assert.That(link2, Is.Not.Null);
            Assert.That(link2, Is.Not.Empty);

            // TODO: The merge var has 3 links now such as merge link. Has the logic changed such that this fails?
            //Assert.AreEqual(link1, link2);
        }

        [Test]
        public void TestReadDeletedPerson()
        {
            var state = tree.AddPerson(TestBacking.GetCreateMalePerson()).Delete().Get();
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.Gone));
        }

        [Test]
        public void TestReadPerson()
        {
            var person = tree.AddPerson(TestBacking.GetCreateMalePerson());
            cleanup.Add(person);
            var state = tree.ReadPerson(new Uri(person.GetSelfUri()));
            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Person, Is.Not.Null);
            Assert.That(state.Person.Id, Is.Not.Null);
            Assert.That(state.Person.Id, Is.Not.Empty);
        }


        [Test]
        public void TestReadPersonSourceReferences()
        {
            var state = (PersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(state);
            state.AddSourceReference(TestBacking.GetPersonSourceReference());

            var state2 = tree.ReadPerson(new Uri(state.GetSelfUri()));
            Assert.DoesNotThrow(() => state2.IfSuccessful());
            Assert.That(state2.Person, Is.Not.Null);
            Assert.That(state2.Person.Sources, Is.Not.Null);
        }

        [Test, Category("AccountNeeded")]
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
            Assert.That(children, Is.Not.Null);
            Assert.That(children, Is.Not.Empty);
        }

        [Test, Category("AccountNeeded")]
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
            Assert.That(parents, Is.Not.Null);
            Assert.That(parents, Is.Not.Empty);
        }

        [Test, Category("AccountNeeded")]
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
            Assert.That(spouses, Is.Not.Null);
            Assert.That(spouses, Is.Not.Empty);
        }

        [Test, Category("AccountNeeded")]
        public void TestReadRelationshipsToSpousesWithPersons()
        {
            var husband = (PersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(husband);
            var wife = tree.AddPerson(TestBacking.GetCreateFemalePerson());
            cleanup.Add(wife);
            husband.AddSpouse(wife);
            var state2 = husband.LoadSpouseRelationships(FamilySearchOptions.IncludePersons());
            Assert.DoesNotThrow(() => state2.IfSuccessful());
            Assert.That(state2.Entity, Is.Not.Null);
            Assert.That(state2.Entity.AnyPersons(), Is.True);
            Assert.That(state2.Entity.Persons, Has.Count.EqualTo(2));
        }

        [Test]
        public void TestReadDiscussionReferences()
        {
            var me = tree.ReadCurrentUser();
            var contributor = new ResourceReference("https://www.familysearch.org/platform/users/agents/" + me.User.TreeUserId).SetResourceId(me.User.TreeUserId);
            var discussion = tree.AddDiscussion(new Discussion()
                                .SetTitle("Test title")
                                .SetDetails("Test details")
                                .SetContributor(contributor)
                                .SetCreated(DateTime.Now));
            var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            person.AddDiscussionReference(discussion);
            var state = tree.ReadPerson(new Uri(person.GetSelfUri()));

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(state.Person.AnyDiscussionReferences(), Is.True);
        }

        [Test, Category("AccountNeeded")]
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
            Assert.That(state2.Persons, Is.Not.Null);
            Assert.That(state2.Persons, Is.Not.Empty);
        }

        [Test]
        public void TestReadNotFoundPerson()
        {
            var state = tree.ReadPerson(new Uri("https://api-integ.familysearch.org/platform/tree/persons/MMMM-MMM"));
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public void TestReadNotModifiedPerson()
        {
            var state = (PersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(state);
            var cache = new CacheDirectives(state);
            var state2 = tree.ReadPerson(new Uri(state.GetSelfUri()), cache);
            Assert.DoesNotThrow(() => state2.IfSuccessful());
            Assert.That(state2.Response.StatusCode, Is.EqualTo(HttpStatusCode.NotModified));
        }

        [Test]
        public void TestReadNotes()
        {
            var state = (PersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(state);
            state.AddNote(TestBacking.GetCreateNote());
            var state2 = state.LoadNotes();
            Assert.DoesNotThrow(() => state2.IfSuccessful());
            Assert.That(state2.Person, Is.Not.Null);
            Assert.That(state2.Person.AnyNotes(), Is.True);
        }

        [Test, Category("AccountNeeded")]
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
            Assert.That(state2.Persons, Is.Not.Null);
            Assert.That(state2.Persons, Is.Not.Empty);
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
            Assert.That(state2.Persons, Is.Not.Null);
            Assert.That(state2.Relationships, Is.Not.Null);
            Assert.That(state2.Persons, Is.Not.Empty);
            Assert.That(state2.Relationships, Is.Not.Empty);
        }

        [Test]
        public void TestHeadPerson()
        {
            var state = tree.AddPerson(TestBacking.GetCreateMalePerson());
            cleanup.Add(state);
            var state2 = (PersonState)state.Head();
            Assert.DoesNotThrow(() => state2.IfSuccessful());
            Assert.That(state2.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void TestUpdatePersonSourceReference()
        {
            var state = (PersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(state);
            var sr = TestBacking.GetPersonSourceReference();
            sr.Tags = new List<Tag>
            {
                new Tag(ChangeObjectType.Name)
            };
            state.AddSourceReference(sr);
            var state3 = tree.ReadPerson(new Uri(state.GetSelfUri()));
            var tag = state3.Person.Sources[0].Tags.First();
            state3.Person.Sources[0].Tags.Remove(tag);
            cleanup.Add(state3);

            var state2 = state.UpdateSourceReferences(state3.Person);
            cleanup.Add(state2);
            Assert.DoesNotThrow(() => state2.IfSuccessful());
            Assert.That(state2.Response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            state3.Person.Sources[0].Tags.Add(tag);
            state2 = state.UpdateSourceReferences(state3.Person);
            Assert.DoesNotThrow(() => state2.IfSuccessful());
            Assert.That(state2.Response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
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

        [Test, Category("AccountNeeded")]
        public void TestUpdatePersonWithPreconditions()
        {
            var state = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(state);
            var cond = new Preconditions(state);
            var state2 = state.UpdateFacts(state.Person.Facts.ToArray(), cond);
            Assert.DoesNotThrow(() => state2.IfSuccessful());
            Assert.That(state2.Response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));

            state = tree.ReadPersonById(state.Person.Id);
            var state3 = state.UpdateFacts(state.Person.Facts.ToArray(), cond);
            Assert.Throws<GedcomxApplicationException>(() => state3.IfSuccessful());
            Assert.That(state3.Response.StatusCode, Is.EqualTo(HttpStatusCode.PreconditionFailed));
        }

        [Test]
        public void TestDeletePerson()
        {
            // Assume the ability to add a person is working
            var state = tree.AddPerson(TestBacking.GetCreateMalePerson());
            var state2 = (PersonState)state.Delete();

            Assert.DoesNotThrow(() => state2.IfSuccessful());
            Assert.That(state2.Response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
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
            Assert.That(state2.Response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
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
            Assert.That(state2.Response.StatusCode, Is.EqualTo(HttpStatusCode.PreconditionFailed));
        }

        [Test]
        public void TestDeleteDiscussionReference()
        {
            var me = tree.ReadCurrentUser();
            var contributor = new ResourceReference("https://www.familysearch.org/platform/users/agents/" + me.User.TreeUserId).SetResourceId(me.User.TreeUserId);
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
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test, Category("AccountNeeded")]
        public void TestRestorePerson()
        {
            // Assume the ability to add/delete a person works
            var state = tree.AddPerson(TestBacking.GetCreateMalePerson());
            var id = state.Headers.Get("X-ENTITY-ID").First().Value.ToString();
            state.Delete();

            var deletedPerson = tree.ReadPersonById(id);
            cleanup.Add(deletedPerson);
            Assert.That(deletedPerson.Response.StatusCode, Is.EqualTo(HttpStatusCode.Gone)); // Ensure we have a deleted person
            var testState = deletedPerson.Restore();
            Assert.DoesNotThrow(() => testState.IfSuccessful());
            Assert.That(testState.Response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test, Category("AccountNeeded")]
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
            Assert.That(state2.Response.StatusCode, Is.EqualTo(HttpStatusCode.SeeOther));
            Assert.That(state2.Headers.Get("Location").Single(), Is.Not.Null);
        }

        [Test, Category("AccountNeeded")]
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
            Assert.That(state2.Response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test, Category("AccountNeeded")]
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
            Assert.That(state2.Response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            Assert.That(state2.Headers.Get("Content-Location").Single(), Is.Not.Null);
        }

        [Test, Category("AccountNeeded")]
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
            Assert.That(state.Person, Is.Not.Null);
            Assert.That(state.ChildAndParentsRelationships, Is.Not.Null);
            Assert.That(state.ChildAndParentsRelationships, Is.Not.Empty);
        }

        [Test, Category("AccountNeeded")]
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
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test, Category("AccountNeeded")]
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
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test]
        public void TestReadPersonPortrait()
        {
            var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(person);

            // This is BETA, and does not yet return a state. Test is based exclusively off response data.
            var response = person.ReadPortrait();
            Assert.That(!response.HasClientError() && !response.HasServerError(), Is.True);
            // NOTE: The READ_PERSON_ID user does not have images, thus the response should be 204.
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test]
        public void TestReadPersonPortraitWithDefault()
        {
            var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(person);
            var location = "http://i.imgur.com/d9J0gYA.jpg";

            // This is BETA, and does not yet return a state. Test is based exclusively off response data.
            var response = person.ReadPortrait(FamilySearchOptions.DefaultUri(location));
            Assert.That(!response.HasClientError() && !response.HasServerError(), Is.True);
            // NOTE: The READ_PERSON_ID user does not have images, but a default is specified, thus the response should be 307.
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.TemporaryRedirect));
            Assert.That(response.Headers.Get("Location").Any(), Is.True);
            Assert.That(response.Headers.Get("Location").Single().Value, Is.Not.Null);
            Assert.That(response.Headers.Get("Location").Single().Value.ToString(), Is.EqualTo(location));
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
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
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
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
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
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test]
        public void TestDeletePersonConclusion()
        {
            var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(person);
            person.Post(TestBacking.GetCreatePersonLifeSketch(person.Person.Id));
            person = (FamilyTreePersonState)person.Get();

            var sketchToDelete = person.Person.Facts.Where(x => x.Type == "http://familysearch.org/v1/LifeSketch").FirstOrDefault();
            Assert.That(sketchToDelete, Is.Not.Null);
            var state = person.DeleteFact(sketchToDelete);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
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
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
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
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void TestUploadPhotoForPerson()
        {
            var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(person);
            var converter = new ImageConverter();
            var bytes = (byte[])converter.ConvertTo(TestBacking.GetCreatePhoto(), typeof(byte[]));
            var dataSource = new BasicDataSource(Guid.NewGuid().ToString("n") + ".jpg", "image/jpeg", bytes);
            var state = person.AddArtifact(new SourceDescription() { Titles = new TextValues() { "PersonImage" }, Citations = new List<SourceCitation>() { new SourceCitation() { Value = "Citation for PersonImage" } } }, dataSource);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            state.Delete();
        }

        [Test, Category("AccountNeeded")]
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
            Assert.That(state2.Response.StatusCode, Is.EqualTo(HttpStatusCode.SeeOther));
            Assert.That(state2.Headers.Get("Location").Single(), Is.Not.Null);
        }

        [Test, Category("AccountNeeded")]
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
            Assert.That(state2.Response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test, Category("AccountNeeded")]
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
            Assert.That(state2.Response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            Assert.That(state2.Headers.Get("Content-Location").Single(), Is.Not.Null);
        }

        [Test, Category("AccountNeeded")]
        public void TestReadPersonMergeAnalysis()
        {
            var person1 = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(person1);
            var person2 = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(person2);
            var state = person1.ReadMergeAnalysis(person2);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(state.Analysis, Is.Not.Null);
        }

        [Test, Category("AccountNeeded")]
        public void TestReadPersonMergeConstraintCanMergeAnyOrder()
        {
            var person1 = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(person1);
            var person2 = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(person2);
            var state = person1.ReadMergeOptions(person2);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(state.GetLink(FamilySearch.Api.Rel.MERGE_MIRROR), Is.Not.Null);
        }

        [Test, Category("AccountNeeded")]
        public void TestReadPersonMergeConstraintCanMergeOtherOrderOnly()
        {
            var person1 = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(person1);
            var person2 = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateFemalePerson()).Get();
            cleanup.Add(person2);

            var state = person1.ReadMergeOptions(person2);
            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(state.IsAllowed, Is.False);
        }

        [Test, Category("AccountNeeded")]
        public void TestMergePerson()
        {
            var person1 = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(person1);
            var person2 = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(person2);
            var merge = person1.ReadMergeAnalysis(person2);
            var m = new Merge
            {
                ResourcesToDelete = new List<ResourceReference>()
            };
            m.ResourcesToDelete.AddRange(merge.Analysis.ConflictingResources.Select(x => x.SurvivorResource));

            m.ResourcesToCopy = new List<ResourceReference>();
            m.ResourcesToCopy.AddRange(merge.Analysis.DuplicateResources);
            m.ResourcesToCopy.AddRange(merge.Analysis.ConflictingResources.Select(x => x.DuplicateResource));
            var state = merge.DoMerge(m);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            Assert.That(person2.Get().GetSelfUri(), Is.EqualTo(person1.Get().GetSelfUri()));
        }
    }
}
