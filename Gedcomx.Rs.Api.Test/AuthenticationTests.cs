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

namespace Gedcomx.Rs.Api.Test
{
    [TestFixture]
    public class AuthenticationTests
    {
        private static readonly String SANDBOX_URI = "https://sandbox.familysearch.org/platform/collections/tree";

        [Test]
        public void TestDeleteAccessToken()
        {
            var collection = new CollectionState(new Uri(SANDBOX_URI));
            collection.AuthenticateViaOAuth2Password("sdktester", "1234sdkpass", "WCQY-7J1Q-GKVV-7DNM-SQ5M-9Q5H-JX3H-CMJK");
            Assert.IsTrue(collection.IsAuthenticated);
            Link link = collection.GetLink(Rel.OAUTH2_TOKEN);
            IRestRequest request = new RestRequest()
                .Accept(MediaTypes.APPLICATION_JSON_TYPE)
                .Build(link.Href + "?access_token=" + collection.CurrentAccessToken, Method.DELETE);
            IRestResponse response = collection.Client.Execute(request);
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Test]
        public void TestObtainAccessTokenBadParameters()
        {
            var collection = new CollectionState(new Uri(SANDBOX_URI));
            Link tokenLink = collection.GetLink(Rel.OAUTH2_TOKEN);
            IDictionary<String, String> formData = new Dictionary<String, String>();
            formData.Add("grant_type", "authorization_code");
            formData.Add("code", "tGzv3JOkF0XG5Qx2TlKWIA");
            formData.Add("client_id", "WCQY-7J1Q-GKVV-7DNM-SQ5M-9Q5H-JX3H-CMJK");
            IRestRequest request = new RestRequest()
                .Accept(MediaTypes.APPLICATION_JSON_TYPE)
                .ContentType(MediaTypes.APPLICATION_FORM_URLENCODED_TYPE)
                .SetEntity(formData)
                .Build(tokenLink.Href, Method.POST);
            IRestResponse response = collection.Client.Execute(request);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        [Ignore("Unable to test without ability to capture redirect URL POST parameters.")]
        public void TestObtainAccessTokenwithAuthorizationCode()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void TestObtainAccessTokenWithUsernameAndPassword()
        {
            var collection = new CollectionState(new Uri(SANDBOX_URI));
            var state = collection.AuthenticateViaOAuth2Password("sdktester", "1234sdkpass", "WCQY-7J1Q-GKVV-7DNM-SQ5M-9Q5H-JX3H-CMJK");
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
            Assert.IsNotNullOrEmpty(state.CurrentAccessToken);
        }

        [Test]
        public void TestObtainAccessTokenWithoutAuthenticating()
        {
            var collection = new CollectionState(new Uri(SANDBOX_URI));
            var ip = new WebClient().DownloadString("http://ipecho.net/plain");
            var state = collection.UnauthenticatedAccess(ip, "WCQY-7J1Q-GKVV-7DNM-SQ5M-9Q5H-JX3H-CMJK");
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
            Assert.IsNotNullOrEmpty(state.CurrentAccessToken);
        }
    }
}
