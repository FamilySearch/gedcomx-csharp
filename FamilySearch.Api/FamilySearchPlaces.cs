using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gx.Rs.Api.Util;
using Gx.Rs.Api;
using Gx.Links;
using Tavis.UriTemplates;
using FamilySearch.Api.Util;

namespace FamilySearch.Api
{
    public class FamilySearchPlaces : FamilySearchCollectionState
    {
        public static readonly String URI = "https://familysearch.org/platform/collections/places";
        public static readonly String SANDBOX_URI = "https://sandbox.familysearch.org/platform/collections/places";

        public FamilySearchPlaces()
            : this(false)
        {
        }

        public FamilySearchPlaces(bool sandbox)
            : this(new Uri(sandbox ? SANDBOX_URI : URI))
        {
        }

        public FamilySearchPlaces(Uri uri)
            : this(uri, new FamilySearchStateFactory())
        {
        }

        private FamilySearchPlaces(Uri uri, FamilySearchStateFactory stateFactory)
            : this(uri, stateFactory.LoadDefaultClient(uri), stateFactory)
        {
        }

        private FamilySearchPlaces(Uri uri, IRestClient client, FamilySearchStateFactory stateFactory)
            : this(new RestRequest().Accept(MediaTypes.GEDCOMX_JSON_MEDIA_TYPE).Build(uri, Method.GET), client, stateFactory)
        {
        }

        private FamilySearchPlaces(IRestRequest request, IRestClient client, FamilySearchStateFactory stateFactory)
            : this(request, client.Execute(request), client, null, stateFactory)
        {
        }

        protected internal FamilySearchPlaces(IRestRequest request, IRestResponse response, IRestClient client, String accessToken, FamilySearchStateFactory stateFactory)
            : base(request, response, client, accessToken, stateFactory)
        {
        }

        protected override GedcomxApplicationState Clone(IRestRequest request, IRestResponse response, IRestClient client)
        {
            return new FamilySearchPlaces(request, response, client, this.CurrentAccessToken, (FamilySearchStateFactory)this.stateFactory);
        }

        public FamilySearchPlaces AuthenticateViaUnauthenticatedAccess(String clientId, String ipAddress)
        {
            IDictionary<String, String> formData = new Dictionary<String, String>();
            formData.Add("grant_type", "unauthenticated_session");
            formData.Add("client_id", clientId);
            formData.Add("ip_address", ipAddress);

            return (FamilySearchPlaces)this.AuthenticateViaOAuth2(formData);
        }

        /**
         * Read the list of place type groups
         *
         * @param options state transition options to be included
         * @return the list of place type groups
         */
        public VocabElementListState ReadPlaceTypeGroups(params StateTransitionOption[] options)
        {
            return this.ReadPlaceElementList(Rel.PLACE_TYPE_GROUPS, options);
        }

        /**
         * Read the list of place types
         *
         * @param options state transition options to be included
         * @return the list of place types
         */
        public VocabElementListState ReadPlaceTypes(params StateTransitionOption[] options)
        {
            return this.ReadPlaceElementList(Rel.PLACE_TYPES, options);
        }

        /**
         * Read the VocabElementList from the given path
         *
         * @param options state transition options to be included
         * @return a VocabElementListState from the given path
         */
        private VocabElementListState ReadPlaceElementList(String path, params StateTransitionOption[] options)
        {
            Link link = GetLink(path);
            if (null == link || null == link.Template)
            {
                return null;
            }

            String template = link.Template;
            String uri = new UriTemplate(template).Resolve();

            IRestRequest request = RequestUtil.ApplyFamilySearchJson(CreateAuthenticatedRequest()).Build(uri, Method.GET);
            return this.stateFactory.NewVocabElementListState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /**
         * Read the place type group with the given id
         *
         * @param id the id of the place type group to be read
         * @param options state transition options to be included
         * @return the place type group with the given id
         */
        public VocabElementListState ReadPlaceTypeGroupById(String id, params StateTransitionOption[] options)
        {
            Link link = GetLink(Rel.PLACE_TYPE_GROUP);
            if (link == null || link.Template == null)
            {
                return null;
            }

            String template = link.Template;
            String uri = new UriTemplate(template).AddParameter("ptgid", id).Resolve();

            IRestRequest request = RequestUtil.ApplyFamilySearchJson(CreateAuthenticatedRequest()).Build(uri, Method.GET);
            return this.stateFactory.NewVocabElementListState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /**
         * Read the place type with the given id
         *
         * @param id the id of the place type to be read
         * @param options state transition options to be included
         * @return the place type with the given id
         */
        public VocabElementState ReadPlaceTypeById(String id, params StateTransitionOption[] options)
        {
            Link link = GetLink(Rel.PLACE_TYPE);
            if (link == null || link.Template == null)
            {
                return null;
            }

            String template = link.Template;
            String uri = new UriTemplate(template).AddParameter("ptid", id).Resolve();

            IRestRequest request = RequestUtil.ApplyFamilySearchJson(CreateAuthenticatedRequest()).Build(uri, Method.GET);
            return this.stateFactory.NewVocabElementState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /**
         *
         */
        public PlaceGroupState ReadPlaceGroupById(String id, params StateTransitionOption[] options)
        {
            Link link = GetLink(Rel.PLACE_GROUP);
            if (link == null || link.Template == null)
            {
                return null;
            }
            String template = link.Template;
            String uri = new UriTemplate(template).AddParameter("pgid", id).Resolve();

            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).Build(uri, Method.GET);
            return this.stateFactory.NewPlaceGroupState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }
    }
}
