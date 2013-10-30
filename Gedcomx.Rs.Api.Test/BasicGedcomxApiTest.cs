using System;
using NUnit.Framework;
using Gx.Rs.Api;
using System.Net;
using RestSharp;
using Gx.Conclusion;

namespace Gedcomx.Rs.Api.Test
{
	[TestFixture]
	public class BasicGedcomxApiTest
	{

		private BasicGedcomxApi api;

		[SetUp]
		public void Setup()
		{
			//make sure we accept any ssl certificates.
			ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
			api = new BasicGedcomxApi("https://sandbox.familysearch.org", ".well-known/app-meta");
		}

		[Test]
		public void TestBuildOAuth2AuthorizationUri ()
		{
			Assert.AreEqual("https://sandbox.familysearch.org/cis-web/oauth2/v3/authorization?response_type=code&client_id=12345&redirect_uri=http://mydomain.com/authorized", 
			                api.BuildOAuth2AuthorizationUri("12345", "http://mydomain.com/authorized").ToString());
		}

		[Test]
		public void TestTryOAuth2Authentication ()
		{
			Assert.IsTrue(api.TryOAuth2Authentication("heatonra", "1234cispass", "WCQY-7J1Q-GKVV-7DNM-SQ5M-9Q5H-JX3H-CMJK"));
		}

		[Test]
		//[Ignore("Ignore until the OAuth2 authentication is fixed so we can get an access token")]
		public void TestGetPerson()
		{
			api.CurrentAccessToken = "USYS90671F93B36F377FF4822E86DF7652E2_idses-int02.a.fsglobal.net";
			Person person = api.GetPerson("KW3B-FVV").Resource;
			Assert.IsTrue(person.Names.Count > 0);
		}

	}
}

