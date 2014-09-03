using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tavis.UriTemplates;

namespace Gx.Rs.Api.Util
{
    public static class UriTemplateExtensions
    {
        public static UriTemplate AddParameter(this UriTemplate @this, string name, object value)
        {
            @this.SetParameter(name, value);

            return @this;
        }
    }
}
