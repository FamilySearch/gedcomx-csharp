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
        private static readonly String INTEGRATION_URI = "https://integration.familysearch.org/platform/collections/tree";
        private CollectionState collection;
        private FamilySearchFamilyTree tree;
        private List<GedcomxApplicationState> cleanup;

        [OneTimeSetUp]
        public void Initialize()
        {
            collection = new CollectionState(new Uri(INTEGRATION_URI));
            collection.AuthenticateViaOAuth2Password(Resources.TestUserName, Resources.TestPassword, Resources.TestClientId);
            tree = new FamilySearchFamilyTree(true);
            tree.AuthenticateWithAccessToken(collection.CurrentAccessToken);
            cleanup = new List<GedcomxApplicationState>();
            Assert.DoesNotThrow(() => collection.IfSuccessful());
            Assert.IsNotNull(collection.CurrentAccessToken);
			Assert.IsNotEmpty(collection.CurrentAccessToken);
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
            var father = collection.AddPerson(TestBacking.GetCreateMalePerson());
            cleanup.Add(father);
            var son = collection.AddPerson(TestBacking.GetCreateMalePerson());
            cleanup.Add(son);
            var state = tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(father, null, son));
            cleanup.Add(state);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(System.Net.HttpStatusCode.Created, state.Response.StatusCode);
        }

        [Test]
        public void TestCreateCoupleRelationship()
        {
            var husband = (PersonState)collection.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(husband);
            var wife = (PersonState)collection.AddPerson(TestBacking.GetCreateFemalePerson()).Get();
            cleanup.Add(wife);
            var state = husband.AddSpouse(wife);
            cleanup.Add(state);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.IsTrue(state.Response.StatusCode == System.Net.HttpStatusCode.Created);
        }
    }
}
