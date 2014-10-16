using FamilySearch.Api.Ft;
using FamilySearch.Api.Util;
using Gx.Rs.Api.Options;
using Gx.Rs.Api.Util;
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
    public class PedigreeTests
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
        public void TestReadPersonAncestry()
        {
            var grandfather = tree.AddPerson(TestBacking.GetCreateMalePerson());
            var father = tree.AddPerson(TestBacking.GetCreateMalePerson());
            var son = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(grandfather, null, father));
            tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(father, null, son));
            son = tree.ReadPersonById(son.Person.Id);
            var state = son.ReadAncestry();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
            Assert.IsNotNull(state.Tree);
            Assert.IsNotNull(state.Tree.Root);
            Assert.IsNotNull(state.Tree.Root.Person);
            Assert.IsNotNull(state.Tree.Root.Father);
            Assert.IsNotNull(state.Tree.Root.Father.Person);
            Assert.IsNotNull(state.Tree.Root.Father.Father);
            Assert.IsNotNull(state.Tree.Root.Father.Father.Person);
            Assert.AreEqual(grandfather.GetSelfUri(), state.Tree.Root.Father.Father.Person.GetLink("self").Href);
            Assert.AreEqual(father.GetSelfUri(), state.Tree.Root.Father.Person.GetLink("self").Href);
            Assert.AreEqual(son.GetSelfUri(), state.Tree.Root.Person.GetLink("self").Href);
        }

        [Test]
        public void TestReadPersonAncestryAndAdditionalPersonDetails()
        {
            var grandfather = tree.AddPerson(TestBacking.GetCreateMalePerson());
            var father = tree.AddPerson(TestBacking.GetCreateMalePerson());
            var son = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(grandfather, null, father));
            tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(father, null, son));
            son = tree.ReadPersonById(son.Person.Id);
            var state = son.ReadAncestry(FamilySearchOptions.IncludePersonDetails());

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
            Assert.IsNotNull(state.Tree);
            Assert.IsNotNull(state.Tree.Root);
            Assert.IsNotNull(state.Tree.Root.Person);
            Assert.IsNotNull(state.Tree.Root.Father);
            Assert.IsNotNull(state.Tree.Root.Father.Person);
            Assert.IsNotNull(state.Tree.Root.Father.Father);
            Assert.IsNotNull(state.Tree.Root.Father.Father.Person);
            Assert.IsNotNull(state.Tree.Root.Person.Facts);
            Assert.IsNotNull(state.Tree.Root.Father.Person.Facts);
            Assert.IsNotNull(state.Tree.Root.Father.Father.Person.Facts);
            Assert.AreEqual(grandfather.GetSelfUri(), state.Tree.Root.Father.Father.Person.GetLink("self").Href);
            Assert.AreEqual(father.GetSelfUri(), state.Tree.Root.Father.Person.GetLink("self").Href);
            Assert.AreEqual(son.GetSelfUri(), state.Tree.Root.Person.GetLink("self").Href);
        }

        [Test]
        public void TestReadPersonAncestryWithSpecifiedSpouse()
        {
            var grandfather = tree.AddPerson(TestBacking.GetCreateMalePerson());
            var father = tree.AddPerson(TestBacking.GetCreateMalePerson());
            var husband = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            var wife = tree.AddPerson(TestBacking.GetCreateFemalePerson());
            husband.AddSpouse(wife);
            tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(grandfather, null, father));
            tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(father, null, husband));
            husband = tree.ReadPersonById(husband.Person.Id);
            var state = husband.ReadAncestry(FamilySearchOptions.SpouseId(wife.Headers.Get("X-ENTITY-ID").Single().Value.ToString()));

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
            Assert.IsNotNull(state.Tree);
            Assert.IsNotNull(state.Tree.Root);
            Assert.IsNotNull(state.Tree.Root.Mother);
            Assert.IsNotNull(state.Tree.Root.Mother.Person);
            Assert.IsNotNull(state.Tree.Root.Father);
            Assert.IsNotNull(state.Tree.Root.Father.Person);
            Assert.IsNotNull(state.Tree.Root.Father.Father);
            Assert.IsNotNull(state.Tree.Root.Father.Father.Person);
            Assert.IsNotNull(state.Tree.Root.Father.Father.Father);
            Assert.IsNotNull(state.Tree.Root.Father.Father.Father.Person);
            Assert.AreEqual(grandfather.GetSelfUri(), state.Tree.Root.Father.Father.Father.Person.GetLink("self").Href);
            Assert.AreEqual(father.GetSelfUri(), state.Tree.Root.Father.Father.Person.GetLink("self").Href);
            Assert.AreEqual(husband.GetSelfUri(), state.Tree.Root.Father.Person.GetLink("self").Href);
            Assert.AreEqual(wife.GetSelfUri(), state.Tree.Root.Mother.Person.GetLink("self").Href);
        }

        [Test]
        public void TestReadPersonAncestryWithSpecifiedSpouseAndAdditionalPersonAndMarriageDetails()
        {
            var grandfather = tree.AddPerson(TestBacking.GetCreateMalePerson());
            var father = tree.AddPerson(TestBacking.GetCreateMalePerson());
            var husband = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            var wife = tree.AddPerson(TestBacking.GetCreateFemalePerson());
            husband.AddSpouse(wife).AddFact(TestBacking.GetMarriageFact());
            tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(grandfather, null, father));
            tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(father, null, husband));
            husband = tree.ReadPersonById(husband.Person.Id);
            var state = husband.ReadAncestry(FamilySearchOptions.SpouseId(wife.Headers.Get("X-ENTITY-ID").Single().Value.ToString()), FamilySearchOptions.IncludePersonDetails(), FamilySearchOptions.IncludeMarriageDetails());

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
            Assert.IsNotNull(state.Tree);
            Assert.IsNotNull(state.Tree.Root);
            Assert.IsNotNull(state.Tree.Root.Mother);
            Assert.IsNotNull(state.Tree.Root.Mother.Person);
            Assert.IsNotNull(state.Tree.Root.Mother.Person.DisplayExtension);
            Assert.IsNotNull(state.Tree.Root.Mother.Person.DisplayExtension.MarriageDate);
            Assert.IsNotNull(state.Tree.Root.Father);
            Assert.IsNotNull(state.Tree.Root.Father.Person);
            Assert.IsNotNull(state.Tree.Root.Father.Father);
            Assert.IsNotNull(state.Tree.Root.Father.Father.Person);
            Assert.IsNotNull(state.Tree.Root.Father.Father.Father);
            Assert.IsNotNull(state.Tree.Root.Father.Father.Father.Person);
            Assert.IsNotNull(state.Tree.Root.Father.Person.Facts);
            Assert.IsNotNull(state.Tree.Root.Mother.Person.Facts);
            Assert.AreEqual(grandfather.GetSelfUri(), state.Tree.Root.Father.Father.Father.Person.GetLink("self").Href);
            Assert.AreEqual(father.GetSelfUri(), state.Tree.Root.Father.Father.Person.GetLink("self").Href);
            Assert.AreEqual(husband.GetSelfUri(), state.Tree.Root.Father.Person.GetLink("self").Href);
            Assert.AreEqual(wife.GetSelfUri(), state.Tree.Root.Mother.Person.GetLink("self").Href);
        }

        [Test]
        public void TestReadPersonDescendancy()
        {
            var father = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            var son = tree.AddPerson(TestBacking.GetCreateMalePerson());
            tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(father, null, son));
            var state = father.ReadDescendancy();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
            Assert.IsNotNull(state.Tree);
            Assert.IsNotNull(state.Tree.Root);
            Assert.IsNotNull(state.Tree.Root.Person);
            Assert.IsNotNull(state.Tree.Root.Children);
            Assert.AreEqual(1, state.Tree.Root.Children.Count);
            Assert.IsNotNull(state.Tree.Root.Children[0].Person);
            Assert.AreEqual(father.Person.Id, state.Tree.Root.Person.Id);
            Assert.AreEqual(son.Headers.Get("X-ENTITY-ID").Single().Value.ToString(), state.Tree.Root.Children[0].Person.Id);
        }

        [Test]
        public void TestReadPersonDescendancyAndAdditionalPersonAndMarriageDetails()
        {
            var father = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            var mother = tree.AddPerson(TestBacking.GetCreateFemalePerson());
            var son = tree.AddPerson(TestBacking.GetCreateMalePerson());
            father.AddSpouse(mother).AddFact(TestBacking.GetMarriageFact());
            tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(father, mother, son));
            var state = father.ReadDescendancy(FamilySearchOptions.IncludePersonDetails(), FamilySearchOptions.IncludeMarriageDetails());

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
            Assert.IsNotNull(state.Tree);
            Assert.IsNotNull(state.Tree.Root);
            Assert.IsNotNull(state.Tree.Root.Person);
            Assert.IsNotNull(state.Tree.Root.Spouse);
            Assert.IsNotNull(state.Tree.Root.Children);
            Assert.IsNotNull(state.Tree.Root.Spouse.DisplayExtension);
            Assert.IsNotNull(state.Tree.Root.Spouse.DisplayExtension.MarriageDate);
            Assert.IsNotNull(state.Tree.Root.Person.Facts);
            Assert.IsNotNull(state.Tree.Root.Spouse.Facts);
            Assert.AreEqual(1, state.Tree.Root.Children.Count);
            Assert.IsNotNull(state.Tree.Root.Children[0].Person);
            Assert.AreEqual(father.Person.Id, state.Tree.Root.Person.Id);
            Assert.AreEqual(mother.Headers.Get("X-ENTITY-ID").Single().Value.ToString(), state.Tree.Root.Spouse.Id);
            Assert.AreEqual(son.Headers.Get("X-ENTITY-ID").Single().Value.ToString(), state.Tree.Root.Children[0].Person.Id);
        }

        [Test]
        public void TestReadPersonDescendancyWithSpecifiedSpouse()
        {
            var father = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            var mother = tree.AddPerson(TestBacking.GetCreateFemalePerson());
            var son = tree.AddPerson(TestBacking.GetCreateMalePerson());
            father.AddSpouse(mother).AddFact(TestBacking.GetMarriageFact());
            tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(father, mother, son));
            var state = father.ReadDescendancy(FamilySearchOptions.SpouseId(mother.Headers.Get("X-ENTITY-ID").Single().Value.ToString()));

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
            Assert.IsNotNull(state.Tree);
            Assert.IsNotNull(state.Tree.Root);
            Assert.IsNotNull(state.Tree.Root.Person);
            Assert.IsNotNull(state.Tree.Root.Spouse);
            Assert.IsNotNull(state.Tree.Root.Children);
            Assert.AreEqual(1, state.Tree.Root.Children.Count);
            Assert.IsNotNull(state.Tree.Root.Children[0].Person);
            Assert.AreEqual(father.Person.Id, state.Tree.Root.Person.Id);
            Assert.AreEqual(mother.Headers.Get("X-ENTITY-ID").Single().Value.ToString(), state.Tree.Root.Spouse.Id);
            Assert.AreEqual(son.Headers.Get("X-ENTITY-ID").Single().Value.ToString(), state.Tree.Root.Children[0].Person.Id);
        }

        [Test]
        public void TestReadPersonDescendancyWithSpecifiedSpouseAndAdditionalPersonAndMarriageDetails()
        {
            var father = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            var mother = tree.AddPerson(TestBacking.GetCreateFemalePerson());
            var son = tree.AddPerson(TestBacking.GetCreateMalePerson());
            father.AddSpouse(mother).AddFact(TestBacking.GetMarriageFact());
            tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(father, mother, son));
            var state = father.ReadDescendancy(FamilySearchOptions.SpouseId(mother.Headers.Get("X-ENTITY-ID").Single().Value.ToString()), FamilySearchOptions.IncludePersonDetails(), FamilySearchOptions.IncludeMarriageDetails());

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
            Assert.IsNotNull(state.Tree);
            Assert.IsNotNull(state.Tree.Root);
            Assert.IsNotNull(state.Tree.Root.Person);
            Assert.IsNotNull(state.Tree.Root.Spouse);
            Assert.IsNotNull(state.Tree.Root.Children);
            Assert.IsNotNull(state.Tree.Root.Spouse.DisplayExtension);
            Assert.IsNotNull(state.Tree.Root.Spouse.DisplayExtension.MarriageDate);
            Assert.IsNotNull(state.Tree.Root.Person.Facts);
            Assert.IsNotNull(state.Tree.Root.Spouse.Facts);
            Assert.AreEqual(1, state.Tree.Root.Children.Count);
            Assert.IsNotNull(state.Tree.Root.Children[0].Person);
            Assert.AreEqual(father.Person.Id, state.Tree.Root.Person.Id);
            Assert.AreEqual(mother.Headers.Get("X-ENTITY-ID").Single().Value.ToString(), state.Tree.Root.Spouse.Id);
            Assert.AreEqual(son.Headers.Get("X-ENTITY-ID").Single().Value.ToString(), state.Tree.Root.Children[0].Person.Id);
        }
    }
}
