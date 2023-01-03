using System.Collections.Generic;
using System.Linq;
using System.Net;

using FamilySearch.Api.Ft;
using FamilySearch.Api.Util;

using Gx.Rs.Api;
using Gx.Rs.Api.Util;

using NUnit.Framework;

namespace Gedcomx.Rs.Api.Test
{
    [TestFixture, Category("AccountNeeded")]
    public class PedigreeTests
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
        public void TestReadPersonAncestry()
        {
            var grandfather = tree.AddPerson(TestBacking.GetCreateMalePerson());
            cleanup.Add(grandfather);
            var father = tree.AddPerson(TestBacking.GetCreateMalePerson());
            cleanup.Add(father);
            var son = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(son);
            var rel1 = tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(grandfather, null, father));
            cleanup.Add(rel1);
            var rel2 = tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(father, null, son));
            cleanup.Add(rel2);
            son = tree.ReadPersonById(son.Person.Id);
            var state = son.ReadAncestry();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(state.Tree, Is.Not.Null);
            Assert.That(state.Tree.Root, Is.Not.Null);
            Assert.That(state.Tree.Root.Person, Is.Not.Null);
            Assert.That(state.Tree.Root.Father, Is.Not.Null);
            Assert.That(state.Tree.Root.Father.Person, Is.Not.Null);
            Assert.That(state.Tree.Root.Father.Father, Is.Not.Null);
            Assert.That(state.Tree.Root.Father.Father.Person, Is.Not.Null);
            Assert.That(state.Tree.Root.Father.Father.Person.GetLink("self").Href, Is.EqualTo(grandfather.GetSelfUri()));
            Assert.That(state.Tree.Root.Father.Person.GetLink("self").Href, Is.EqualTo(father.GetSelfUri()));
            Assert.That(state.Tree.Root.Person.GetLink("self").Href, Is.EqualTo(son.GetSelfUri()));
        }

        [Test]
        public void TestReadPersonAncestryAndAdditionalPersonDetails()
        {
            var grandfather = tree.AddPerson(TestBacking.GetCreateMalePerson());
            cleanup.Add(grandfather);
            var father = tree.AddPerson(TestBacking.GetCreateMalePerson());
            cleanup.Add(father);
            var son = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(son);
            var rel1 = tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(grandfather, null, father));
            cleanup.Add(rel1);
            var rel2 = tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(father, null, son));
            cleanup.Add(rel2);
            son = tree.ReadPersonById(son.Person.Id);
            var state = son.ReadAncestry(FamilySearchOptions.IncludePersonDetails());

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(state.Tree, Is.Not.Null);
            Assert.That(state.Tree.Root, Is.Not.Null);
            Assert.That(state.Tree.Root.Person, Is.Not.Null);
            Assert.That(state.Tree.Root.Father, Is.Not.Null);
            Assert.That(state.Tree.Root.Father.Person, Is.Not.Null);
            Assert.That(state.Tree.Root.Father.Father, Is.Not.Null);
            Assert.That(state.Tree.Root.Father.Father.Person, Is.Not.Null);
            Assert.That(state.Tree.Root.Person.Facts, Is.Not.Null);
            Assert.That(state.Tree.Root.Father.Person.Facts, Is.Not.Null);
            Assert.That(state.Tree.Root.Father.Father.Person.Facts, Is.Not.Null);
            Assert.That(state.Tree.Root.Father.Father.Person.GetLink("self").Href, Is.EqualTo(grandfather.GetSelfUri()));
            Assert.That(state.Tree.Root.Father.Person.GetLink("self").Href, Is.EqualTo(father.GetSelfUri()));
            Assert.That(state.Tree.Root.Person.GetLink("self").Href, Is.EqualTo(son.GetSelfUri()));
        }

        [Test]
        public void TestReadPersonAncestryWithSpecifiedSpouse()
        {
            var grandfather = tree.AddPerson(TestBacking.GetCreateMalePerson());
            cleanup.Add(grandfather);
            var father = tree.AddPerson(TestBacking.GetCreateMalePerson());
            cleanup.Add(father);
            var husband = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(husband);
            var wife = tree.AddPerson(TestBacking.GetCreateFemalePerson());
            cleanup.Add(wife);
            var rel1 = husband.AddSpouse(wife);
            cleanup.Add(rel1);
            var rel2 = tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(grandfather, null, father));
            cleanup.Add(rel2);
            var rel3 = tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(father, null, husband));
            cleanup.Add(rel3);
            husband = tree.ReadPersonById(husband.Person.Id);
            var state = husband.ReadAncestry(FamilySearchOptions.SpouseId(wife.Headers.Get("X-ENTITY-ID").Single().Value.ToString()));

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(state.Tree, Is.Not.Null);
            Assert.That(state.Tree.Root, Is.Not.Null);
            Assert.That(state.Tree.Root.Mother, Is.Not.Null);
            Assert.That(state.Tree.Root.Mother.Person, Is.Not.Null);
            Assert.That(state.Tree.Root.Father, Is.Not.Null);
            Assert.That(state.Tree.Root.Father.Person, Is.Not.Null);
            Assert.That(state.Tree.Root.Father.Father, Is.Not.Null);
            Assert.That(state.Tree.Root.Father.Father.Person, Is.Not.Null);
            Assert.That(state.Tree.Root.Father.Father.Father, Is.Not.Null);
            Assert.That(state.Tree.Root.Father.Father.Father.Person, Is.Not.Null);
            Assert.That(state.Tree.Root.Father.Father.Father.Person.GetLink("self").Href, Is.EqualTo(grandfather.GetSelfUri()));
            Assert.That(state.Tree.Root.Father.Father.Person.GetLink("self").Href, Is.EqualTo(father.GetSelfUri()));
            Assert.That(state.Tree.Root.Father.Person.GetLink("self").Href, Is.EqualTo(husband.GetSelfUri()));
            Assert.That(state.Tree.Root.Mother.Person.GetLink("self").Href, Is.EqualTo(wife.GetSelfUri()));
        }

        [Test]
        public void TestReadPersonAncestryWithSpecifiedSpouseAndAdditionalPersonAndMarriageDetails()
        {
            var grandfather = tree.AddPerson(TestBacking.GetCreateMalePerson());
            cleanup.Add(grandfather);
            var father = tree.AddPerson(TestBacking.GetCreateMalePerson());
            cleanup.Add(father);
            var husband = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(husband);
            var wife = tree.AddPerson(TestBacking.GetCreateFemalePerson());
            cleanup.Add(wife);
            var rel1 = husband.AddSpouse(wife).AddFact(TestBacking.GetMarriageFact());
            cleanup.Add(rel1);
            var rel2 = tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(grandfather, null, father));
            cleanup.Add(rel2);
            var rel3 = tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(father, null, husband));
            cleanup.Add(rel3);
            husband = tree.ReadPersonById(husband.Person.Id);
            var state = husband.ReadAncestry(FamilySearchOptions.SpouseId(wife.Headers.Get("X-ENTITY-ID").Single().Value.ToString()), FamilySearchOptions.IncludePersonDetails(), FamilySearchOptions.IncludeMarriageDetails());

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(state.Tree, Is.Not.Null);
            Assert.That(state.Tree.Root, Is.Not.Null);
            Assert.That(state.Tree.Root.Mother, Is.Not.Null);
            Assert.That(state.Tree.Root.Mother.Person, Is.Not.Null);
            Assert.That(state.Tree.Root.Mother.Person.DisplayExtension, Is.Not.Null);
            Assert.That(state.Tree.Root.Mother.Person.DisplayExtension.MarriageDate, Is.Not.Null);
            Assert.That(state.Tree.Root.Father, Is.Not.Null);
            Assert.That(state.Tree.Root.Father.Person, Is.Not.Null);
            Assert.That(state.Tree.Root.Father.Father, Is.Not.Null);
            Assert.That(state.Tree.Root.Father.Father.Person, Is.Not.Null);
            Assert.That(state.Tree.Root.Father.Father.Father, Is.Not.Null);
            Assert.That(state.Tree.Root.Father.Father.Father.Person, Is.Not.Null);
            Assert.That(state.Tree.Root.Father.Person.Facts, Is.Not.Null);
            Assert.That(state.Tree.Root.Mother.Person.Facts, Is.Not.Null);
            Assert.That(state.Tree.Root.Father.Father.Father.Person.GetLink("self").Href, Is.EqualTo(grandfather.GetSelfUri()));
            Assert.That(state.Tree.Root.Father.Father.Person.GetLink("self").Href, Is.EqualTo(father.GetSelfUri()));
            Assert.That(state.Tree.Root.Father.Person.GetLink("self").Href, Is.EqualTo(husband.GetSelfUri()));
            Assert.That(state.Tree.Root.Mother.Person.GetLink("self").Href, Is.EqualTo(wife.GetSelfUri()));
        }

        [Test]
        public void TestReadPersonDescendancy()
        {
            var father = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(father);
            var son = tree.AddPerson(TestBacking.GetCreateMalePerson());
            cleanup.Add(son);
            var rel1 = tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(father, null, son));
            cleanup.Add(rel1);
            var state = father.ReadDescendancy();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(state.Tree, Is.Not.Null);
            Assert.That(state.Tree.Root, Is.Not.Null);
            Assert.That(state.Tree.Root.Person, Is.Not.Null);
            Assert.That(state.Tree.Root.Children, Is.Not.Null);
            Assert.That(state.Tree.Root.Children, Has.Count.EqualTo(1));
            Assert.That(state.Tree.Root.Children[0].Person, Is.Not.Null);
            Assert.That(state.Tree.Root.Person.Id, Is.EqualTo(father.Person.Id));
            Assert.That(state.Tree.Root.Children[0].Person.Id, Is.EqualTo(son.Headers.Get("X-ENTITY-ID").Single().Value.ToString()));
        }

        [Test]
        public void TestReadPersonDescendancyAndAdditionalPersonAndMarriageDetails()
        {
            var father = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(father);
            var mother = tree.AddPerson(TestBacking.GetCreateFemalePerson());
            cleanup.Add(mother);
            var son = tree.AddPerson(TestBacking.GetCreateMalePerson());
            cleanup.Add(son);
            var rel1 = father.AddSpouse(mother).AddFact(TestBacking.GetMarriageFact());
            cleanup.Add(rel1);
            var rel2 = tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(father, mother, son));
            cleanup.Add(rel2);
            var state = father.ReadDescendancy(FamilySearchOptions.IncludePersonDetails(), FamilySearchOptions.IncludeMarriageDetails());

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(state.Tree, Is.Not.Null);
            Assert.That(state.Tree.Root, Is.Not.Null);
            Assert.That(state.Tree.Root.Person, Is.Not.Null);
            Assert.That(state.Tree.Root.Spouse, Is.Not.Null);
            Assert.That(state.Tree.Root.Children, Is.Not.Null);
            Assert.That(state.Tree.Root.Spouse.DisplayExtension, Is.Not.Null);
            Assert.That(state.Tree.Root.Spouse.DisplayExtension.MarriageDate, Is.Not.Null);
            Assert.That(state.Tree.Root.Person.Facts, Is.Not.Null);
            Assert.That(state.Tree.Root.Spouse.Facts, Is.Not.Null);
            Assert.That(state.Tree.Root.Children, Has.Count.EqualTo(1));
            Assert.That(state.Tree.Root.Children[0].Person, Is.Not.Null);
            Assert.That(state.Tree.Root.Person.Id, Is.EqualTo(father.Person.Id));
            Assert.That(state.Tree.Root.Spouse.Id, Is.EqualTo(mother.Headers.Get("X-ENTITY-ID").Single().Value.ToString()));
            Assert.That(state.Tree.Root.Children[0].Person.Id, Is.EqualTo(son.Headers.Get("X-ENTITY-ID").Single().Value.ToString()));
        }

        [Test]
        public void TestReadPersonDescendancyWithSpecifiedSpouse()
        {
            var father = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(father);
            var mother = tree.AddPerson(TestBacking.GetCreateFemalePerson());
            cleanup.Add(mother);
            var son = tree.AddPerson(TestBacking.GetCreateMalePerson());
            cleanup.Add(son);
            var rel1 = father.AddSpouse(mother).AddFact(TestBacking.GetMarriageFact());
            cleanup.Add(rel1);
            var rel2 = tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(father, mother, son));
            cleanup.Add(rel2);
            var state = father.ReadDescendancy(FamilySearchOptions.SpouseId(mother.Headers.Get("X-ENTITY-ID").Single().Value.ToString()));

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(state.Tree, Is.Not.Null);
            Assert.That(state.Tree.Root, Is.Not.Null);
            Assert.That(state.Tree.Root.Person, Is.Not.Null);
            Assert.That(state.Tree.Root.Spouse, Is.Not.Null);
            Assert.That(state.Tree.Root.Children, Is.Not.Null);
            Assert.That(state.Tree.Root.Children, Has.Count.EqualTo(1));
            Assert.That(state.Tree.Root.Children[0].Person, Is.Not.Null);
            Assert.That(state.Tree.Root.Person.Id, Is.EqualTo(father.Person.Id));
            Assert.That(state.Tree.Root.Spouse.Id, Is.EqualTo(mother.Headers.Get("X-ENTITY-ID").Single().Value.ToString()));
            Assert.That(state.Tree.Root.Children[0].Person.Id, Is.EqualTo(son.Headers.Get("X-ENTITY-ID").Single().Value.ToString()));
        }

        [Test]
        public void TestReadPersonDescendancyWithSpecifiedSpouseAndAdditionalPersonAndMarriageDetails()
        {
            var father = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(father);
            var mother = tree.AddPerson(TestBacking.GetCreateFemalePerson());
            cleanup.Add(mother);
            var son = tree.AddPerson(TestBacking.GetCreateMalePerson());
            cleanup.Add(son);
            var rel1 = father.AddSpouse(mother).AddFact(TestBacking.GetMarriageFact());
            cleanup.Add(rel1);
            var rel2 = tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(father, mother, son));
            cleanup.Add(rel2);
            var state = father.ReadDescendancy(FamilySearchOptions.SpouseId(mother.Headers.Get("X-ENTITY-ID").Single().Value.ToString()), FamilySearchOptions.IncludePersonDetails(), FamilySearchOptions.IncludeMarriageDetails());

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(state.Tree, Is.Not.Null);
            Assert.That(state.Tree.Root, Is.Not.Null);
            Assert.That(state.Tree.Root.Person, Is.Not.Null);
            Assert.That(state.Tree.Root.Spouse, Is.Not.Null);
            Assert.That(state.Tree.Root.Children, Is.Not.Null);
            Assert.That(state.Tree.Root.Spouse.DisplayExtension, Is.Not.Null);
            Assert.That(state.Tree.Root.Spouse.DisplayExtension.MarriageDate, Is.Not.Null);
            Assert.That(state.Tree.Root.Person.Facts, Is.Not.Null);
            Assert.That(state.Tree.Root.Spouse.Facts, Is.Not.Null);
            Assert.That(state.Tree.Root.Children, Has.Count.EqualTo(1));
            Assert.That(state.Tree.Root.Children[0].Person, Is.Not.Null);
            Assert.That(state.Tree.Root.Person.Id, Is.EqualTo(father.Person.Id));
            Assert.That(state.Tree.Root.Spouse.Id, Is.EqualTo(mother.Headers.Get("X-ENTITY-ID").Single().Value.ToString()));
            Assert.That(state.Tree.Root.Children[0].Person.Id, Is.EqualTo(son.Headers.Get("X-ENTITY-ID").Single().Value.ToString()));
        }
    }
}
