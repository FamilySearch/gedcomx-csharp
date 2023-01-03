using System;
using System.Linq;

using FamilySearch.Api;

using Gx.Conclusion;

using NUnit.Framework;

namespace Gedcomx.Rs.Api.Test
{
    [TestFixture, Category("AccountNeeded")]
    public class AuthoritiesTests
    {
        private FamilySearchCollectionState date;

        [OneTimeSetUp]
        public void Initialize()
        {
            date = new FamilySearchCollectionState(new Uri("https://api-integ.familysearch.org/platform/collections/dates"));
            date.AuthenticateViaOAuth2Password(Resources.TestUserName, Resources.TestPassword, Resources.TestClientId);
        }

        [Test]
        public void TestReadDate()
        {
            DateInfo state = date.NormalizeDate("17 Jul 1968");
            Assert.That(state.Formal, Is.EqualTo("gedcomx-date:+1968-07-17"));
            Assert.That(state.NormalizedExtensions.First().Value, Is.EqualTo("17 July 1968"));
        }
    }
}
