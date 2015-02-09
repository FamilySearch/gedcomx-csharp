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
using Gedcomx.Support;

namespace FamilySearch.Api
{
    /// <summary>
    /// The FamilySearchPlaces is a collection of FamilySearch places and exposes management of those places.
    /// </summary>
    public class FamilySearchPlaces : FamilySearchCollectionState
    {
        /// <summary>
        /// The default production environment URI for this collection.
        /// </summary>
        public static readonly String URI = "https://familysearch.org/platform/collections/places";
        /// <summary>
        /// The default sandbox environment URI for this collection.
        /// </summary>
        public static readonly String SANDBOX_URI = "https://sandbox.familysearch.org/platform/collections/places";

        /// <summary>
        /// Initializes a new instance of the <see cref="FamilySearchPlaces"/> class using the production environment URI.
        /// </summary>
        public FamilySearchPlaces()
            : this(false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FamilySearchPlaces"/> class.
        /// </summary>
        /// <param name="sandbox">If set to <c>true</c> this will use the sandbox environment URI; otherwise, it will use production.</param>
        public FamilySearchPlaces(bool sandbox)
            : this(new Uri(sandbox ? SANDBOX_URI : URI))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FamilySearchPlaces"/> class.
        /// </summary>
        /// <param name="uri">The URI where the target collection resides.</param>
        public FamilySearchPlaces(Uri uri)
            : this(uri, new FamilySearchStateFactory())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FamilySearchPlaces"/> class.
        /// </summary>
        /// <param name="uri">The URI where the target resides.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        private FamilySearchPlaces(Uri uri, FamilySearchStateFactory stateFactory)
            : this(uri, stateFactory.LoadDefaultClientInt(uri), stateFactory)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FamilySearchPlaces"/> class.
        /// </summary>
        /// <param name="uri">The URI where the target resides.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        private FamilySearchPlaces(Uri uri, IFilterableRestClient client, FamilySearchStateFactory stateFactory)
            : this(new RestRequest().Accept(MediaTypes.GEDCOMX_JSON_MEDIA_TYPE).Build(uri, Method.GET), client, stateFactory)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FamilySearchPlaces"/> class.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        private FamilySearchPlaces(IRestRequest request, IFilterableRestClient client, FamilySearchStateFactory stateFactory)
            : this(request, client.Handle(request), client, null, stateFactory)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FamilySearchPlaces"/> class.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        protected internal FamilySearchPlaces(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, FamilySearchStateFactory stateFactory)
            : base(request, response, client, accessToken, stateFactory)
        {
        }

        /// <summary>
        /// Clones the current state instance.
        /// </summary>
        /// <param name="request">The REST API request used to create this state instance.</param>
        /// <param name="response">The REST API response used to create this state instance.</param>
        /// <param name="client">The REST API client used to create this state instance.</param>
        /// <returns>A cloned instance of the current state instance.</returns>
        protected override GedcomxApplicationState Clone(IRestRequest request, IRestResponse response, IFilterableRestClient client)
        {
            return new FamilySearchPlaces(request, response, client, this.CurrentAccessToken, (FamilySearchStateFactory)this.stateFactory);
        }

        /// <summary>
        /// Creates a state instance without authentication. It will produce an access token, but only good for requests that do not need authentication.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns>A <see cref="FamilySearchPlaces"/> instance containing the REST API response.</returns>
        /// <remarks>See https://familysearch.org/developers/docs/guides/oauth2 for more information.</remarks>
        public FamilySearchPlaces AuthenticateViaUnauthenticatedAccess(String clientId, String ipAddress)
        {
            IDictionary<String, String> formData = new Dictionary<String, String>();
            formData.Add("grant_type", "unauthenticated_session");
            formData.Add("client_id", clientId);
            formData.Add("ip_address", ipAddress);

            return (FamilySearchPlaces)this.AuthenticateViaOAuth2(formData);
        }

        /// <summary>
        /// Reads the place type groups of the current collection.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="VocabElementListState"/> instance containing the REST API response.
        /// </returns>
        public VocabElementListState ReadPlaceTypeGroups(params IStateTransitionOption[] options)
        {
            return this.ReadPlaceElementList(Rel.PLACE_TYPE_GROUPS, options);
        }

        /// <summary>
        /// Reads the place types of the current collection.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="VocabElementListState"/> instance containing the REST API response.
        /// </returns>
        public VocabElementListState ReadPlaceTypes(params IStateTransitionOption[] options)
        {
            return this.ReadPlaceElementList(Rel.PLACE_TYPES, options);
        }

        /// <summary>
        /// Reads place element list using a link with a rel equal to the path specified.
        /// </summary>
        /// <param name="path">The path to use in search of the resource link.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="VocabElementListState"/> instance containing the REST API response.
        /// </returns>
        private VocabElementListState ReadPlaceElementList(String path, params IStateTransitionOption[] options)
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

        /// <summary>
        /// Reads the place type group by the ID specified.
        /// </summary>
        /// <param name="id">The place type group ID to read.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="VocabElementListState"/> instance containing the REST API response.
        /// </returns>
        public VocabElementListState ReadPlaceTypeGroupById(String id, params IStateTransitionOption[] options)
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

        /// <summary>
        /// Reads the place type by the ID specified.
        /// </summary>
        /// <param name="id">The place type ID to read.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="VocabElementState"/> instance containing the REST API response.
        /// </returns>
        public VocabElementState ReadPlaceTypeById(String id, params IStateTransitionOption[] options)
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

        /// <summary>
        /// Read the place group by the ID specified.
        /// </summary>
        /// <param name="id">The place group ID to read.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PlaceGroupState"/> instance containing the REST API response.
        /// </returns>
        public PlaceGroupState ReadPlaceGroupById(String id, params IStateTransitionOption[] options)
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
