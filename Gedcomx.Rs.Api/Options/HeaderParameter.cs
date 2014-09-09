using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gx.Rs.Api.Options
{
    public class HeaderParameter : StateTransitionOption
    {

        public static readonly String LANG = "Accept-Language";
        public static readonly String LOCALE = LANG;
        public static readonly String IF_NONE_MATCH = "If-None-Match";
        public static readonly String IF_MODIFIED_SINCE = "If-Modified-Since";
        public static readonly String IF_MATCH = "If-Match";
        public static readonly String IF_UNMODIFIED_SINCE = "If-Unmodified-Since";

        private readonly bool replace;
        private readonly String name;
        private readonly String[] value;

        public HeaderParameter(String name, params String[] value)
            : this(false, name, value)
        {
        }

        public HeaderParameter(bool replace, String name, params String[] value)
        {
            this.replace = replace;
            this.name = name;
            this.value = value.Length > 0 ? value : new String[] { };
        }

        public void Apply(IRestRequest request)
        {
            if (this.replace)
            {
                request.Parameters.RemoveAll(x => x.Type == ParameterType.HttpHeader);
                request.Parameters.AddRange(value.Select(x => new Parameter() { Type = ParameterType.HttpHeader, Name = this.name, Value = x }));
            }
            else
            {
                foreach (String value in this.value)
                {
                    request.AddHeader(this.name, value);
                }
            }
        }

        public static HeaderParameter Lang(String value)
        {
            return new HeaderParameter(true, LANG, value);
        }

        public static HeaderParameter Locale(String value)
        {
            return new HeaderParameter(true, LOCALE, value);
        }

    }
}
