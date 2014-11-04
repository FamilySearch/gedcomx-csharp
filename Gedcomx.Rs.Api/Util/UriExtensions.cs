using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gx.Rs.Api.Util
{
    /// <summary>
    /// A URI extensions helper class.
    /// </summary>
    public static class UriExtensions
    {
        /// <summary>
        /// Gets the base URL of the specified URI.
        /// </summary>
        /// <param name="this">The URI from which the base URL is determined. See remarks.</param>
        /// <returns>The base URL from the specified URI.</returns>
        /// <remarks>
        /// The specified URI should be an absolute URI. The base URL is determined by extracting the authority from the specified URI, which will fail
        /// for relative URIs.
        /// </remarks>
        public static string GetBaseUrl(this Uri @this)
        {
            return @this.GetLeftPart(UriPartial.Authority);
        }
    }
}
