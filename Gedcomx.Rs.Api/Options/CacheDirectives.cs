using Gx.Rs.Api.Util;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gx.Rs.Api.Options
{
    public class CacheDirectives : StateTransitionOption
    {

        private readonly String etag;
        private readonly DateTime? lastModified;

        public CacheDirectives(GedcomxApplicationState state)
            : this(state.ETag, state.LastModified)
        {
        }

        public CacheDirectives(DateTime? lastModified)
            : this(null, lastModified)
        {
        }

        public CacheDirectives(String etag)
            : this(etag, null)
        {
        }

        public CacheDirectives(String etag, DateTime? lastModified)
        {
            this.etag = etag;
            this.lastModified = lastModified;
        }

        public void Apply(IRestRequest request)
        {
            if (this.etag != null)
            {
                request.AddHeader(HeaderParameter.IF_NONE_MATCH, this.etag);
            }

            if (this.lastModified != null)
            {
                request.AddHeader(HeaderParameter.IF_MODIFIED_SINCE, this.lastModified.Value.ToUniversalTime().ToString(ServiceHelper.DATE_FORMAT));
            }
        }
    }
}
