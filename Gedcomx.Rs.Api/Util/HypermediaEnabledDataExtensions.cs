using Gx.Links;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gx.Rs.Api.Util
{
    public static class HypermediaEnabledDataExtensions
    {
        public static Link GetLink(this HypermediaEnabledData @this, String rel)
        {
            return @this.Links.FirstOrDefault(x => x.Rel == rel);
        }
    }
}
