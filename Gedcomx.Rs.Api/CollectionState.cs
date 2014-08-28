using Gx.Common;
using Gx.Conclusion;
using Gx.Links;
using Gx.Records;
using Gx.Rs.Api.Util;
using Gx.Source;
using Gx.Types;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Tavis.UriTemplates;

namespace Gx.Rs.Api
{

    public class CollectionState : GedcomxApplicationState<Gedcomx>
    {

        public CollectionState(Uri uri)
            : this(uri, new StateFactory())
        {
        }

        private CollectionState(Uri uri, StateFactory stateFactory)
            : this(uri, stateFactory.LoadDefaultClient(uri), stateFactory)
        {
        }

        private CollectionState(Uri uri, IRestClient client, StateFactory stateFactory)
            : this(new RestRequest(uri, Method.GET).SetDataFormat(DataFormat.Xml), client, stateFactory) // XML format is used since JSON fails to parse Links correctly
        {
        }

        private CollectionState(IRestRequest request, IRestClient client, StateFactory stateFactory)
            : this(request, client.Execute<Gedcomx>(request), client, null, stateFactory)
        {
        }

        internal CollectionState(IRestRequest request, IRestResponse<Gedcomx> response, IRestClient client, String accessToken, StateFactory stateFactory)
            : base(request, response, client, accessToken, stateFactory)
        {
        }

        protected override GedcomxApplicationState Clone(IRestRequest request, IRestResponse<Gedcomx> response, IRestClient client)
        {
            return new CollectionState(request, response, client, this.CurrentAccessToken, this.stateFactory);
        }

        protected override Gedcomx LoadEntity(IRestResponse<Gedcomx> response)
        {
            Gedcomx result = null;

            if (response != null)
            {
                result = response.Data;
            }

            return result;
        }

        protected override Collection MainDataElement
        {
            get
            {
                return Entity == null ? null : Entity.Collections == null ? null : Entity.Collections.FirstOrDefault();
            }
        }

        public CollectionState AuthenticateWithAccessToken(String accessToken)
        {
            return (CollectionState)base.AuthenticateWithAccessToken(accessToken);
        }

        public CollectionState AuthenticateViaOAuth2Password(String username, String password, String clientId)
        {
            return (CollectionState)base.AuthenticateViaOAuth2Password(username, password, clientId);
        }

        public CollectionState AuthenticateViaOAuth2Password(String username, String password, String clientId, String clientSecret)
        {
            return (CollectionState)base.AuthenticateViaOAuth2Password(username, password, clientId, clientSecret);
        }

        public CollectionState AuthenticateViaOAuth2AuthCode(String authCode, String redirect, String clientId)
        {
            return (CollectionState)base.AuthenticateViaOAuth2AuthCode(authCode, redirect, clientId);
        }

        public CollectionState AuthenticateViaOAuth2AuthCode(String authCode, String redirect, String clientId, String clientSecret)
        {
            return (CollectionState)base.AuthenticateViaOAuth2AuthCode(authCode, redirect, clientId, clientSecret);
        }

        public CollectionState authenticateViaOAuth2ClientCredentials(String clientId, String clientSecret)
        {
            return (CollectionState)base.AuthenticateViaOAuth2ClientCredentials(clientId, clientSecret);
        }

        public CollectionState AuthenticateViaOAuth2(IDictionary<String, String> formData, params StateTransitionOption[] options)
        {
            return (CollectionState)base.AuthenticateViaOAuth2(formData, options);
        }
    }
}
