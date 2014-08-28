using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Tavis.IANA
{
    [LinkRelationType("about")]
    public class AboutLink : Link
    {
    }

    [LinkRelationType("alternate")]
    public class AlternateLink : Link
    {
    }

    [LinkRelationType("appendix")]
    public class AppendixLink : Link
    {
    }

    [LinkRelationType("archives")]
    public class ArchivesLink : Link
    {
    }

    [LinkRelationType("author")]
    public class AuthorLink : Link
    {
    }

    [LinkRelationType("bookmark")]
    public class BookmarkLink : Link
    {
    }

    [LinkRelationType("canonical")]
    public class CanonicalLink : Link
    {
    }

    [LinkRelationType("chapter")]
    public class ChapterLink : Link
    {
    }

    [LinkRelationType("collection")]
    public class CollectionLink : Link
    {
    }

    [LinkRelationType("contents")]
    public class ContentsLink : Link
    {
    }

    [LinkRelationType("copyright")]
    public class CopyrightLink : Link
    {
    }

    [LinkRelationType("create-form")]
    public class CreateFormLink : Link
    {
    }

    [LinkRelationType("current")]
    public class CurrentLink : Link
    {
    }

    [LinkRelationType("describedby")]
    public class DescribedByLink : Link
    {
    }

    [LinkRelationType("describes")]
    public class DescribesLink : Link
    {
    }

    [LinkRelationType("disclosure")]
    public class DisclosureLink : Link
    {
    }

    [LinkRelationType("duplicate")]
    public class DuplicateLink : Link
    {
    }

    [LinkRelationType("edit")]
    public class EditLink : Link
    {
    }

    [LinkRelationType("edit-form")]
    public class EditFormLink : Link
    {
    }

    [LinkRelationType("edit-media")]
    public class EditMediaLink : Link
    {
    }

    [LinkRelationType("enclosure")]
    public class EnclosureLink : Link
    {
    }

    [LinkRelationType("first")]
    public class FirstLink : Link
    {
    }

    [LinkRelationType("glossary")]
    public class GlossaryLink : Link
    {
    }

    [LinkRelationType("help")]
    public class HelpLink : Link
    {
    }

    [LinkRelationType("hosts")]
    public class HostsLink : Link
    {
    }

    [LinkRelationType("hub")]
    public class HubLink : Link
    {
        public enum SubscribeMode
        {
            subscribe,
            unsubscribe
        };

        public Uri Callback { get; set; }
        public SubscribeMode Mode  { get; set; }
        public Uri Topic { get; set; } 
        public TimeSpan Lease { get; set; }
        public string Secret { get; set; }

        public HubLink()
        {
            Method = HttpMethod.Post;
            Mode = SubscribeMode.subscribe;
        }

        public override HttpRequestMessage CreateRequest()
        {
            var bodyParameters = new Dictionary<string, string>()
            {
                {"hub.callback",Callback.OriginalString},
                {"hub.mode", Mode.ToString()},
                {"hub.topic", Topic.OriginalString},
            };
            
            if (Lease.TotalSeconds > 0) bodyParameters.Add("hub.lease_seconds",Lease.TotalSeconds.ToString());
            if (!String.IsNullOrEmpty(Secret)) bodyParameters.Add("hub.secret", Secret);

            var request = base.CreateRequest();

            request.Content = new FormUrlEncodedContent(bodyParameters);
            return request;
        }

        public static bool IsSubscribed(HttpResponseMessage response)
        {
            return response.StatusCode == HttpStatusCode.Accepted;
        }
    }

    [LinkRelationType("icon")]
    public class IconLink : Link
    {

    }

    [LinkRelationType("index")]
    public class IndexLink : Link
    {
    }

    [LinkRelationType("item")]
    public class ItemLink : Link
    {
    }

    [LinkRelationType("last")]
    public class LastLink : Link
    {
    }

    [LinkRelationType("latest-version")]
    public class LatestVersionLink : Link
    {
    }

    [LinkRelationType("license")]
    public class LicenseLink : Link
    {
    }

    [LinkRelationType("lrdd")]
    public class LrddLink : Link
    {
    }

    [LinkRelationType("monitor")]
    public class MonitorLink : Link
    {
    }

    [LinkRelationType("monitor-group")]
    public class MonitorGroupLink : Link
    {
    }

    [LinkRelationType("next")]
    public class NextLink : Link
    {
    }

    [LinkRelationType("next-archive")]
    public class NextArchiveLink : Link
    {
    }

    [LinkRelationType("nofollow")]
    public class NoFollowLink : Link
    {
    }

    [LinkRelationType("noreferrer")]
    public class NoReferrerLink : Link
    {
    }

    [LinkRelationType("payment")]
    public class PaymentLink : Link
    {
    }

    [LinkRelationType("predecessor-version")]
    public class PredecessorVersionLink : Link
    {
    }

    [LinkRelationType("prefetch")]
    public class PrefetchLink : Link
    {
    }

    [LinkRelationType("prev")]
    public class PrevLink : Link
    {
    }

    [LinkRelationType("preview")]
    public class PreviewLink : Link
    {
    }

    [LinkRelationType("previous")]
    public class PreviousLink : Link
    {
    }

    [LinkRelationType("prev-archive")]
    public class PrevArchiveLink : Link
    {
    }

    [LinkRelationType("privacy-policy")]
    public class PrivacyPolicyLink : Link
    {
    }

    [LinkRelationType("profile")]
    public class ProfileLink : Link
    {
    }

    [LinkRelationType("related")]
    public class RelatedLink : Link
    {
    }

    /// <summary>
    /// Identifies a resource that is a reply to the context of the link. 
    /// </summary>
    [LinkRelationType("replies")]
    public class RepliesLink : Link
    {
    }

    /// <summary>
    /// Refers to a resource that can be used to search through the link's context and related resources.
    /// </summary>

    [LinkRelationType("search")]
    public class SearchLink : Link
    {
    }

    /// <summary>
    /// Refers to a section in a collection of resources.
    /// </summary>
    [LinkRelationType("section")]
    public class SectionLink : Link
    {
    }

    /// <summary>
    /// Conveys an identifier for the link's context. 
    /// </summary>
    [LinkRelationType("self")]
    public class SelfLink : Link
    {
    }

    /// <summary>
    /// Indicates a URI that can be used to retrieve a service document.
    /// </summary>
    [LinkRelationType("service")]
    public class ServiceLink : Link
    {
    }

    /// <summary>
    /// Refers to the first resource in a collection of resources.
    /// </summary>
    [LinkRelationType("start")]
    public class StartLink : Link
    {
    }

    /// <summary>
    /// Refers to a stylesheet.
    /// </summary>
    [LinkRelationType("stylesheet")]
    public class StylesheetLink : Link
    {
    }

    /// <summary>
    /// Refers to a resource serving as a subsection in a collection of resources.
    /// </summary>
    [LinkRelationType("subsection")]
    public class SubSectionLink : Link
    {
    }


    /// <summary>
    /// Points to a resource containing the successor version in the version history. 
    /// </summary>
    [LinkRelationType("successor-version")]
    public class SuccessorVersionLink : Link
    {
    }


    /// <summary>
    /// Gives a tag (identified by the given address) that applies to the current document. 
    /// </summary>
    [LinkRelationType("tag")]
    public class TagLink : Link
    {
    }

    /// <summary>
    /// Refers to the terms of service associated with the link's context.
    /// </summary>
    [LinkRelationType("terms-of-service")]
    public class TermsOfServiceLink : Link
    {
    }

    /// <summary>
    /// Refers to a resource identifying the abstract semantic type of which the link's context is considered to be an instance.
    /// </summary>
    [LinkRelationType("type")]
    public class TypeLink : Link
    {
    }

    /// <summary>
    /// Refers to a parent document in a hierarchy of documents. 
    /// </summary>
    [LinkRelationType("up")]
    public class UpLink : Link
    {
    }

    /// <summary>
    /// Points to a resource containing the version history for the context. 
    /// </summary>
    [LinkRelationType("version-history")]
    public class VersionHistoryLink : Link
    {
    }

    /// <summary>
    /// Identifies a resource that is the source of the information in the link's context. 
    /// </summary>
    [LinkRelationType("via")]
    public class ViaLink : Link
    {
    }

    /// <summary>
    /// Points to a working copy for this resource.
    /// </summary>
    [LinkRelationType("working-copy")]
    public class WorkingCopyLink : Link
    {
    }

    /// <summary>
    /// Points to the versioned resource from which this working copy was obtained. 
    /// </summary>
    [LinkRelationType("working-copy-of")]
    public class WorkingCopyOfLink : Link
    {
    }
}


