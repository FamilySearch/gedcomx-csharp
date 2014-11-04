using Gx.Links;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gx.Rs.Api.Util
{
    /// <summary>
    /// An extension helper for looking up links by rels.
    /// </summary>
    public static class HypermediaEnabledDataExtensions
    {
        /// <summary>
        /// Gets the link from the <see cref="HypermediaEnabledData"/> instance specified by the rel.
        /// </summary>
        /// <param name="this">The <see cref="HypermediaEnabledData"/> instance from which the links will be filtered.</param>
        /// <param name="rel">The rel the link is known by.</param>
        /// <returns>The link with a matching rel as specified by the rel parameter.</returns>
        public static Link GetLink(this HypermediaEnabledData @this, String rel)
        {
            return @this.Links.FirstOrDefault(x => x.Rel == rel);
        }
    }
}
