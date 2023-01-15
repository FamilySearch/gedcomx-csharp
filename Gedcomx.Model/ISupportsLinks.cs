using System;
using System.Collections.Generic;

using Gedcomx.Model.Util;

using Gx.Links;

using Newtonsoft.Json;

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
        Gx.Model.Collections.Links Links { get; set; }

        /// <summary>
        /// Determines whether a sequence exists and contains any links.
        /// </summary>
        /// <returns></returns>
        bool AnyLinks();

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
