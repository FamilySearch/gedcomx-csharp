using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace Tavis
{
    public static class LinkExtensions
    {
        /// <summary>
        /// Add response handler to end of chain of handlers
        /// </summary>
        /// <param name="link"></param>
        /// <param name="responseHandler"></param>
        public static void AddHandler(this Link link, DelegatingResponseHandler responseHandler)
        {
           
            if (link.HttpResponseHandler == null)
            {
                link.HttpResponseHandler = responseHandler;
            }
            else
            {
                var currentHandler = link.HttpResponseHandler as DelegatingResponseHandler;
                if (currentHandler == null) throw new Exception("Cannot add handler unless existing handler is a delegating handler");
            
                while (currentHandler != null)
                {
                    if (currentHandler.InnerResponseHandler == null)
                    {
                        currentHandler.InnerResponseHandler = responseHandler;
                        currentHandler = null;
                    }
                    else
                    {
                        currentHandler = currentHandler.InnerResponseHandler;
                    } 
                }
            }
        }

        /// <summary>
        /// Serialize link in format that can be returned as a HttpHeader
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        public static string AsLinkHeader(this Link link)
        {
            var headerValue = new StringBuilder();
            headerValue.Append("<");
            headerValue.Append(link.Target.OriginalString);
            headerValue.Append(">");


            if (!String.IsNullOrEmpty(link.Relation))
            {
                headerValue.Append(";").AppendKey("rel").AppendQuotedString(link.Relation);
            }
            if (!String.IsNullOrEmpty(link.Anchor))
            {
                headerValue.Append(";").AppendKey("anchor").AppendQuotedString(link.Anchor);
            }
            if (!String.IsNullOrEmpty(link.Rev))
            {
                headerValue.Append(";").AppendKey("rev").AppendQuotedString(link.Rev);
            }
            foreach (var cultureInfo in link.HrefLang)
            {
                if (cultureInfo != null)
                {
                    headerValue.Append(";").AppendKey("hreflang").Append(cultureInfo.Name);
                }
            }

            if (!String.IsNullOrEmpty(link.Media))
            {
                headerValue.Append(";").AppendKey("media").AppendQuotedString(link.Media);
            }
            if (!String.IsNullOrEmpty(link.Title))
            {
                headerValue.Append(";").AppendKey("title").AppendQuotedString(link.Title);
            }
            if (link.Type != null)
            {
                headerValue.Append(";").AppendKey("type").AppendQuotedString(link.Type.MediaType);
            }

            foreach (var linkExtension in link.LinkExtensions)
            {
                headerValue.Append(";").AppendKey(linkExtension.Key).AppendQuotedString(linkExtension.Value);
            }
            return headerValue.ToString();
        }


        
    }
}
