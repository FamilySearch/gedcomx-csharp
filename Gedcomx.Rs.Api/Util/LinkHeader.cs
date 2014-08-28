using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tavis.UriTemplates;

namespace Gx.Rs.Api.Util
{
    public class LinkHeader
    {
        public Uri Href { get; set; }
        public string Rel { get; set; }
        public string Template { get; set; }
        public string Title { get; set; }
        public string Accept { get; set; }
        public string Allow { get; set; }
        public string HrefLang { get; set; }
        public string Type { get; set; }
    }
}
