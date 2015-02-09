using Gx.Rs.Api;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gx.Rs.Api.Util;
using Gx.Links;
using Tavis.UriTemplates;
using Gx.Conclusion;
using Gx.Common;
using FamilySearch.Api.Util;
using Gx.Fs.Discussions;
using Gx.Fs;
using Gedcomx.Support;

namespace FamilySearch.Api
{
    /// <summary>
    /// The FamilySearchCollectionState is a collection of FamilySearch resources and exposes management of those resources.
    /// </summary>
    public class FamilySearchCollectionState : CollectionState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FamilySearchCollectionState"/> class.
        /// </summary>
        /// <param name="uri">The URI where the target collection resides.</param>
        public FamilySearchCollectionState(Uri uri)
            : this(uri, new FamilySearchStateFactory())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FamilySearchCollectionState"/> class.
        /// </summary>
        /// <param name="uri">The URI where the target resides.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        private FamilySearchCollectionState(Uri uri, FamilySearchStateFactory stateFactory)
            : this(uri, stateFactory.LoadDefaultClientInt(uri), stateFactory)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FamilySearchCollectionState"/> class.
        /// </summary>
        /// <param name="uri">The URI where the target resides.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        private FamilySearchCollectionState(Uri uri, IFilterableRestClient client, FamilySearchStateFactory stateFactory)
            : this(new RestRequest().Accept(MediaTypes.GEDCOMX_JSON_MEDIA_TYPE).Build(uri, Method.GET), client, stateFactory)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FamilySearchCollectionState"/> class.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        private FamilySearchCollectionState(IRestRequest request, IFilterableRestClient client, FamilySearchStateFactory stateFactory)
            : this(request, client.Handle(request), client, null, stateFactory)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FamilySearchCollectionState"/> class.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        protected internal FamilySearchCollectionState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, FamilySearchStateFactory stateFactory)
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
            return new FamilySearchCollectionState(request, response, client, this.CurrentAccessToken, (FamilySearchStateFactory)this.stateFactory);
        }

        /// <summary>
        /// Normalizes the specified date to a <see cref="DateInfo"/>.
        /// </summary>
        /// <param name="date">The date to be normalized.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns></returns>
        public DateInfo NormalizeDate(String date, params IStateTransitionOption[] options)
        {
            Link normalizedDateLink = GetLink(Rel.NORMALIZED_DATE);
            if (normalizedDateLink == null || normalizedDateLink.Template == null)
            {
                return null;
            }
            String template = normalizedDateLink.Template;
            String uri = new UriTemplate(template).AddParameter("date", date).Resolve();

            IRestRequest request = CreateRequest().Accept(MediaTypes.TEXT_PLAIN).Build(uri, Method.GET);
            IRestResponse response = Invoke(request, options);
            DateInfo dateValue = new DateInfo();
            dateValue.Original = date;
            dateValue.AddNormalizedExtension(new TextValue(response.ToIRestResponse<String>().Data));
            if (response.Headers != null)
            {
                dateValue.Formal = response.Headers.Where(x => x.Name == "Location").Select(x => x.Value as string).FirstOrDefault();
            }
            return dateValue;
        }

        /// <summary>
        /// Reads the current tree user data.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="UserState"/> instance containing the REST API response.
        /// </returns>
        public UserState ReadCurrentUser(params IStateTransitionOption[] options)
        {
            Link link = GetLink(Rel.CURRENT_USER);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).Build(link.Href, Method.GET);
            return ((FamilySearchStateFactory)this.stateFactory).NewUserState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Searches for person matches based off the specified query.
        /// </summary>
        /// <param name="query">The query with search parameters to use.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonMatchResultsState"/> instance containing the REST API response.
        /// </returns>
        /// <remarks>
        /// The REST API may not produce results if the query is lacking in any way. When this occurs, use the <see cref="P:PersonMatchResults.Warnings"/>
        /// collection to determine possible causes. The most common issue is not supplying a sufficient number of search parameters, in which case too
        /// many search results could return.
        /// </remarks>
        public PersonMatchResultsState SearchForPersonMatches(GedcomxPersonSearchQueryBuilder query, params IStateTransitionOption[] options)
        {
            return SearchForPersonMatches(query.Build(), options);
        }

        /// <summary>
        /// Searches for person matches based off the specified query.
        /// </summary>
        /// <param name="query">The query with search parameters to use.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonMatchResultsState"/> instance containing the REST API response.
        /// </returns>
        /// <remarks>
        /// The REST API may not produce results if the query is lacking in any way. When this occurs, use the <see cref="P:PersonMatchResults.Warnings"/>
        /// collection to determine possible causes. The most common issue is not supplying a sufficient number of search parameters, in which case too
        /// many search results could return.
        /// 
        /// The query string syntax is documented here: https://familysearch.org/developers/docs/api/tree/Person_Search_resource
        /// </remarks>
        public PersonMatchResultsState SearchForPersonMatches(String query, params IStateTransitionOption[] options)
        {
            Link searchLink = GetLink(Rel.PERSON_MATCHES_QUERY);
            if (searchLink == null || searchLink.Template == null)
            {
                return null;
            }
            String template = searchLink.Template;

            String uri = new UriTemplate(template).AddParameter("q", query).Resolve();

            IRestRequest request = CreateAuthenticatedFeedRequest().Build(uri, Method.GET);
            return ((FamilySearchStateFactory)this.stateFactory).NewPersonMatchResultsState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Reads the discussions on the current collection.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="DiscussionsState"/> instance containing the REST API response.
        /// </returns>
        public DiscussionsState ReadDiscussions(params IStateTransitionOption[] options)
        {
            Link link = GetLink(Rel.DISCUSSIONS);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).Build(link.Href, Method.GET);
            return ((FamilySearchStateFactory)this.stateFactory).NewDiscussionsState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Adds a discussion to the current collection.
        /// </summary>
        /// <param name="discussion">The discussion.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="DiscussionState"/> instance containing the REST API response.
        /// </returns>
        /// <exception cref="Gx.Rs.Api.GedcomxApplicationException">Thrown if a link to the required resource cannot be found.</exception>
        public DiscussionState AddDiscussion(Discussion discussion, params IStateTransitionOption[] options)
        {
            Link link = GetLink(Rel.DISCUSSIONS);
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException("Unable to add discussion: missing link.");
            }

            FamilySearchPlatform entity = new FamilySearchPlatform();
            entity.AddDiscussion(discussion);
            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).SetEntity(entity).Build(link.Href, Method.POST);
            return ((FamilySearchStateFactory)this.stateFactory).NewDiscussionState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }
    }
}
