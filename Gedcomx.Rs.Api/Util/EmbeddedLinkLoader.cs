using Gx.Conclusion;
using Gx.Links;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gx.Rs.Api.Util
{
    public class EmbeddedLinkLoader
    {
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

        protected ISet<String> EmbeddedLinkRels
        {
            get
            {
                return DEFAULT_EMBEDDED_LINK_RELS;
            }
        }

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
