using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gx.Rs.Api.Util;
using Gx.Rs.Api;
using Gx.Fs;
using Gedcomx.Support;

namespace FamilySearch.Api.Util
{
    /// <summary>
    /// An extension utlity class to set accept and content-type headers.
    /// </summary>
    public class RequestUtil
    {
        /// <summary>
        /// Returns the specified REST API request with an accept and content-type header of "application/x-fs-v1+json".
        /// </summary>
        /// <param name="request">The REST API request to be modified.</param>
        /// <returns>The specified REST API request with an accept and content-type header of "application/x-fs-v1+json".</returns>
        public static IRestRequest ApplyFamilySearchConneg(IRestRequest request)
        {
            return request.Accept(FamilySearchPlatform.JSON_MEDIA_TYPE).ContentType(FamilySearchPlatform.JSON_MEDIA_TYPE);
        }

        /// <summary>
        /// Returns the specified REST API request with an accept and content-type header of "application/json".
        /// </summary>
        /// <param name="request">The REST API request to be modified.</param>
        /// <returns>The specified REST API request with an accept and content-type header of "application/json".</returns>
        public static IRestRequest ApplyFamilySearchJson(IRestRequest request)
        {
            return request.Accept(MediaTypes.APPLICATION_JSON_TYPE).ContentType(MediaTypes.APPLICATION_JSON_TYPE);
        }
    }
}
