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
		[Ignore("There's a bug in the FamilySearch implementation of OAuth2 password flow. This test needs to be disabled until then.")]
		public void TestTryPasswordOAuth2Authentication ()
		{
			Assert.IsTrue(api.TryPasswordOAuth2Authentication("heatonra", "1234cispass", "WCQY-7J1Q-GKVV-7DNM-SQ5M-9Q5H-JX3H-CMJK"));
		}

		[Test]
		[Ignore("The test below depends on a signature with a private key that we can't really check in to VCS, so we don't really have a way to test this right now...")]
		public void TestTryAuthCodeOAuth2Authentication ()
		{
			Assert.IsTrue(api.TryAuthCodeOAuth2Authentication("AUTH_CODE_HERE", 
			                                          "DEV_KEY_HERE",
			                                          "CALLBACK_HERE",
			                                          "SECRET_HERE"));
		}
		
		[Test]
		[Ignore("Until we can get the password flow working (see above), we can't really get a valid access token, so this test needs a session id pasted in.")]
		public void TestGetPerson()
		{
			api.CurrentAccessToken = "ACCESS_TOKEN_HERE";
			Person person = api.GetPerson("KW3B-FVV").Resource;
			Assert.IsTrue(person.Names.Count > 0);
		}

	}
}

