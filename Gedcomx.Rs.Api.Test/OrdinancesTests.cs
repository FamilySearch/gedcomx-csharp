using System;
using System.Net;

using Gx.Rs.Api;
using Gx.Rs.Api.Util;

using NUnit.Framework;

using RestSharp;

namespace Gedcomx.Rs.Api.Test
{
    [TestFixture]
    public class OrdinancesTests
    {
        private readonly Uri ordinanceUri = new Uri("https://api-integ.familysearch.org/platform/ordinances/ordinances");

        [Test]
        public void TestReadOrdinancePolicy()
        {
            var request = new RedirectableRestRequest("/platform/ordinances/policy", Method.GET).Accept("text/html");
            var client = new FilterableRestClient("https://api-integ.familysearch.org");
            var response = client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Content, Is.Not.Null);
            Assert.That(response.Content, Is.Not.Empty);
        }

        [Test]
        public void TestReadOrdinancePolicyInFrench()
        {
            var request = new RedirectableRestRequest("/platform/ordinances/policy", Method.GET).Accept("text/html").AcceptLanguage("fr");
            var client = new FilterableRestClient("https://api-integ.familysearch.org");
            var response = client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Content, Is.Not.Null);
            Assert.That(response.Content, Is.Not.Empty);
        }

        [Test, Category("AccountNeeded")]
        public void TestReadOrdinances()
        {
            var collection = new CollectionState(ordinanceUri);
            var state = collection.AuthenticateViaOAuth2Password(Resources.TestUserName, Resources.TestPassword, Resources.TestClientId).Get();

            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test, Category("AccountNeeded")]
        public void TestReadOrdinancesAccessForbidden()
        {
            var collection = new CollectionState(ordinanceUri);
            var state = collection.AuthenticateViaOAuth2Password(Resources.PublicUserName, Resources.PublicPassword, Resources.TestClientId).Get();

            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }
    }
}
