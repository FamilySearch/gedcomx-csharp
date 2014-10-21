using FamilySearch.Api.Ft;
using Gx.Conclusion;
using Gx.Rs.Api.Options;
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
    public class ChangeHistoryTests
    {
        private FamilySearchFamilyTree tree;

        [TestFixtureSetUp]
        public void Initialize()
        {
            tree = new FamilySearchFamilyTree(true);
            tree.AuthenticateViaOAuth2Password(Resources.TestUserName, Resources.TestPassword, Resources.TestClientId);
        }

        [Test]
        public void TestReadCoupleRelationshipChangeHistory()
        {
            var husband = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            var wife = tree.AddPerson(TestBacking.GetCreateFemalePerson());
            var relationship = (FamilyTreeRelationshipState)husband.AddSpouse(wife).Get();
            relationship.AddFact(TestBacking.GetMarriageFact());
            var state = relationship.ReadChangeHistory();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
            Assert.IsNotNull(state.Page);
            Assert.IsNotNull(state.Page.Entries);
            Assert.Greater(state.Page.Entries.Count, 0);
        }

        [Test]
        public void TestRestoreChangeAction()
        {
            var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            person.DeleteFact(person.Person.Facts.First());
            var changes = person.ReadChangeHistory();
            var deleted = changes.Page.Entries.First(x => x.Operation != null && x.Operation.Value == Gx.Fs.Tree.ChangeOperation.Delete);
            var restore = changes.Page.Entries.First(x => x.ObjectType != null && x.ObjectType == deleted.ObjectType && x.ObjectModifier != null & x.ObjectModifier == deleted.ObjectModifier && x.Operation != null & x.Operation.Value != Gx.Fs.Tree.ChangeOperation.Delete);
            var state = changes.RestoreChange(restore.Entry);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.NoContent, state.Response.StatusCode);
        }

        [Test]
        public void TestReadPersonChangeHistory()
        {
            var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            var state = person.ReadChangeHistory();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
            Assert.IsNotNull(state.Page);
            Assert.IsNotNull(state.Page.Entries);
            Assert.Greater(state.Page.Entries.Count, 0);
        }

        [Test]
        public void TestReadPersonChangeHistoryFirstPage()
        {
            var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            var state = person.ReadChangeHistory(QueryParameter.Count(10));

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
            Assert.IsNotNull(state.Page);
            Assert.IsNotNull(state.Page.Entries);
            Assert.Greater(state.Page.Entries.Count, 0);
        }

        [Test]
        public void TestReadChildAndParentsRelationshipChangeHistory()
        {
            var father = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            var mother = tree.AddPerson(TestBacking.GetCreateFemalePerson());
            var son = tree.AddPerson(TestBacking.GetCreateMalePerson());
            var relationship = (ChildAndParentsRelationshipState)tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(father, mother, son)).Get();
            var state = relationship.ReadChangeHistory();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
            Assert.IsNotNull(state.Entity);
            Assert.IsNotNull(state.Entity.Entries);
            Assert.Greater(state.Entity.Entries.Count, 0);

            father.Delete();
            mother.Delete();
            son.Delete();
        }
    }
}
