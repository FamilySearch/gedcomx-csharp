using System;
using RestSharp;
using System.Text;
using System.Collections.Generic;
using Gx.Conclusion;

namespace Gx.Rs.Api
{
	public class BasicGedcomxApi : GedcomxApi
	{

		private readonly GedcomxApiDescriptor descriptor;
		private string accessToken;

		public BasicGedcomxApi (string host, string discoveryPath) : this ( new RestClient(host), discoveryPath )
		{
		}

		public BasicGedcomxApi ( RestClient client, string discoveryPath ) : this ( new GedcomxApiDescriptor (client, discoveryPath) )
		{
		}

		public BasicGedcomxApi ( GedcomxApiDescriptor descriptor ) {
			this.descriptor = descriptor;
			Timeout = 10000;
		}

		public int Timeout { get; set; }

		public GedcomxApiDescriptor Descriptor {
			get {
				return this.descriptor;
			}
		}

		public string CurrentAccessToken {
			get {
				return this.accessToken;
			}
			set {
				this.accessToken = value;
			}
		}

		public bool RefreshWithAuthentication ()
		{
			return descriptor.RefreshWithAuthentication (accessToken);
		}

		public Uri BuildOAuth2AuthorizationUri(string clientId, string redirectUri) 
		{
			if (this.descriptor.Expired) {
				this.descriptor.Refresh();
			}

			Uri authorizationBase = this.descriptor.GetOAuth2AuthorizationUri();
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

		public bool TryPasswordOAuth2Authentication(string username, string password, string clientId)
		{
			return TryPasswordOAuth2Authentication(username, password, clientId, null);
		}

		public bool TryPasswordOAuth2Authentication(string username, string password, string clientId, string clientSecret)
		{
			if (this.descriptor.Expired) {
				this.descriptor.Refresh();
			}

			RestClient client;
			RestRequest request;
			if (this.descriptor.GetOAuth2TokenRequest(out client, out request)) {
				request.Method = Method.POST;
				request.AddParameter("grant_type", "password");
				request.AddParameter("username", username);
				request.AddParameter("password", password);
				request.AddParameter("client_id", clientId);
				request.Timeout = Timeout;
				if (clientSecret != null) {
					request.AddParameter("client_secret", clientSecret);
				}
				var response = client.Execute<Dictionary<string, object>>(request);
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

		public bool TryAuthCodeOAuth2Authentication(string authCode, string clientId, string redirectUri)
		{
			return TryAuthCodeOAuth2Authentication(authCode, clientId, redirectUri, null);
		}

		public bool TryAuthCodeOAuth2Authentication(string authCode, string clientId, string redirectUri, string clientSecret)
		{
			if (this.descriptor.Expired) {
				this.descriptor.Refresh();
			}
			
			RestClient client;
			RestRequest request;
			if (this.descriptor.GetOAuth2TokenRequest(out client, out request)) {
				request.Method = Method.POST;
				request.AddParameter("code", authCode);
				request.AddParameter("client_id", clientId);
				request.AddParameter("grant_type", "authorization_code");
				request.AddParameter("redirect_uri", redirectUri);
				request.Timeout = Timeout;
				if (clientSecret != null) {
					request.AddParameter("client_secret", clientSecret);
				}
				var response = client.Execute<Dictionary<string, object>>(request);
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
		
		public bool TryClientCredentialsOAuth2Authentication(string clientId, string clientSecret)
		{
			if (this.descriptor.Expired) {
				this.descriptor.Refresh();
			}
			
			RestClient client;
			RestRequest request;
			if (this.descriptor.GetOAuth2TokenRequest(out client, out request)) {
				request.Method = Method.POST;
				request.AddParameter("client_id", clientId);
				request.AddParameter("grant_type", "client_credentials");
				request.Timeout = Timeout;
				if (clientSecret != null) {
					request.AddParameter("client_secret", clientSecret);
				}
				var response = client.Execute<Dictionary<string, object>>(request);
				if (response.ErrorException != null) {
					return false;
				}
				
				Dictionary<string, object> result = response.Data;
				if (result.ContainsKey("token")) {
					this.accessToken = (string) result["token"];
					return true;
				}
				
				return false;
			}
			else {
				return false;
			}
		}
		
		public bool TryGetAccessTokenWithoutAuthentication(string ipAddress, string clientId, string clientSecret = null)
		{
			if (this.descriptor.Expired) {
				this.descriptor.Refresh();
			}

			RestClient client;
			RestRequest request;
			if (this.descriptor.GetOAuth2TokenRequest(out client, out request)) {
				request.Method = Method.POST;
				request.AddParameter("client_id", clientId);
				request.AddParameter("grant_type", "unauthenticated_session");
				request.AddParameter("ip_address", ipAddress);
				request.Timeout = Timeout;
				if (clientSecret != null) {
					request.AddParameter("client_secret", clientSecret);
				}
				var response = client.Execute<Dictionary<string, object>>(request);
				if (response.ErrorException != null) {
					return false;
				}

				Dictionary<string, object> result = response.Data;
				if (result.ContainsKey("token")) {
					this.accessToken = (string) result["token"];
					return true;
				}

				return false;
			}
			else {
				return false;
			}
		}

		public GedcomxApiResponse<Person> GetPerson(String pid) {
			if (this.descriptor.Expired) {
				this.descriptor.Refresh();
			}

			RestClient client;
			RestRequest request;
			if (this.descriptor.GetPersonRequest(pid, out client, out request)) {
				request.Method = Method.GET;
				request.AddHeader("Accept", MediaTypes.GEDCOMX_XML_MEDIA_TYPE);
				request.AddHeader("Authorization", string.Format("Bearer {0}", this.accessToken));
				request.Timeout = Timeout;
				var response = client.Execute<Gedcomx>(request);

				if (response.ResponseStatus != ResponseStatus.Completed) {
					throw new HttpException (response.StatusCode);
				}

				//todo: think about moved? unauthorized? access denied?
				if (response.StatusCode < System.Net.HttpStatusCode.OK || response.StatusCode >= System.Net.HttpStatusCode.MultipleChoices) {
					throw new HttpException (response.StatusCode);
				}
				
				if (response.ErrorException != null) {
					throw response.ErrorException;
				}

				Gedcomx data = response.Data;
				if (data.Persons == null || data.Persons.Count < 1) {
					throw new ApiNonConformanceException("Expected a person returned from the server.");
				}

				return GedcomxApiResponse<Person>.Wrap(response, response.Data.Persons[0]);
			}
			else {
				throw new NotSupportedException("The API descriptor doesn't have a link to the GEDCOM X person resource.");
			}
		}
	}
}

