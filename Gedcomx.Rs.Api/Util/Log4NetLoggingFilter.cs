using log4net;
using log4net.Core;
using RestSharp;
using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gx.Rs.Api.Util
{
    /// <summary>
    /// Enables log4net logging of REST API requests before the requests are executed. See remarks.
    /// </summary>
    /// <remarks>
    /// Log4net is used to log information about requests. The information is output as a DEBUG string and the logger is of
    /// type <c>Gx.Rs.Api.Util.Log4NetLoggingFilter</c>.
    /// </remarks>
    public class Log4NetLoggingFilter : IFilter
    {
        private ILog logger = LogManager.GetLogger(typeof(Log4NetLoggingFilter));

        /// <summary>
        /// This method uses log4net to output a DEBUG string containing the HTTP method and fully qualified URI that will be executed.
        /// </summary>
        /// <param name="client">The REST API client that will execute the specified request.</param>
        /// <param name="request">The REST API request that will be executed by the specified client.</param>
        public void Handle(IRestClient client, IRestRequest request)
        {
            logger.Debug(string.Format("{0} {1}{2}", request.Method, client.BaseUrl, request.Resource));
        }
    }
}
