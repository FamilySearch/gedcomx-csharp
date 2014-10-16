using FamilySearch.Api.Ft;
using Gx.Rs.Api;
using Gx.Source;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gedcomx.Rs.Api.Test
{
    [TestFixture]
    public class SourceBoxTests
    {
        private CollectionState collection;
        private FamilySearchFamilyTree tree;

        [TestFixtureSetUp]
        public void Initialize()
        {
            collection = new CollectionState(new Uri("https://sandbox.familysearch.org/platform/collections/sources"));
            collection.AuthenticateViaOAuth2Password(Resources.TestUserName, Resources.TestPassword, Resources.TestClientId);
            tree = new FamilySearchFamilyTree(true);
            tree.AuthenticateWithAccessToken(collection.CurrentAccessToken);
        }

        [Test]
        public void TestReadAllSourcesOfAllUserDefinedCollectionsOfASpecificUser()
        {
        }
    }
}
