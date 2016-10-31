using Gx.Rs.Api;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gx.Rs.Api.Util;
using NUnit.Framework;
using Gx.Links;
using System.Net;
using CsQuery;
using Gedcomx.Support;

namespace Gedcomx.Rs.Api.Test
{
    [TestFixture]
    public class AuthenticationTests
    {
        private static readonly String INTEGRATION_URI = "https://integration.familysearch.org/platform/collections/tree";

        [Test]
        public void TestDeleteAccessToken()
        {
            var collection = new CollectionState(new Uri(INTEGRATION_URI));
            collection.AuthenticateViaOAuth2Password(Resources.TestUserName, Resources.TestPassword, Resources.TestClientId);
            Assert.IsTrue(collection.IsAuthenticated);
            Link link = collection.GetLink(Rel.OAUTH2_TOKEN);
            IRestRequest request = new RedirectableRestRequest()
                .Accept(MediaTypes.APPLICATION_JSON_TYPE)
                .Build(link.Href + "?access_token=" + collection.CurrentAccessToken, Method.DELETE);
            IRestResponse response = collection.Client.Handle(request);
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Test]
        public void TestObtainAccessTokenBadParameters()
        {
            var collection = new CollectionState(new Uri(INTEGRATION_URI));
            Link tokenLink = collection.GetLink(Rel.OAUTH2_TOKEN);
            IDictionary<String, String> formData = new Dictionary<String, String>();
            formData.Add("grant_type", "authorization_code");
            formData.Add("code", "tGzv3JOkF0XG5Qx2TlKWIA");
            formData.Add("client_id", "WCQY-7J1Q-GKVV-7DNM-SQ5M-9Q5H-JX3H-CMJK");
            IRestRequest request = new RedirectableRestRequest()
                .Accept(MediaTypes.APPLICATION_JSON_TYPE)
                .ContentType(MediaTypes.APPLICATION_FORM_URLENCODED_TYPE)
                .SetEntity(formData)
                .Build(tokenLink.Href, Method.POST);
            IRestResponse response = collection.Client.Handle(request);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public void TestObtainAccessTokenWithUsernameAndPassword()
        {
            var collection = new CollectionState(new Uri(INTEGRATION_URI));
            var state = collection.AuthenticateViaOAuth2Password(Resources.TestUserName, Resources.TestPassword, Resources.TestClientId);
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
            Assert.IsNotNull(state.CurrentAccessToken);
			Assert.IsNotEmpty(state.CurrentAccessToken);
		}

        [Test]
        public void TestObtainAccessTokenWithoutAuthenticating()
        {
            var collection = new CollectionState(new Uri(INTEGRATION_URI));
            var response = new WebClient().DownloadString("http://checkip.dyndns.com/");
            var ip = new CQ(response).Select("body").Text().Split(new string[] { ": " }, StringSplitOptions.RemoveEmptyEntries)[1].Trim();
            var state = collection.UnauthenticatedAccess(ip, "WCQY-7J1Q-GKVV-7DNM-SQ5M-9Q5H-JX3H-CMJK");
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
            Assert.IsNotNull(state.CurrentAccessToken);
			Assert.IsNotEmpty(state.CurrentAccessToken);
		}

        [Test]
        public void TestInitiateAuthorizationGet()
        {
            var collection = new CollectionState(new Uri(INTEGRATION_URI));
            Link tokenLink = collection.GetLink(Rel.OAUTH2_AUTHORIZE);
            IDictionary<String, String> formData = new Dictionary<String, String>();
            formData.Add("response_type", "code");
            formData.Add("client_id", "ABCD-EFGH-JKLM-NOPQ-RSTU-VWXY-0123-4567");
            formData.Add("redirect_uri", "https://familysearch.org/developers/integration-oauth2-redirect");
            IRestRequest request = new RedirectableRestRequest()
                .SetEntity(formData)
                .Build(tokenLink.Href, Method.GET);
            IRestResponse response = collection.Client.Handle(request);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var cq = new CQ(response.Content);
            Assert.AreEqual(1, cq.Select("input#userName").Length);
            Assert.AreEqual(1, cq.Select("input#password").Length);
        }

        [Test]
        public void TestInitiateAuthorizationInvalidParameter()
        {
            var collection = new CollectionState(new Uri(INTEGRATION_URI));
            Link tokenLink = collection.GetLink(Rel.OAUTH2_AUTHORIZE);
            IDictionary<String, String> formData = new Dictionary<String, String>();
            formData.Add("response_type", "authorize_me");
            formData.Add("client_id", "ABCD-EFGH-JKLM-NOPQ-RSTU-VWXY-0123-4567");
            formData.Add("redirect_uri", "https://familysearch.org/developers/integration-oauth2-redirect");
            IRestRequest request = new RedirectableRestRequest()
                .ContentType(MediaTypes.APPLICATION_FORM_URLENCODED_TYPE)
                .SetEntity(formData)
                .Build(tokenLink.Href, Method.POST);
            IRestResponse response = collection.Client.Handle(request);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
			//var cq = new CQ(response.Content);
			//Assert.AreEqual(1, cq.Select("p#error").Length);
			Assert.True(response.Content.Contains("Error")); // An error should occur.
        }

        [Test]
        public void TestInitiateAuthorizationPost()
        {
            var collection = new CollectionState(new Uri(INTEGRATION_URI));
            Link tokenLink = collection.GetLink(Rel.OAUTH2_AUTHORIZE);
            IDictionary<String, String> formData = new Dictionary<String, String>();
            formData.Add("response_type", "code");
            formData.Add("client_id", "ABCD-EFGH-JKLM-NOPQ-RSTU-VWXY-0123-4567");
            formData.Add("redirect_uri", "https://familysearch.org/developers/integration-oauth2-redirect");
            IRestRequest request = new RedirectableRestRequest()
                .SetEntity(formData)
                .Build(tokenLink.Href, Method.POST);
            IRestResponse response = collection.Client.Handle(request);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var cq = new CQ(response.Content);
            Assert.AreEqual(1, cq.Select("input#userName").Length);
            Assert.AreEqual(1, cq.Select("input#password").Length);
        }
    }
}
