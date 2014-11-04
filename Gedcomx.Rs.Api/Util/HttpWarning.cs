using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Gx.Rs.Api.Util
{
    /// <summary>
    /// Represents a model for "Warning" headers from REST API responses.
    /// </summary>
    public class HttpWarning
    {
        private static Regex regex = new Regex("\\w+[\\s]+\\w+[\\s]+\\\"[^\"]+\\\"", RegexOptions.Compiled);
        private readonly int? code;
        private readonly String application;
        private readonly String message;

        /// <summary>
        /// Parses the specified header and returns a collection of warning headers discovered and parsed.
        /// </summary>
        /// <param name="header">The header to parse.</param>
        /// <returns>A collection of warning headers that were discovered in the header and parsed.</returns>
        public static IEnumerable<HttpWarning> Parse(Parameter header)
        {
            if (header != null && header.Value != null)
            {
                foreach (Match match in regex.Matches(header.Value.ToString()))
                {
                    yield return HttpWarning.Parse(match.Value);
                }
            }
        }

        /// <summary>
        /// Parses the specified header value and returns the parsed warning header.
        /// </summary>
        /// <param name="headerValue">The header value to parse.</param>
        /// <returns>A warning header that was parsed.</returns>
        public static HttpWarning Parse(String headerValue)
        {
            int? code = null;
            String application = null;
            StringBuilder message = new StringBuilder();
            foreach (String token in headerValue.Split(' '))
            {
                if (code == null)
                {
                    try
                    {
                        code = int.Parse(token);
                    }
                    catch (FormatException)
                    {
                        code = -1;
                    }
                }
                else if (application == null)
                {
                    application = token;
                }
                else
                {
                    message.Append(' ').Append(token);
                }
            }

            code = code == null ? -1 : code;
            application = application == null ? "" : application;
            return new HttpWarning(code, application, message.ToString());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpWarning"/> class.
        /// </summary>
        /// <param name="code">The code to use in the warning.</param>
        /// <param name="application">The application originating the warning.</param>
        /// <param name="message">The warning message.</param>
        public HttpWarning(int? code, String application, String message)
        {
            this.code = code;
            this.application = application;
            this.message = message;
        }

        /// <summary>
        /// Gets the code for the warning.
        /// </summary>
        /// <value>
        /// The code for the warning.
        /// </value>
        public int? Code
        {
            get
            {
                return code;
            }
        }

        /// <summary>
        /// Gets the originating application for the warning.
        /// </summary>
        /// <value>
        /// The originating application for the warning.
        /// </value>
        public String Application
        {
            get
            {
                return application;
            }
        }

        /// <summary>
        /// Gets the warning message.
        /// </summary>
        /// <value>
        /// The warning message.
        /// </value>
        public String Message
        {
            get
            {
                return message;
            }
        }
    }
}
