using Gx.Fs;
using Gx.Rs.Api;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gx.Rs.Api.Util;
using System.Net;
using Gedcomx.Model;
using Gx.Fs.Users;

namespace FamilySearch.Api
{
    public class UserState : GedcomxApplicationState<FamilySearchPlatform>
    {
        public UserState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, FamilySearchStateFactory stateFactory)
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
        protected override GedcomxApplicationState<FamilySearchPlatform> Clone(IRestRequest request, IRestResponse response, IFilterableRestClient client)
        {
            return new UserState(request, response, client, this.CurrentAccessToken, (FamilySearchStateFactory)this.stateFactory);
        }

        /// <summary>
        /// Returns the <see cref="FamilySearchPlatform"/> from the REST API response.
        /// </summary>
        /// <param name="response">The REST API response.</param>
        /// <returns>The <see cref="FamilySearchPlatform"/> from the REST API response.</returns>
        protected override FamilySearchPlatform LoadEntity(IRestResponse response)
        {
            return response.StatusCode == HttpStatusCode.OK ? response.ToIRestResponse<FamilySearchPlatform>().Data : null;
        }

        protected override SupportsLinks MainDataElement
        {
            get
            {
                return User;
            }
        }

        public User User
        {
            get
            {
                return Entity == null ? null : Entity.Users.FirstOrDefault();
            }
        }
    }
}
