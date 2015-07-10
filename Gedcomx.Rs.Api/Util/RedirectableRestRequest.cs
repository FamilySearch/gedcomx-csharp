using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gx.Rs.Api.Util
{
    /// <summary>
    /// A RestRequest class that allows for base URL redirection.
    /// </summary>
    public class RedirectableRestRequest : RestRequest
    {
        /// <summary>
        /// The base URL for this request. If it is different than the client base URL, this one will be used instead.
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public RedirectableRestRequest()
            : base()
        {
        }

        /// <summary>
        /// Sets Method property to value of method
        /// </summary>
        /// <param name="method">Method to use for this request</param>
        public RedirectableRestRequest(Method method)
            : base(method)
        {
        }

        /// <summary>
        /// Sets Resource property
        /// </summary>
        /// <param name="resource">Resource to use for this request</param>
        public RedirectableRestRequest(string resource)
            : base(resource)
        {
        }

        /// <summary>
        /// Sets Resource property
        /// </summary>
        /// <param name="resource">Resource to use for this request</param>
        public RedirectableRestRequest(Uri resource)
            : base(resource)
        {
        }

        /// <summary>
        /// Sets Resource and Method properties
        /// </summary>
        /// <param name="resource">Resource to use for this request</param>
        /// <param name="method">Method to use for this request</param>
        public RedirectableRestRequest(string resource, Method method)
            : base(resource, method)
        {
        }

        /// <summary>
        /// Sets Resource and Method properties
        /// </summary>
        /// <param name="resource">Resource to use for this request</param>
        /// <param name="method">Method to use for this request</param>
        public RedirectableRestRequest(Uri resource, Method method)
            : base(resource, method)
        {
        }
    }
}
