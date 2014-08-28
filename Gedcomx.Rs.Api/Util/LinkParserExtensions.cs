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

        //public static IEnumerable<LinkHeader> ParseLinkHeader(String value)
        //{
        //    if (!String.IsNullOrEmpty(value))
        //    {
        //        foreach (var link in value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        //        {
        //            LinkHeader result = new LinkHeader();

        //            foreach (var param in link.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()))
        //            {
        //                if (param.StartsWith("<") && param.EndsWith(">"))
        //                {
        //                    Uri href;

        //                    if (Uri.TryCreate(param.Substring(1, param.Length - 2), UriKind.RelativeOrAbsolute, out href))
        //                    {
        //                        result.Href = href;
        //                    }
        //                }
        //                else
        //                {
        //                    String[] kvp = param.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

        //                    if (kvp.Length == 2)
        //                    {
        //                        String paramValue = GetParamValue(kvp[1]);
        //                        switch (kvp[0])
        //                        {
        //                            case "rel":
        //                                if (String.IsNullOrEmpty(result.Rel)) result.Rel = paramValue;
        //                                break;
        //                            case "template":
        //                                if (result.Template == null) result.Template = paramValue;
        //                                break;
        //                            case "title":
        //                                if (String.IsNullOrEmpty(result.Title)) result.Title = paramValue;
        //                                break;
        //                            case "accept":
        //                                if (String.IsNullOrEmpty(result.Accept)) result.Accept = paramValue;
        //                                break;
        //                            case "allow":
        //                                if (String.IsNullOrEmpty(result.Allow)) result.Allow = paramValue;
        //                                break;
        //                            case "hreflang":
        //                                if (String.IsNullOrEmpty(result.HrefLang)) result.HrefLang = paramValue;
        //                                break;
        //                            case "type":
        //                                if (String.IsNullOrEmpty(result.Type)) result.Type = paramValue;
        //                                break;
        //                        }
        //                    }
        //                }
        //            }

        //            if (result.Href != null)
        //            {
        //                yield return result;
        //            }
        //        }
        //    }
        //}

        //private static String GetParamValue(String value)
        //{
        //    String result = value;

        //    if (!String.IsNullOrEmpty(value) && value.StartsWith("\"") && value.EndsWith("\""))
        //    {
        //        result = value.Substring(1, value.Length - 2);
        //    }

        //    return result;
        //}
    }
}
