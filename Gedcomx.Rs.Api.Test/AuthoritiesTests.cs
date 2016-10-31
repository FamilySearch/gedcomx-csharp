using FamilySearch.Api;
using FamilySearch.Api.Ft;
using Gx.Conclusion;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gedcomx.Rs.Api.Test
{
    [TestFixture]
    public class AuthoritiesTests
    {
        private FamilySearchCollectionState date;

        [OneTimeSetUp]
        public void Initialize()
        {
            date = new FamilySearchCollectionState(new Uri("https://integration.familysearch.org/platform/collections/dates"));
            date.AuthenticateViaOAuth2Password(Resources.TestUserName, Resources.TestPassword, Resources.TestClientId);
        }

        [Test]
        public void TestReadDate()
        {
            DateInfo state = date.NormalizeDate("17 Jul 1968");
            Assert.AreEqual("gedcomx-date:+1968-07-17", state.Formal);
            Assert.AreEqual("17 July 1968", state.NormalizedExtensions.First().Value);
        }
    }
}
