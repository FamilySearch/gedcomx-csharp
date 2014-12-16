using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gx.Rs.Api
{
    /// <summary>
    /// A collection of rel links to assist in looking up resource links. See remarks for additional information.
    /// </summary>
    /// <remarks>
    /// When a resource is consumed, it typically returns a set of hypermedia links that enable additional actions on the resource. While
    /// resources typically provide links, not all links will be available on a given resource (such as paging links on a person resource).
    /// The links exposed in this class are a set of predefined constants, which can be used to determine if a link is available on a
    /// given resource.
    /// </remarks>
    public class Rel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Rel"/> class.
        /// </summary>
        protected Rel() { }

        /****************************
         * Standard well-known RELs *
         ****************************/
        /// <summary>
        /// The link that points to oneself.
        /// </summary>
        public static readonly String SELF = "self";
        /// <summary>
        /// A link that points to the next item, such as in a collection or result set.
        /// </summary>
        public static readonly String NEXT = "next";
        /// <summary>
        /// A link that points to the previous item, such as in a collection or result set.
        /// </summary>
        public static readonly String PREV = "prev";
        /// <summary>
        /// A link that points to the previous item, such as in a collection or result set.
        /// </summary>
        public static readonly String PREVIOUS = PREV;
        /// <summary>
        /// A link that points to the first item, such as in a collection or result set.
        /// </summary>
        public static readonly String FIRST = "first";
        /// <summary>
        /// A link that points to the last item, such as in a collection or result set.
        /// </summary>
        public static readonly String LAST = "last";

        /**************************
         * GEDCOM-X specific rels *
         **************************/
        /// <summary>
        /// A link that points to the agent resource.
        /// </summary>
        public static readonly String AGENT = "agent";
        /// <summary>
        /// A link that points to the ancestry resource.
        /// </summary>
        public static readonly String ANCESTRY = "ancestry";
        /// <summary>
        /// A link that points to the artifacts resource.
        /// </summary>
        public static readonly String ARTIFACTS = "artifacts";
        /// <summary>
        /// A link that points to the children resource.
        /// </summary>
        public static readonly String CHILDREN = "children";
        /// <summary>
        /// A link that points to the child relationships resource.
        /// </summary>
        public static readonly String CHILD_RELATIONSHIPS = "child-relationships";
        /// <summary>
        /// A link that points to the collection resource.
        /// </summary>
        public static readonly String COLLECTION = "collection";
        /// <summary>
        /// A link that points to the subcollections resource.
        /// </summary>
        public static readonly String SUBCOLLECTIONS = "subcollections";
        /// <summary>
        /// A link that points to the conclusion resource.
        /// </summary>
        public static readonly String CONCLUSION = "conclusion";
        /// <summary>
        /// A link that points to the conclusions resource.
        /// </summary>
        public static readonly String CONCLUSIONS = "conclusions";
        /// <summary>
        /// A link that points to the current user person resource.
        /// </summary>
        public static readonly String CURRENT_USER_PERSON = "current-user-person";
        /// <summary>
        /// A link that points to the current user resources resource.
        /// </summary>
        public static readonly String CURRENT_USER_RESOURCES = "current-user-resources";
        /// <summary>
        /// A link that points to the descendancy resource.
        /// </summary>
        public static readonly String DESCENDANCY = "descendancy";
        /// <summary>
        /// A link that points to the description resource.
        /// </summary>
        public static readonly String DESCRIPTION = "description";
        /// <summary>
        /// A link that points to the evidence reference resource.
        /// </summary>
        public static readonly String EVIDENCE_REFERENCE = "evidence-reference";
        /// <summary>
        /// A link that points to the evidence references resource.
        /// </summary>
        public static readonly String EVIDENCE_REFERENCES = "evidence-references";
        /// <summary>
        /// A link that points to the matches resource.
        /// </summary>
        public static readonly String MATCHES = "matches";
        /// <summary>
        /// A link that points to the media reference resource.
        /// </summary>
        public static readonly String MEDIA_REFERENCE = "media-reference";
        /// <summary>
        /// A link that points to the media references resource.
        /// </summary>
        public static readonly String MEDIA_REFERENCES = "media-references";
        /// <summary>
        /// A link that points to the note resource.
        /// </summary>
        public static readonly String NOTE = "note";
        /// <summary>
        /// A link that points to the notes resource.
        /// </summary>
        public static readonly String NOTES = "notes";
        /// <summary>
        /// A link that points to the OAuth2 authorization resource.
        /// </summary>
        public static readonly String OAUTH2_AUTHORIZE = "http://oauth.net/core/2.0/endpoint/authorize";
        /// <summary>
        /// A link that points to the OAuth2 token resource.
        /// </summary>
        public static readonly String OAUTH2_TOKEN = "http://oauth.net/core/2.0/endpoint/token";
        /// <summary>
        /// A link that points to the parent relationships resource.
        /// </summary>
        public static readonly String PARENT_RELATIONSHIPS = "parent-relationships";
        /// <summary>
        /// A link that points to the parents resource.
        /// </summary>
        public static readonly String PARENTS = "parents";
        /// <summary>
        /// A link that points to the person1 resource.
        /// </summary>
        public static readonly String PERSON1 = "person1";
        /// <summary>
        /// A link that points to the person2 resource.
        /// </summary>
        public static readonly String PERSON2 = "person2";
        /// <summary>
        /// A link that points to the person resource.
        /// </summary>
        public static readonly String PERSON = "person";
        /// <summary>
        /// A link that points to the persons resource.
        /// </summary>
        public static readonly String PERSONS = "persons";
        /// <summary>
        /// A link that points to the person search resource.
        /// </summary>
        public static readonly String PERSON_SEARCH = "person-search";
        /// <summary>
        /// A link that points to the place resource.
        /// </summary>
        public static readonly String PLACE = "place";
        /// <summary>
        /// A link that points to the place search resource.
        /// </summary>
        public static readonly String PLACE_SEARCH = "place-search";
        /// <summary>
        /// The A link that points to the place type groups resource.
        /// </summary>
        public static readonly String PLACE_TYPE_GROUPS = "place-type-groups";
        /// <summary>
        /// A link that points to the place type group resource.
        /// </summary>
        public static readonly String PLACE_TYPE_GROUP = "place-type-group";
        /// <summary>
        /// A link that points to the place types resource.
        /// </summary>
        public static readonly String PLACE_TYPES = "place-types";
        /// <summary>
        /// A link that points to the place type resource.
        /// </summary>
        public static readonly String PLACE_TYPE = "place-type";
        /// <summary>
        /// A link that points to the place group resource.
        /// </summary>
        public static readonly String PLACE_GROUP = "place-group";
        /// <summary>
        /// A link that points to the place description resource.
        /// </summary>
        public static readonly String PLACE_DESCRIPTION = "place-description";
        /// <summary>
        /// A link that points to the profile resource.
        /// </summary>
        public static readonly String PROFILE = "profile";
        /// <summary>
        /// A link that points to the record resource.
        /// </summary>
        public static readonly String RECORD = "record";
        /// <summary>
        /// A link that points to the records resource.
        /// </summary>
        public static readonly String RECORDS = "records";
        /// <summary>
        /// A link that points to the relationship resource.
        /// </summary>
        public static readonly String RELATIONSHIP = "relationship";
        /// <summary>
        /// A link that points to the relationships resource.
        /// </summary>
        public static readonly String RELATIONSHIPS = "relationships";
        /// <summary>
        /// A link that points to the source descriptions resource.
        /// </summary>
        public static readonly String SOURCE_DESCRIPTIONS = "source-descriptions";
        /// <summary>
        /// A link that points to the source reference resource.
        /// </summary>
        public static readonly String SOURCE_REFERENCE = "source-reference";
        /// <summary>
        /// A link that points to the source references resource.
        /// </summary>
        public static readonly String SOURCE_REFERENCES = "source-references";
        /// <summary>
        /// A link that points to the source references query resource.
        /// </summary>
        public static readonly String SOURCE_REFERENCES_QUERY = "source-references-query";
        /// <summary>
        /// A link that points to the spouses resource.
        /// </summary>
        public static readonly String SPOUSES = "spouses";
        /// <summary>
        /// A link that points to the spouse relationships resource.
        /// </summary>
        public static readonly String SPOUSE_RELATIONSHIPS = "spouse-relationships";
        /// <summary>
        /// A link that points to the discussion reference resource.
        /// </summary>
        public static readonly String DISCUSSION_REFERENCE = "discussion-reference";
        /// <summary>
        /// A link that points to the discussion references resource.
        /// </summary>
        public static readonly String DISCUSSION_REFERENCES = "discussion-references";
    }
}
