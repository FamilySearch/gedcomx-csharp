using System;
using RestSharp;
using System.Text;
using System.Collections.Generic;

namespace Gx.Rs.Api
{
	public class BasicGedcomxApi : GedcomxApi
	{
		private readonly RestClient client;
		private readonly GedcomxApiDescriptor descriptor;
		private string accessToken;

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
			if (this.descriptor.Expired) {
				this.descriptor.Refresh(this.client);
			}

			Uri authorizationBase = this.descriptor.OAuth2AuthorizationUri;
			if (authorizationBase != null) {
				UriBuilder builder = new UriBuilder(authorizationBase);
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

		public bool TryOAuth2Authentication(string username, string password, string clientId)
		{
			if (this.descriptor.Expired) {
				this.descriptor.Refresh(this.client);
			}

			Uri tokenUri = this.descriptor.OAuth2TokenUri;
			if (tokenUri != null) {
				RestRequest request = new RestRequest(tokenUri, Method.POST);
				request.AddParameter("grant_type", "password");
				request.AddParameter("username", username);
				request.AddParameter("password", password);
				request.AddParameter("client_id", clientId);
				var response = this.client.Execute<Dictionary<string, object>>(request);
				if (response.ErrorException != null) {
					return false;
				}

				Dictionary<string, object> result = response.Data;
				if (result.ContainsKey("access_token")) {
					this.accessToken = (string) result["access_token"];
					return true;
				}

				return false;
			}
			else {
				return false;
			}
		}

		string CurrentAccessToken {
			get {
				return this.accessToken;
			}
		}
	}
}

