using Gx.Conclusion;
using Gx.Links;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gx.Rs.Api.Util
{
    /// <summary>
    /// This class extracts specific links from <see cref="P:Gedcomx.Persons"/> and <see cref="P:Gedcomx.Relationships"/>. Each of these are lists of instances
    /// that derive from <see cref="C:Gedcomx.Model.SupportsLinks"/>, thus the links are pulled directly from <see cref="P:SupportsLinks.Links"/>.
    /// </summary>
    /// <remarks>
    /// It is important to note that not all links will be extracted. Only the following will be extracted by default:
    /// <list type="ul">
    ///     <item>CHILD_RELATIONSHIPS</item>
    ///     <item>CONCLUSIONS</item>
    ///     <item>EVIDENCE_REFERENCES</item>
    ///     <item>MEDIA_REFERENCES</item>
    ///     <item>NOTES</item>
    ///     <item>PARENT_RELATIONSHIPS</item>
    ///     <item>SOURCE_REFERENCES</item>
    ///     <item>SPOUSE_RELATIONSHIPS</item>
    /// </list>
    /// </remarks>
    public class EmbeddedLinkLoader
    {
        /// <summary>
        /// The list of embedded links to extract by default. See remarks.
        /// </summary>
        /// <remarks>
        /// Only the following will be extracted by default:
        /// <list type="ul">
        ///     <item>CHILD_RELATIONSHIPS</item>
        ///     <item>CONCLUSIONS</item>
        ///     <item>EVIDENCE_REFERENCES</item>
        ///     <item>MEDIA_REFERENCES</item>
        ///     <item>NOTES</item>
        ///     <item>PARENT_RELATIONSHIPS</item>
        ///     <item>SOURCE_REFERENCES</item>
        ///     <item>SPOUSE_RELATIONSHIPS</item>
        /// </list>
        /// </remarks>
        public static readonly ISet<String> DEFAULT_EMBEDDED_LINK_RELS = new HashSet<String>()
        {
            Rel.CHILD_RELATIONSHIPS,
            Rel.CONCLUSIONS,
            Rel.EVIDENCE_REFERENCES,
            Rel.MEDIA_REFERENCES,
            Rel.NOTES,
            Rel.PARENT_RELATIONSHIPS,
            Rel.SOURCE_REFERENCES,
            Rel.SPOUSE_RELATIONSHIPS,
        };

        /// <summary>
        /// Gets the list of embedded links that will be extracted when <see cref="LoadEmbeddedLinks"/> is called.
        /// </summary>
        /// <value>
        /// The list of embedded links that will be extracted when <see cref="LoadEmbeddedLinks"/> is called.
        /// </value>
        protected ISet<String> EmbeddedLinkRels
        {
            get
            {
                return DEFAULT_EMBEDDED_LINK_RELS;
            }
        }

        /// <summary>
        /// Loads the list of links as specified in <see cref="EmbeddedLinkRels" /> from the specified <see cref="Gedcomx" />.
        /// </summary>
        /// <param name="entity">The <see cref="Gedcomx" /> entity from which links will be extracted. See remarks.</param>
        /// <returns></returns>
        /// <remarks>
        /// Not all objects with <see cref="C:Gedcomx.Model.SupportsLinks" /> will be evaluated. Only <see cref="P:Gedcomx.Perons" /> and
        /// <see cref="P:Gedcomx.Relationships" /> will be considered.
        /// </remarks>
        public IList<Link> LoadEmbeddedLinks(Gedcomx entity)
        {
            List<Link> embeddedLinks = new List<Link>();
            ISet<String> embeddedRels = EmbeddedLinkRels;

            List<Person> persons = entity.Persons;
            if (persons != null)
            {
                foreach (Person person in persons)
                {
                    foreach (String embeddedRel in embeddedRels)
                    {
                        Link link = person.Links.FirstOrDefault(x => x.Rel == embeddedRel);
                        if (link != null)
                        {
                            embeddedLinks.Add(link);
                        }
                    }
                }
            }

            List<Relationship> relationships = entity.Relationships;
            if (relationships != null)
            {
                foreach (Relationship relationship in relationships)
                {
                    foreach (String embeddedRel in embeddedRels)
                    {
                        Link link = relationship.Links.FirstOrDefault(x => x.Rel == embeddedRel);
                        if (link != null)
                        {
                            embeddedLinks.Add(link);
                        }
                    }
                }
            }

            return embeddedLinks;
        }
    }
}
