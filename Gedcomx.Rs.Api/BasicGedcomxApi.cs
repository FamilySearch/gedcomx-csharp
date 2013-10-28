using System;
using RestSharp;
using System.Text;

namespace Gx.Rs.Api
{
	public class BasicGedcomxApi : GedcomxApi
	{
		private readonly RestClient client;
		private readonly GedcomxApiDescriptor descriptor;

		public BasicGedcomxApi (string host, string discoveryPath) : this ( new RestClient(host), discoveryPath )
		{
		}

		public BasicGedcomxApi ( RestClient client, string discoveryPath ) : this ( client, new GedcomxApiDescriptor (client, discoveryPath) )
		{
		}

		public BasicGedcomxApi ( RestClient client, GedcomxApiDescriptor descriptor ) {
			this.client = client;
			this.descriptor = descriptor;
		}

		public RestClient Client {
			get {
				return this.client;
			}
		}

		public GedcomxApiDescriptor Descriptor {
			get {
				return this.descriptor;
			}
		}

		public Uri BuildOAuth2AuthorizationUri(string clientId, string redirectUri) 
		{
			Uri baseUri = this.descriptor.OAuth2AuthorizationUri;
			if (baseUri != null) {
				UriBuilder builder = new UriBuilder(baseUri);
				StringBuilder query = new StringBuilder("response_type=code&client_id=")
					.Append(Uri.EscapeUriString(clientId))
				    .Append("&redirect_uri=")
					.Append(Uri.EscapeUriString(redirectUri));
				builder.Query = query.ToString();
				return builder.Uri;
			}
			else {
				return null;
			}
		}
	}
}

