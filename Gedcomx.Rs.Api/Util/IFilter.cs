using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gx.Rs.Api.Util
{
    /// <summary>
    /// An interface for manipulating or reporting on a REST API client and request just before the specified client executes the specified request.
    /// </summary>
    public interface IFilter
    {
        /// <summary>
        /// When overridden in a class this method is used to manipulate or report on the specified REST API client and request just before the specified client executes the specified request.
        /// </summary>
        /// <param name="client">The REST API client that will execute the specified request.</param>
        /// <param name="request">The REST API request that will be executed by the specified client.</param>
        void Handle(IRestClient client, IRestRequest request);
    }
}
