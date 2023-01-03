using System.Collections.Generic;
using System.Net;

using FamilySearch.Api.Ft;

using Gx.Rs.Api;
using Gx.Rs.Api.Options;

using NUnit.Framework;

namespace Gedcomx.Rs.Api.Test
{
    [TestFixture]
    public class UserTests
    {
        private FamilySearchFamilyTree tree;
        private List<GedcomxApplicationState> cleanup;

        [OneTimeSetUp]
        public void Initialize()
        {
            tree = new FamilySearchFamilyTree(true);
            tree.AuthenticateViaOAuth2Password(Resources.TestUserName, Resources.TestPassword, Resources.TestClientId);
            cleanup = new List<GedcomxApplicationState>();
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
        public void TestReadCurrentTreePerson()
        {
            var state = tree.ReadPersonForCurrentUser();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.SeeOther));
        }

        [Test]
        public void TestReadCurrentTreePersonExpecting200Response()
        {
            var expect = new HeaderParameter("X-Expect-Override", "200-ok");
            var state = tree.ReadPersonForCurrentUser(expect);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void TestReadCurrentUser()
        {
            var state = tree.ReadCurrentUser();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void TestReadUser()
        {
            var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(person);
            var state = person.ReadContributor(person.GetName());

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}
