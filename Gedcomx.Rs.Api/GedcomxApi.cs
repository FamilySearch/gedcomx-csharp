using System;
using Gx.Conclusion;

namespace Gx.Rs.Api
{
    /// <summary>
    /// The basic interface for a GEDCOM X API.
    /// </summary>
    public interface GedcomxApi
    {
        Uri BuildOAuth2AuthorizationUri(string clientId, string redirectUri);
        bool TryPasswordOAuth2Authentication(string username, string password, string clientId);
        bool TryPasswordOAuth2Authentication(string username, string password, string clientId, string clientSecret);
        bool TryAuthCodeOAuth2Authentication(string username, string password, string clientId);
        bool TryAuthCodeOAuth2Authentication(string username, string password, string clientId, string clientSecret);
        bool TryClientCredentialsOAuth2Authentication(string clientId, string clientSecret);
        GedcomxApiResponse<Person> GetPerson(String pid);
        //todo: fill in the methods as needed.
    }

    public class Rel
    {
        protected Rel() { }

        //Standard well-known RELs
        public static readonly String SELF = "self";
        public static readonly String NEXT = "next";
        public static readonly String PREV = "prev";
        public static readonly String PREVIOUS = PREV;
        public static readonly String FIRST = "first";
        public static readonly String LAST = "last";

        //GEDCOM-X specific rels.
        public static readonly String AGENT = "agent";
        public static readonly String ANCESTRY = "ancestry";
        public static readonly String ARTIFACTS = "artifacts";
        public static readonly String CHILDREN = "children";
        public static readonly String CHILD_RELATIONSHIPS = "child-relationships";
        public static readonly String COLLECTION = "collection";
        public static readonly String SUBCOLLECTIONS = "subcollections";
        public static readonly String CONCLUSION = "conclusion";
        public static readonly String CONCLUSIONS = "conclusions";
        public static readonly String CURRENT_USER_PERSON = "current-user-person";
        public static readonly String CURRENT_USER_RESOURCES = "current-user-resources";
        public static readonly String DESCENDANCY = "descendancy";
        public static readonly String DESCRIPTION = "description";
        public static readonly String EVIDENCE_REFERENCE = "evidence-reference";
        public static readonly String EVIDENCE_REFERENCES = "evidence-references";
        public static readonly String MATCHES = "matches";
        public static readonly String MEDIA_REFERENCE = "media-reference";
        public static readonly String MEDIA_REFERENCES = "media-references";
        public static readonly String NOTE = "note";
        public static readonly String NOTES = "notes";
        public static readonly String OAUTH2_AUTHORIZE = "http://oauth.net/core/2.0/endpoint/authorize";
        public static readonly String OAUTH2_TOKEN = "http://oauth.net/core/2.0/endpoint/token";
        public static readonly String PARENT_RELATIONSHIPS = "parent-relationships";
        public static readonly String PARENTS = "parents";
        public static readonly String PERSON1 = "person1";
        public static readonly String PERSON2 = "person2";
        public static readonly String PERSON = "person";
        public static readonly String PERSONS = "persons";
        public static readonly String PERSON_SEARCH = "person-search";
        public static readonly String PLACE = "place";
        public static readonly String PLACE_SEARCH = "place-search";
        public static readonly String PLACE_TYPE_GROUPS = "place-type-groups";
        public static readonly String PLACE_TYPE_GROUP = "place-type-group";
        public static readonly String PLACE_TYPES = "place-types";
        public static readonly String PLACE_TYPE = "place-type";
        public static readonly String PLACE_GROUP = "place-group";
        public static readonly String PLACE_DESCRIPTION = "place-description";
        public static readonly String PROFILE = "profile";
        public static readonly String RECORD = "record";
        public static readonly String RECORDS = "records";
        public static readonly String RELATIONSHIP = "relationship";
        public static readonly String RELATIONSHIPS = "relationships";
        public static readonly String SOURCE_DESCRIPTIONS = "source-descriptions";
        public static readonly String SOURCE_REFERENCE = "source-reference";
        public static readonly String SOURCE_REFERENCES = "source-references";
        public static readonly String SOURCE_REFERENCES_QUERY = "source-references-query";
        public static readonly String SPOUSES = "spouses";
        public static readonly String SPOUSE_RELATIONSHIPS = "spouse-relationships";
        public static readonly String DISCUSSION_REFERENCE = "discussion-reference";
        public static readonly String DISCUSSION_REFERENCES = "discussion-references";
    }

    public static class TemplateVariables
    {
        public const string PersonId = "pid";
        public const string ConclusionId = "cid";
        public const string CoupleRelationshipId = "crid";
        public const string SourceReferenceId = "srid";
        public const string NoteId = "nid";
        public const string CollectionId = "clid";
        public const string SourceDescriptionId = "sdid";
        public const string ArtifactId = "aid";
        public const string PersonaReferenceId = "prid";
    }

    public class MediaTypes
    {
        public static readonly String GEDCOMX_XML_MEDIA_TYPE = "application/x-gedcomx-v1+xml";
        public static readonly String GEDCOMX_JSON_MEDIA_TYPE = "application/x-gedcomx-v1+json";
        public static readonly String GEDCOMX_RECORDSET_XML_MEDIA_TYPE = "application/x-gedcomx-records-v1+xml";
        public static readonly String GEDCOMX_RECORDSET_JSON_MEDIA_TYPE = "application/x-gedcomx-records-v1+json";
        public static readonly String ATOM_XML_MEDIA_TYPE = "application/atom+xml";
        public static readonly String ATOM_GEDCOMX_JSON_MEDIA_TYPE = "application/x-gedcomx-atom+json";
        public static readonly String APPLICATION_OCTET_STREAM = "application/octet-stream";
        public static readonly String APPLICATION_JSON_TYPE = "application/json";
        public static readonly String APPLICATION_FORM_URLENCODED_TYPE = "application/x-www-form-urlencoded";
    }
}

