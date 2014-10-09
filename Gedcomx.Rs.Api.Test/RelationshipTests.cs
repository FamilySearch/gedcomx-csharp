using FamilySearch.Api.Ft;
using Gx.Rs.Api;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gedcomx.Rs.Api.Test
{
    [TestFixture]
    public class RelationshipTests
    {
        private static readonly String SANDBOX_URI = "https://sandbox.familysearch.org/platform/collections/tree";
        private CollectionState collection;
        private FamilySearchFamilyTree tree;

        [TestFixtureSetUp]
        public void Initialize()
        {
            collection = new CollectionState(new Uri(SANDBOX_URI));
            collection.AuthenticateViaOAuth2Password("sdktester", "1234sdkpass", "WCQY-7J1Q-GKVV-7DNM-SQ5M-9Q5H-JX3H-CMJK");
            tree = new FamilySearchFamilyTree(true);
            tree.AuthenticateWithAccessToken(collection.CurrentAccessToken);
            Assert.DoesNotThrow(() => collection.IfSuccessful());
            Assert.IsNotNullOrEmpty(collection.CurrentAccessToken);
        }

        [Test]
        public void TestCreateChildAndParentsRelationship()
        {
            var father = collection.AddPerson(TestBacking.GetCreateMalePerson());
            var son = collection.AddPerson(TestBacking.GetCreateMalePerson());
            var state = tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(father, null, son));

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(System.Net.HttpStatusCode.Created, state.Response.StatusCode);
        }

        [Test]
        public void TestCreateCoupleRelationship()
        {
            var husband = (PersonState)collection.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            var wife = (PersonState)collection.AddPerson(TestBacking.GetCreateFemalePerson()).Get();

            var state = husband.AddSpouse(wife);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.IsTrue(state.Response.StatusCode == System.Net.HttpStatusCode.Created);
        }
    }
}
