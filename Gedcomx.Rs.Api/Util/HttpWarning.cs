using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Gx.Rs.Api.Util
{
    public class HttpWarning
    {
        private static Regex regex = new Regex("\\w+[\\s]+\\w+[\\s]+\\\"[^\"]+\\\"", RegexOptions.Compiled);
        private readonly int? code;
        private readonly String application;
        private readonly String message;

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

        public HttpWarning(int? code, String application, String message)
        {
            this.code = code;
            this.application = application;
            this.message = message;
        }

        public int? Code
        {
            get
            {
                return code;
            }
        }

        public String Application
        {
            get
            {
                return application;
            }
        }

        public String Message
        {
            get
            {
                return message;
            }
        }
    }
}
