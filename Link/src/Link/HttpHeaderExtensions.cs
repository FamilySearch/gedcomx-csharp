using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Tavis
{
    public static class HttpHeaderExtensions
    {
        
        public static void AddLinkHeader(this HttpHeaders headers, Link link )
        {
            var headerValue = link.AsLinkHeader();
            headers.Add("Link", headerValue);
        }

        public static void AddLinkHeaders(this HttpHeaders headers, List<Link> links)
        {
            string headerValue = string.Empty;
            foreach (var link in links)
            {
                headerValue += link.AsLinkHeader();
                headerValue += ", ";
            }

            headerValue = headerValue.Substring(0, headerValue.Length - 2);

            headers.Add("Link", headerValue);
        }

        
        
        public static List<Link> ParseLinkHeaders(this HttpResponseMessage responseMessage, LinkFactory linkRegistry)
        {
            return ParseLinkHeaders(responseMessage.Headers, responseMessage.RequestMessage.RequestUri, linkRegistry);
        }

        public static List<Link> ParseLinkHeaders(this HttpHeaders headers, Uri baseUri, LinkFactory linkRegistry)
        {
            var list = new List<Link>();
            var parser = new LinkHeaderParser(linkRegistry);
            var linkHeaders = headers.GetValues("Link");
            foreach (var linkHeader in linkHeaders)
            {
                list.AddRange(parser.Parse(baseUri, linkHeader));
            }
            return list;
        }

        public static IList<Link> ParseLinkHeader(this Link link, string linkHeader, LinkFactory linkRegistry)
        {
            var parser = new LinkHeaderParser(linkRegistry);
            return parser.Parse(link.Target, linkHeader);
        }


    }
}
