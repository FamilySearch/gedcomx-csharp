using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Tavis.IANA;

namespace Tavis
{
    /// <summary>
    /// 
    /// </summary>
    public class LinkFactory
    {
        private readonly Dictionary<string, LinkRegistration>  _LinkRegistry = new Dictionary<string, LinkRegistration>(StringComparer.OrdinalIgnoreCase);


        /// <summary>
        /// 
        /// </summary>
        public HintFactory HintFactory { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public LinkFactory()
        {
            // Register all official IANA link types
            AddLinkType<AboutLink>();
            AddLinkType<AlternateLink>();
            AddLinkType<AppendixLink>();
            AddLinkType<ArchivesLink>();
            AddLinkType<AuthorLink>();
            AddLinkType<BookmarkLink>();
            AddLinkType<CanonicalLink>();
            AddLinkType<ChapterLink>();
            AddLinkType<CollectionLink>();
            AddLinkType<ContentsLink>();
            AddLinkType<CopyrightLink>();
            AddLinkType<CreateFormLink>();
            AddLinkType<CurrentLink>();
            AddLinkType<DescribedByLink>();
            AddLinkType<DescribesLink>();
            AddLinkType<DisclosureLink>();
            AddLinkType<DuplicateLink>();
            AddLinkType<EditLink>();
            AddLinkType<EditFormLink>();
            AddLinkType<EnclosureLink>();
            AddLinkType<FirstLink>();
            AddLinkType<GlossaryLink>();
            AddLinkType<HelpLink>();
            AddLinkType<HostsLink>();
            AddLinkType<HubLink>();
            AddLinkType<IconLink>();
            AddLinkType<IndexLink>();
            AddLinkType<ItemLink>();
            AddLinkType<LastLink>();
            AddLinkType<LatestVersionLink>();
            AddLinkType<LicenseLink>();
            AddLinkType<LrddLink>();
            AddLinkType<MonitorLink>();
            AddLinkType<MonitorGroupLink>();
            AddLinkType<NextLink>();
            AddLinkType<NextArchiveLink>();
            AddLinkType<NoFollowLink>();
            AddLinkType<NoReferrerLink>();
            AddLinkType<PaymentLink>();
            AddLinkType<PredecessorVersionLink>();
            AddLinkType<PrefetchLink>();
            AddLinkType<PrevLink>();
            AddLinkType<PreviewLink>();
            AddLinkType<PreviousLink>();
            AddLinkType<PrevArchiveLink>();
            AddLinkType<PrivacyPolicyLink>();
            AddLinkType<ProfileLink>();
            AddLinkType<RelatedLink>();
            AddLinkType<RepliesLink>();
            AddLinkType<SearchLink>();
            AddLinkType<SectionLink>();
            AddLinkType<SelfLink>();
            AddLinkType<ServiceLink>();
            AddLinkType<StartLink>();
            AddLinkType<StylesheetLink>();
            AddLinkType<SubSectionLink>();
            AddLinkType<SuccessorVersionLink>();
            AddLinkType<TagLink>();
            AddLinkType<TermsOfServiceLink>();
            AddLinkType<TypeLink>();
            AddLinkType<UpLink>();
            AddLinkType<VersionHistoryLink>();
            AddLinkType<ViaLink>();
            AddLinkType<WorkingCopyLink>();
            AddLinkType<WorkingCopyOfLink>();

            HintFactory = new HintFactory();  // Default hintfactory

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void AddLinkType<T>() where T : Link, new()
        {
            var t = new T();
            _LinkRegistry.Add(t.Relation, new LinkRegistration() {LinkType =typeof(T) } ); 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        public void SetHandler<T>(IHttpResponseHandler handler) where T : Link, new()
        {
            var t = new T();
            var reg = _LinkRegistry[t.Relation];
            reg.ResponseHandler = handler;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="relation"></param>
        /// <returns></returns>
        public Link CreateLink(string relation)
        {
            if (!_LinkRegistry.ContainsKey(relation))
            {
                return new Link()
                    {
                        Relation = relation
                    };
            }
            var reg = _LinkRegistry[relation];
            var t = Activator.CreateInstance(reg.LinkType) as Link;
            t.HttpResponseHandler = reg.ResponseHandler;
            return t;

        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T CreateLink<T>() where T : Link, new()
        {
            var t = new T();
            var reg = _LinkRegistry[t.Relation];
            t.HttpResponseHandler = reg.ResponseHandler;
            return t;

        }
    }

    internal class LinkRegistration
    {
        public Type LinkType { get; set; }
        public IHttpResponseHandler ResponseHandler { get; set; }

    }

   
}
