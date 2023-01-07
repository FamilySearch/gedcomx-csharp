using System;
using System.Collections.Generic;

using Gx.Links;

namespace Gx.Model.Collections
{
    /// <summary>
    ///  A list of <see cref="Link"/>.
    /// </summary>
    public class Links : List<Link>
    {
        /// <summary>
        /// Add a hypermedia link.
        /// </summary>
        /// <param name="rel">The link rel.</param>
        /// <param name="href">The target URI.</param>
        public void Add(string rel, Uri href) => Add(new Link(rel, href.ToString()));

        /// <summary>
        /// Add a templated link.
        /// </summary>
        /// <param name="rel">The link rel.</param>
        /// <param name="template">The link template.</param>
        public void Add(string rel, string template) => Add(new Link { Rel = rel, Template = template });
    }
}
