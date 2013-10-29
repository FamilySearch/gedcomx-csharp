using System;
using NUnit.Framework;
using Gx.Rs.Api;
using System.Net;
using RestSharp;

namespace Gedcomx.Rs.Api.Test
{
	[TestFixture]
	public class BasicGedcomxApiTest
	{

		[Test]
		public void TestBuildOAuth2AuthorizationUri ()
		{
			//make sure we accept any ssl certificates.
			ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
			BasicGedcomxApi api = new BasicGedcomxApi("https://sandbox.familysearch.org", ".well-known/app-meta");
			Assert.AreEqual("https://sandbox.familysearch.org/cis-web/oauth2/v3/authorization?response_type=code&client_id=12345&redirect_uri=http://mydomain.com/authorized", 
			                api.BuildOAuth2AuthorizationUri("12345", "http://mydomain.com/authorized").ToString());
		}

		[Test]
		public void TestTryOAuth2Authentication ()
		{
			//make sure we accept any ssl certificates.
			ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
			BasicGedcomxApi api = new BasicGedcomxApi("https://sandbox.familysearch.org", ".well-known/app-meta");
			Assert.IsTrue(api.TryOAuth2Authentication("heatonra", "1234cispass", "WCQY-7J1Q-GKVV-7DNM-SQ5M-9Q5H-JX3H-CMJK"));
		}

	}
}

