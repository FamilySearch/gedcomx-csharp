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
			BasicGedcomxApi api = new BasicGedcomxApi("https://familysearch.org", ".well-known/app-meta");
			Assert.AreEqual("https://ident.familysearch.org/cis-web/oauth2/v3/authorization?response_type=code&client_id=12345&redirect_uri=http://mydomain.com/authorized", 
			                api.BuildOAuth2AuthorizationUri("12345", "http://mydomain.com/authorized").ToString());
		}

	}
}

