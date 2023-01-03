using System;
using System.Collections.Generic;
using System.Net;

using CsQuery;

using Gedcomx.Support;

using Gx.Links;
using Gx.Rs.Api;
using Gx.Rs.Api.Util;

using NUnit.Framework;

using RestSharp;

namespace Gedcomx.Rs.Api.Test
{
    [TestFixture]
    public class AuthenticationTests
    {
        private static readonly String INTEGRATION_URI = "https://api-integ.familysearch.org/platform/collections/tree";

        [Test, Category("AccountNeeded")]
        public void TestDeleteAccessToken()
        {
            var collection = new CollectionState(new Uri(INTEGRATION_URI));
            collection.AuthenticateViaOAuth2Password(Resources.TestUserName, Resources.TestPassword, Resources.TestClientId);
            Assert.That(collection.IsAuthenticated, Is.True);
            Link link = collection.GetLink(Rel.OAUTH2_TOKEN);
            IRestRequest request = new RedirectableRestRequest()
                .Accept(MediaTypes.APPLICATION_JSON_TYPE)
                .Build(link.Href + "?access_token=" + collection.CurrentAccessToken, Method.DELETE);
            IRestResponse response = collection.Client.Handle(request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test]
        public void TestObtainAccessTokenBadParameters()
        {
            var collection = new CollectionState(new Uri(INTEGRATION_URI));
            Link tokenLink = collection.GetLink(Rel.OAUTH2_TOKEN);
            IDictionary<String, String> formData = new Dictionary<String, String>
            {
                { "grant_type", "authorization_code" },
                { "code", "tGzv3JOkF0XG5Qx2TlKWIA" },
                { "client_id", "WCQY-7J1Q-GKVV-7DNM-SQ5M-9Q5H-JX3H-CMJK" }
            };
            IRestRequest request = new RedirectableRestRequest()
                .Accept(MediaTypes.APPLICATION_JSON_TYPE)
                .ContentType(MediaTypes.APPLICATION_FORM_URLENCODED_TYPE)
                .SetEntity(formData)
                .Build(tokenLink.Href, Method.POST);
            IRestResponse response = collection.Client.Handle(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public void TestObtainAccessTokenWithUsernameAndPassword()
        {
            var collection = new CollectionState(new Uri(INTEGRATION_URI));
            var state = collection.AuthenticateViaOAuth2Password(Resources.TestUserName, Resources.TestPassword, Resources.TestClientId);
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(state.CurrentAccessToken, Is.Not.Null);
            Assert.That(state.CurrentAccessToken, Is.Not.Empty);
        }

        [Test]
        public void TestObtainAccessTokenWithoutAuthenticating()
        {
            var collection = new CollectionState(new Uri(INTEGRATION_URI));
            var response = new WebClient().DownloadString("http://checkip.dyndns.com/");
            var ip = new CQ(response).Select("body").Text().Split(new string[] { ": " }, StringSplitOptions.RemoveEmptyEntries)[1].Trim();
            var state = collection.UnauthenticatedAccess(ip, "WCQY-7J1Q-GKVV-7DNM-SQ5M-9Q5H-JX3H-CMJK");
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(state.CurrentAccessToken, Is.Not.Null);
            Assert.That(state.CurrentAccessToken, Is.Not.Empty);
        }

        [Test, Category("AccountNeeded")]
        public void TestInitiateAuthorizationGet()
        {
            var collection = new CollectionState(new Uri(INTEGRATION_URI));
            Link tokenLink = collection.GetLink(Rel.OAUTH2_AUTHORIZE);
            IDictionary<String, String> formData = new Dictionary<String, String>
            {
                { "response_type", "code" },
                { "client_id", "ABCD-EFGH-JKLM-NOPQ-RSTU-VWXY-0123-4567" },
                { "redirect_uri", "https://www.familysearch.org/developers/integration-oauth2-redirect" }
            };
            IRestRequest request = new RedirectableRestRequest()
                .SetEntity(formData)
                .Build(tokenLink.Href, Method.GET);
            IRestResponse response = collection.Client.Handle(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var cq = new CQ(response.Content);
            Assert.That(cq.Select("input#userName").Length, Is.EqualTo(1));
            Assert.That(cq.Select("input#password").Length, Is.EqualTo(1));
        }

        [Test]
        public void TestInitiateAuthorizationInvalidParameter()
        {
            var collection = new CollectionState(new Uri(INTEGRATION_URI));
            Link tokenLink = collection.GetLink(Rel.OAUTH2_AUTHORIZE);
            IDictionary<String, String> formData = new Dictionary<String, String>
            {
                { "response_type", "authorize_me" },
                { "client_id", "ABCD-EFGH-JKLM-NOPQ-RSTU-VWXY-0123-4567" },
                { "redirect_uri", "https://www.familysearch.org/developers/integration-oauth2-redirect" }
            };
            IRestRequest request = new RedirectableRestRequest()
                .ContentType(MediaTypes.APPLICATION_FORM_URLENCODED_TYPE)
                .SetEntity(formData)
                .Build(tokenLink.Href, Method.POST);
            IRestResponse response = collection.Client.Handle(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            //var cq = new CQ(response.Content);
            //Assert.AreEqual(1, cq.Select("p#error").Length);
            Assert.True(response.Content.Contains("Error")); // An error should occur.
        }

        [Test, Category("AccountNeeded")]
        public void TestInitiateAuthorizationPost()
        {
            var collection = new CollectionState(new Uri(INTEGRATION_URI));
            Link tokenLink = collection.GetLink(Rel.OAUTH2_AUTHORIZE);
            IDictionary<String, String> formData = new Dictionary<String, String>
            {
                { "response_type", "code" },
                { "client_id", "ABCD-EFGH-JKLM-NOPQ-RSTU-VWXY-0123-4567" },
                { "redirect_uri", "https://www.familysearch.org/developers/integration-oauth2-redirect" }
            };
            IRestRequest request = new RedirectableRestRequest()
                .SetEntity(formData)
                .Build(tokenLink.Href, Method.POST);
            IRestResponse response = collection.Client.Handle(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var cq = new CQ(response.Content);
            Assert.That(cq.Select("input#userName").Length, Is.EqualTo(1));
            Assert.That(cq.Select("input#password").Length, Is.EqualTo(1));
        }
    }
}
