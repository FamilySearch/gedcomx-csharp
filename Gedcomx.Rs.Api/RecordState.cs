using Gedcomx.Model;
using Gx.Links;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gx.Rs.Api.Util;
using Gedcomx.Support;

namespace Gx.Rs.Api
{
    /// <summary>
    /// The RecordState exposes management functions for a record.
    /// </summary>
    public class RecordState : GedcomxApplicationState<Gedcomx>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecordState"/> class.
        /// </summary>
        /// <param name="uri">The URI where the target resides.</param>
        public RecordState(Uri uri)
            : this(uri, new StateFactory())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordState"/> class.
        /// </summary>
        /// <param name="uri">The URI where the target resides.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        private RecordState(Uri uri, StateFactory stateFactory)
            : this(uri, stateFactory.LoadDefaultClient(uri), stateFactory)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordState"/> class.
        /// </summary>
        /// <param name="uri">The URI where the target resides.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        private RecordState(Uri uri, IFilterableRestClient client, StateFactory stateFactory)
            : this(new RestRequest().Accept(MediaTypes.GEDCOMX_JSON_MEDIA_TYPE).Build(uri, Method.GET), client, stateFactory)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordState"/> class.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        private RecordState(IRestRequest request, IFilterableRestClient client, StateFactory stateFactory)
            : this(request, client.Handle(request), client, null, stateFactory)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordState"/> class.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        public RecordState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, StateFactory stateFactory)
            : base(request, response, client, accessToken, stateFactory)
        {
        }

        /// <summary>
        /// Gets the rel name for the current state instance. This is expected to be overridden.
        /// </summary>
        /// <value>
        /// The rel name for the current state instance
        /// </value>
        public override String SelfRel
        {
            get
            {
                return Rel.RECORD;
            }
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
            return new RecordState(request, response, client, this.CurrentAccessToken, this.stateFactory);
        }
    }
}
