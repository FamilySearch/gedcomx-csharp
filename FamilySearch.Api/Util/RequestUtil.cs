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
    public class RequestUtil
    {
        public static IRestRequest ApplyFamilySearchConneg(IRestRequest request)
        {
            return request.Accept(FamilySearchPlatform.JSON_MEDIA_TYPE).ContentType(FamilySearchPlatform.JSON_MEDIA_TYPE);
        }

        /**
         * Add Accept and Content-Type headers for basic JSON; this is used when retrieving RDF data
         *
         * @param request the request to add the headers to
         * @return the request with Accept and Content-Type headers for basic JSON added
         */
        public static IRestRequest ApplyFamilySearchJson(IRestRequest request)
        {
            return request.Accept(MediaTypes.APPLICATION_JSON_TYPE).ContentType(MediaTypes.APPLICATION_JSON_TYPE);
        }
    }
}
