using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gx.Rs.Api.Util
{
    /// <summary>
    /// Defines a REST API client that can apply filters before handling requests.
    /// </summary>
    public interface IFilterableRestClient : IRestClient
    {
        /// <summary>
        /// When implemented in a class this method adds a filter to apply to the REST API client before handling a request.
        /// </summary>
        /// <param name="filter">The <see cref="IFilter"/> to apply before handling a REST API request.</param>
        void AddFilter(IFilter filter);
        /// <summary>
        /// Handles the specified request by applying the current <see cref="IFilter"/>s then calling <see cref="O:IRestClient.Handle"/>.
        /// </summary>
        /// <param name="request">The REST API request that will be filtered then executed.</param>
        /// <returns>A REST API response after the filters have been applied and the request executed.</returns>
        IRestResponse Handle(IRestRequest request);
        /// <summary>
        /// Gets or sets a value indicating whether the REST API client should automatically follow redirect responses.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the REST API client should automatically follow redirect responses; otherwise, <c>false</c>.
        /// </value>
        bool FollowRedirects { get; set; }
    }
}
