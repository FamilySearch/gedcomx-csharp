using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gx.Rs.Api
{
    /// <summary>
    /// Defines a method to manipulate and apply options to a REST API request before execution.
    /// </summary>
    public interface IStateTransitionOption
    {
        /// <summary>
        /// When overriden in a class, this method applies specific options or manipulates the REST API request.
        /// </summary>
        /// <param name="request">The REST API request that will be modified or manipulated.</param>
        void Apply(IRestRequest request);
    }
}
