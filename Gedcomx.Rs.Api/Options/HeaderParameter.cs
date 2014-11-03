using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gx.Rs.Api.Options
{
    /// <summary>
    /// This is a helper class for managing headers in REST API requests.
    /// </summary>
    public class HeaderParameter : StateTransitionOption
    {
        /// <summary>
        /// The accept language header
        /// </summary>
        public static readonly String LANG = "Accept-Language";
        /// <summary>
        /// The locale header
        /// </summary>
        public static readonly String LOCALE = LANG;
        /// <summary>
        /// The if-none-match header
        /// </summary>
        public static readonly String IF_NONE_MATCH = "If-None-Match";
        /// <summary>
        /// The if-modified-since header
        /// </summary>
        public static readonly String IF_MODIFIED_SINCE = "If-Modified-Since";
        /// <summary>
        /// The if-match header
        /// </summary>
        public static readonly String IF_MATCH = "If-Match";
        /// <summary>
        /// The if-unmodified-since header
        /// </summary>
        public static readonly String IF_UNMODIFIED_SINCE = "If-Unmodified-Since";

        private readonly bool replace;
        private readonly String name;
        private readonly String[] value;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderParameter"/> class.
        /// </summary>
        /// <param name="name">The name of the header to use.</param>
        /// <param name="value">The value of this new header.</param>
        public HeaderParameter(String name, params String[] value)
            : this(false, name, value)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderParameter" /> class.
        /// </summary>
        /// <param name="replace">if set to <c>true</c> and if a header already exists with the same name, this header parameter will replace the existing header.</param>
        /// <param name="name">The name of the header to use.</param>
        /// <param name="value">The value of this new header.</param>
        public HeaderParameter(bool replace, String name, params String[] value)
        {
            this.replace = replace;
            this.name = name;
            this.value = value.Length > 0 ? value : new String[] { };
        }

        /// <summary>
        /// This method adds the current header parameters to the REST API request.
        /// </summary>
        /// <param name="request">The REST API request that will be modified or manipulated.</param>
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

        /// <summary>
        /// Creates an accept-language header parameter.
        /// </summary>
        /// <param name="value">The value of the language the modified REST API request will accept.</param>
        /// <returns>An accpet-language header parameter as a <see cref="HeaderParameter"/>.</returns>
        /// <remarks>This method always sets <c>replace</c> to <c>true</c>.</remarks>
        public static HeaderParameter Lang(String value)
        {
            return new HeaderParameter(true, LANG, value);
        }

        /// <summary>
        /// Creates an accept-language header parameter.
        /// </summary>
        /// <param name="value">The value of the locale the modified REST API request will accept.</param>
        /// <returns>An accpet-language header parameter as a <see cref="HeaderParameter"/>.</returns>
        /// <remarks>This method always sets <c>replace</c> to <c>true</c>.</remarks>
        public static HeaderParameter Locale(String value)
        {
            return new HeaderParameter(true, LOCALE, value);
        }

    }
}
