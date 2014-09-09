using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tavis;
using Tavis.UriTemplates;

namespace Gx.Rs.Api.Util
{
    public static class LinkParserExtensions
    {
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
