using System;
using System.Collections.Generic;

using FamilySearch.Api.Ft;

using Gx.Rs.Api;

using NUnit.Framework;

namespace Gedcomx.Rs.Api.Test
{
    [TestFixture]
    public class RelationshipTests
    {
        private static readonly String INTEGRATION_URI = "https://api-integ.familysearch.org/platform/collections/tree";
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
            Assert.That(collection.CurrentAccessToken, Is.Not.Null);
            Assert.That(collection.CurrentAccessToken, Is.Not.Empty);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            foreach (var state in cleanup)
            {
                state.Delete();
            }
        }

        [Test, Category("AccountNeeded")]
        public void TestCreateChildAndParentsRelationship()
        {
            var father = collection.AddPerson(TestBacking.GetCreateMalePerson());
            cleanup.Add(father);
            var son = collection.AddPerson(TestBacking.GetCreateMalePerson());
            cleanup.Add(son);
            var state = tree.AddChildAndParentsRelationship(TestBacking.GetCreateChildAndParentsRelationship(father, null, son));
            cleanup.Add(state);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Created));
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
            Assert.That(state.Response.StatusCode == System.Net.HttpStatusCode.Created, Is.True);
        }
    }
}
