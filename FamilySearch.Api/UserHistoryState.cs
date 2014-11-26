using Gx.Rs.Api;
using Gx.Rs.Api.Util;
using Gx.Source;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FamilySearch.Api
{
    /// <summary>
    /// The UserHistoryState exposes management functions for user history.
    /// </summary>
    public class UserHistoryState : GedcomxApplicationState<Gx.Gedcomx>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserHistoryState"/> class.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        public UserHistoryState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, FamilySearchStateFactory stateFactory)
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
            return new UserHistoryState(request, response, client, this.CurrentAccessToken, (FamilySearchStateFactory)this.stateFactory);
        }

        /// <summary>
        /// Returns the <see cref="Gx.Gedcomx"/> from the REST API response.
        /// </summary>
        /// <param name="response">The REST API response.</param>
        /// <returns>The <see cref="Gx.Gedcomx"/> from the REST API response.</returns>
        protected override Gx.Gedcomx LoadEntity(IRestResponse response)
        {
            return response.StatusCode == HttpStatusCode.OK ? response.ToIRestResponse<Gx.Gedcomx>().Data : null;
        }

        /// <summary>
        /// Gets the user history represented by this state instance.
        /// </summary>
        /// <value>
        /// The user history represented by this state instance.
        /// </value>
        public List<SourceDescription> UserHistory
        {
            get
            {
                return Entity == null ? null : Entity.SourceDescriptions;
            }
        }
    }
}
