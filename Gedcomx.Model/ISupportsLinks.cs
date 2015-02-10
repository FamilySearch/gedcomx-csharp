using Gx.Links;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gedcomx.Model.Util;

namespace Gedcomx.Model
{
    public interface ISupportsLinks
    {
        /// <summary>
        /// The list of hypermedia links. Links are not specified by GEDCOM X core, but as extension elements by GEDCOM X RS.
        /// </summary>
        /// <value>
        /// The list of hypermedia links. Links are not specified by GEDCOM X core, but as extension elements by GEDCOM X RS.
        /// </value>
        [JsonConverter(typeof(JsonHypermediaLinksConverter))]
        List<Link> Links { get; set; }

        /// <summary>
        /// Add a hypermedia link. Links are not specified by GEDCOM X core, but as extension elements by GEDCOM X RS.
        /// </summary>
        /// <param name="link">The hypermedia link. Links are not specified by GEDCOM X core, but as extension elements by GEDCOM X RS.</param>
        void AddLink(Link link);

        /// <summary>
        /// Add a hypermedia link.
        /// </summary>
        /// <param name="rel">The link rel.</param>
        /// <param name="href">The target URI.</param>
        void AddLink(String rel, Uri href);

        /// <summary>
        /// Add a templated link.
        /// </summary>
        /// <param name="rel">The link rel.</param>
        /// <param name="template">The link template.</param>
        void AddTemplatedLink(String rel, String template);

        /// <summary>
        /// Get a link by its rel. Links are not specified by GEDCOM X core, but as extension elements by GEDCOM X RS.
        /// </summary>
        /// <param name="rel">The link rel.</param>
        /// <returns>The link by rel.</returns>
        Link GetLink(String rel);

        /// <summary>
        /// Get a list of links by rel. Links are not specified by GEDCOM X core, but as extension elements by GEDCOM X RS.
        /// </summary>
        /// <param name="rel">The links by rel.</param>
        /// <returns></returns>
        List<Link> GetLinks(String rel);
    }
}
