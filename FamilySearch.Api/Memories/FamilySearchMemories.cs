using Gx.Rs.Api;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gx.Rs.Api.Util;
using FamilySearch.Api.Ft;
using Gedcomx.Support;

namespace FamilySearch.Api.Memories
{
    public class FamilySearchMemories : FamilySearchCollectionState
    {
        public static readonly String URI = "https://familysearch.org/platform/collections/memories";
        public static readonly String SANDBOX_URI = "https://sandbox.familysearch.org/platform/collections/memories";

        public FamilySearchMemories()
            : this(false)
        {
        }

        public FamilySearchMemories(bool sandbox)
            : this(new Uri(sandbox ? SANDBOX_URI : URI))
        {
        }

        public FamilySearchMemories(Uri uri)
            : this(uri, new FamilySearchStateFactory())
        {
        }

        private FamilySearchMemories(Uri uri, FamilySearchStateFactory stateFactory)
            : this(uri, stateFactory.LoadDefaultClientInt(uri), stateFactory)
        {
        }

        private FamilySearchMemories(Uri uri, IFilterableRestClient client, FamilySearchStateFactory stateFactory)
            : this(new RestRequest().Accept(MediaTypes.GEDCOMX_JSON_MEDIA_TYPE).Build(uri, Method.GET), client, stateFactory)
        {
        }

        private FamilySearchMemories(IRestRequest request, IFilterableRestClient client, FamilySearchStateFactory stateFactory)
            : this(request, client.Handle(request), client, null, stateFactory)
        {
        }

        protected FamilySearchMemories(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, FamilySearchStateFactory stateFactory)
            : base(request, response, client, accessToken, stateFactory)
        {
        }

        protected override GedcomxApplicationState<Gx.Gedcomx> Clone(IRestRequest request, IRestResponse response, IFilterableRestClient client)
        {
            return new FamilySearchMemories(request, response, this.Client, this.CurrentAccessToken, (FamilySearchStateFactory)this.stateFactory);
        }

        public FamilySearchMemories AuthenticateViaUnauthenticatedAccess(String clientId, String ipAddress)
        {

            IDictionary<String, String> formData = new Dictionary<String, String>();
            formData.Add("grant_type", "unauthenticated_session");
            formData.Add("client_id", clientId);
            formData.Add("ip_address", ipAddress);

            return (FamilySearchMemories)this.AuthenticateViaOAuth2(formData);
        }
    }
}
