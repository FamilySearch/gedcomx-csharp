using FamilySearch.Api;
using FamilySearch.Api.Ft;
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
        private FamilySearchFamilyTree tree;

        [TestFixtureSetUp]
        public void Initialize()
        {
            tree = new FamilySearchFamilyTree(true);
            tree.AuthenticateViaOAuth2Password("sdktester", "1234sdkpass", "WCQY-7J1Q-GKVV-7DNM-SQ5M-9Q5H-JX3H-CMJK");
        }

        [Test]
        [Ignore("Need rel link info.")]
        public void TestReadDate()
        {
            var state = (FamilySearchCollectionState)tree.ReadCollection();
            var result = state.NormalizeDate("1 Jul 1801");

            Assert.IsNotNull(result);
            Assert.AreEqual("July 1, 1801", result.Formal);
        }
    }
}
