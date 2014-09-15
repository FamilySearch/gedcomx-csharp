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
        public UserState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken, FamilySearchStateFactory stateFactory)
            : base(request, response, client, accessToken, stateFactory)
        {
        }

        protected override GedcomxApplicationState Clone(IRestRequest request, IRestResponse response, IRestClient client)
        {
            return new UserState(request, response, client, this.CurrentAccessToken, (FamilySearchStateFactory)this.stateFactory);
        }

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
