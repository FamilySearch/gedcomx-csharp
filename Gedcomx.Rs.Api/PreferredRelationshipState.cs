using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gx.Rs.Api
{
    /// <summary>
    /// A basic interface declaring the explicit ability to produce a self URI.
    /// </summary>
    public interface IPreferredRelationshipState
    {
        /// <summary>
        /// Gets the self URI.
        /// </summary>
        /// <returns></returns>
        string GetSelfUri();
    }
}
