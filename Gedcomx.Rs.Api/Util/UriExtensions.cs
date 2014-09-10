using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gx.Rs.Api.Util
{
    public static class UriExtensions
    {
        public static string GetBaseUrl(this Uri @this)
        {
            return @this.GetLeftPart(UriPartial.Authority);
        }
    }
}
