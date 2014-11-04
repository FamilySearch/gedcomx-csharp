using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gx.Rs.Api.Util
{
    /// <summary>
    /// Represents a REST API client that can have <see cref="IFilter"/>s applied just before executing requests. See remarks.
    /// </summary>
    /// <remarks>
    /// It is important to note, however, that in order for the <see cref="IFilter"/>s to be applied, the <see cref="Handle"/> method must be called.
    /// Calling <see cref="O:IRestClient.Execute"/> directly will not result in these filters being applied.
    /// </remarks>
    public class FilterableRestClient : RestClient, IFilterableRestClient
    {
        private List<IFilter> filters;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterableRestClient"/> class.
        /// </summary>
        public FilterableRestClient()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterableRestClient"/> class.
        /// </summary>
        /// <param name="baseUrl">The base URI for the REST API client.</param>
        public FilterableRestClient(string baseUrl)
            : base(baseUrl)
        {
            filters = new List<IFilter>();
        }

        /// <summary>
        /// Adds a filter to the current REST API client.
        /// </summary>
        /// <param name="filter">The filter to apply to the REST API client. See remarks.</param>
        /// <remarks>
        /// The filter added here will be applied for all subsequent calls to <see cref="Handle"/>. It is important to note, however, that in order for any
        /// <see cref="IFilter"/> to be applied, the <see cref="Handle"/> method must be called. Calling <see cref="O:IRestClient.Execute"/> directly will not
        /// result in these filters being applied.
        /// </remarks>
        public void AddFilter(IFilter filter)
        {
            filters.Add(filter);
        }

        /// <summary>
        /// Handles the specified request by applying all current filters.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <returns></returns>
        public IRestResponse Handle(IRestRequest request)
        {
            foreach (var filter in filters)
            {
                filter.Handle((IRestClient)this, request);
            }

            return this.Execute(request);
        }
    }
}
