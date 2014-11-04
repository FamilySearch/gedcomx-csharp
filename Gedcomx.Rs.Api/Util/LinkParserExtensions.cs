using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tavis;
using Tavis.UriTemplates;

namespace Gx.Rs.Api.Util
{
    /// <summary>
    /// An extensions helper class for managing RFC 5988 compliant links.
    /// </summary>
    public static class LinkParserExtensions
    {
        /// <summary>
        /// Gets the link value of the specified extension property without throwing an exception with the specified extension is not present.
        /// </summary>
        /// <param name="this">The RFC 5988 compliant link.</param>
        /// <param name="key">The name of the extension property.</param>
        /// <remarks>
        /// Properties that are not defined by RFC 5988 are stuffed into an extension list. This method only examines those links. All other standard RFC 5988
        /// properties can be managed safely through the RFC 5988 link class <see cref="Tavis.Link"/>.
        /// </remarks>
        /// <returns></returns>
        public static string GetLinkExtensionSafe(this Link @this, string key)
        {
            string result = null;

            if (@this.LinkExtensions.Any(x => x.Key == key))
            {
                result = @this.GetLinkExtension(key);
            }

            return result;
        }
    }
}
