﻿using Gx.Rs.Api.Util;
using RestSharp;
using System;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gx.Rs.Api.Options
{
    /// <summary>
    /// A REST API request options helper providing precondition features. This is similar to the <see cref="CacheDirectives"/> class, but applies
    /// inverse logic.
    /// </summary>
    public class Preconditions : IStateTransitionOption
    {
        private readonly String etag;
        private readonly DateTime? lastModified;

        /// <summary>
        /// Initializes a new instance of the <see cref="Preconditions" /> class.
        /// </summary>
        /// <param name="state">The state with an <see cref="P:GedcomxApplicationState.ETag" /> and <see cref="P:GedcomxApplicationState.LastModified" /> properties
        /// to use for preconditions. See remarks.</param>
        /// <remarks>
        /// If the ETag (entity tag) specified here matches the server's ETag for a resource, the resource will be returned; otherwise, a precondition-failed status
        /// is returned. The same applies to last modified. If the server's last modified date for a resource is equal to the last modified specified here, the
        /// resource will be returned; otherwise, a precondition-failed status is returned.
        /// </remarks>
        public Preconditions(GedcomxApplicationState state)
            : this(state.ETag, state.LastModified)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Preconditions" /> class.
        /// </summary>
        /// <param name="lastModified">
        /// The last modified date to send to the server for evaluation. See remarks.
        /// </param>
        /// <remarks>
        /// If the server's last modified date for a resource is equal to the last modified specified here, the
        /// resource will be returned; otherwise, a precondition-failed status is returned.
        /// </remarks>
        public Preconditions(DateTime? lastModified)
            : this(null, lastModified)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Preconditions"/> class.
        /// </summary>
        /// <param name="etag">The ETag to send to the server for evaluation. See remarks.</param>
        /// <remarks>
        /// If the ETag (entity tag) specified here matches the server's ETag for a resource, the resource will be returned; otherwise, a precondition-failed status
        /// is returned.
        /// </remarks>
        public Preconditions(String etag)
            : this(etag, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Preconditions"/> class.
        /// </summary>
        /// <param name="etag">The ETag to send to the server for evaluation. See remarks.</param>
        /// <param name="lastModified">
        /// The last modified date to send to the server for evaluation. See remarks.
        /// </param>
        /// <remarks>
        /// If the ETag (entity tag) specified here matches the server's ETag for a resource, the resource will be returned; otherwise, a precondition-failed status
        /// is returned. The same applies to last modified. If the server's last modified date for a resource is equal to the last modified specified here, the
        /// resource will be returned; otherwise, a precondition-failed status is returned.
        /// </remarks>
        public Preconditions(String etag, DateTime? lastModified)
        {
            this.etag = etag;
            this.lastModified = lastModified;
        }

        /// <summary>
        /// Applies the ETag or LastModified cache control headers to the specified REST API request. See remarks.
        /// </summary>
        /// <param name="request">The REST API request that will be modified.</param>
        /// <remarks>
        /// The cache control headers are applied conditionally. The ETag and LastModified values will only be applied if they are not null. Furthermore, the
        /// application of each are independent of the other. This could, therefore, only apply a LastModified cache control header and not an ETag cache control
        /// header if the ETag property of this instance were null and LastModified was not.
        /// </remarks>
        public void Apply(IRestRequest request)
        {
            if (this.etag != null)
            {
                request.AddHeader(HeaderParameter.IF_MATCH, this.etag);
            }

            if (this.lastModified != null)
            {
                request.AddHeader(HeaderParameter.IF_UNMODIFIED_SINCE, this.lastModified.Value.ToUniversalTime().ToString(ServiceHelper.DATE_FORMAT));
            }
        }
    }
}
