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
            collection.AuthenticateViaOAuth2Password("sdktester", "1234sdkpass", "WCQY-7J1Q-GKVV-7DNM-SQ5M-9Q5H-JX3H-CMJK");
            tree = new FamilySearchFamilyTree(true);
            tree.AuthenticateWithAccessToken(collection.CurrentAccessToken);
        }

        [Test]
        public void TestReadAllSourcesOfAllUserDefinedCollectionsOfASpecificUser()
        {
        }
    }
}
