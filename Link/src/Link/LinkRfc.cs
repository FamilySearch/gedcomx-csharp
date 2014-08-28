using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;

namespace Tavis
{
    /// <summary>
    /// Link class with all properties as defined by RFC 5788
    /// http://tools.ietf.org/html/rfc5988
    /// </summary>
    public class LinkRfc
    {
        /// <summary>
        /// The URI of the resource that returned the representation that contained this link 
        /// </summary>
        public Uri Context { get; set; }

        /// <summary>
        /// The URI of resource that this link is pointing to
        /// </summary>
        public Uri Target { get;  set; }

        /// <summary>
        /// A string identify for the Link Relation Type
        /// </summary>
        public string Relation { get; set; }
      
        /// <summary>
        /// An identifier that further qualifies the Context of the link within the current representation  
        /// </summary>
        public string Anchor { get;  set; }

        /// <summary>
        /// Reverse link relation
        /// </summary>
        /// <remarks>
        /// Deprecated byRFC5988
        /// </remarks>
        public string Rev { get;  set; }
  
        /// <summary>
        /// Human readable description of the purpose of the link
        /// </summary>
        public string Title { get;  set; }

        /// <summary>
        /// Human readable description of the purpose of the link, with support for extended character sets 
        /// </summary>
        /// <remarks>
        /// See RFC 5987 for details
        /// </remarks>
        public Encoding TitleEncoding { get; set; }  

        /// <summary>
        /// Set of languages supported by the target resource
        /// </summary>
        public List<CultureInfo> HrefLang { get;  private set; }  

        /// <summary>
        /// Identifier to describe the type of device that the target representation will be rendered on.
        /// </summary>
        public string Media { get;  set; }

        /// <summary>
        /// Hint to indicate what media type might be returned by the target resource
        /// </summary>
        public MediaTypeHeaderValue Type { get;  set; }

        /// <summary>
        /// Retrieve extension attribute from link
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetLinkExtension(string name)
        {
            return _LinkExtensions[name];
        }

        /// <summary>
        /// Set extension attribute on link
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetLinkExtension(string name, string value)
        {
            _LinkExtensions[name] = value;
        }

        /// <summary>
        /// Returns a list of extension attributes assigned to the link
        /// </summary>
        public IEnumerable<KeyValuePair<string, string>> LinkExtensions { get { return _LinkExtensions; } } 

        /// <summary>
        /// Create an instance of a link
        /// </summary>
        public LinkRfc() {
            TitleEncoding = Encoding.UTF8;  // Should be ASCII but PCL does not support ascii and UTF8 does not change ASCII values 
            HrefLang = new List<CultureInfo>();
        }

        protected readonly Dictionary<string, string> _LinkExtensions = new Dictionary<string, string>();

    }
}