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

        [TestFixtureSetUp]
        public void Initialize()
        {
            collection = new CollectionState(new Uri(SANDBOX_URI));
            collection.AuthenticateViaOAuth2Password("sdktester", "1234sdkpass", "WCQY-7J1Q-GKVV-7DNM-SQ5M-9Q5H-JX3H-CMJK");
            Assert.DoesNotThrow(() => collection.IfSuccessful());
            Assert.IsNotNullOrEmpty(collection.CurrentAccessToken);
        }

        [Test]
        [Ignore("Fails in Java SDK and here. Thinks the relationship is a couple.")]
        public void TestCreateChildAndParentsRelationship()
        {
            var father = (PersonState)collection.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            var son = (PersonState)collection.AddPerson(TestBacking.GetCreateMalePerson()).Get();

            var state = father.AddChild(son);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.IsTrue(state.Response.StatusCode == System.Net.HttpStatusCode.Created);
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
