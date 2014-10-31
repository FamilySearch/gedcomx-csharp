using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilySearch.Api
{
    /// <summary>
    /// A collection of rel links to assist in looking up resource links. See remarks for additional information.
    /// </summary>
    /// <remarks>
    /// When a resource is consumed, it typcially returns a set of hypermedia links that enable additional actions on the resource. While
    /// resources typically provide links, not all links will be available on a given resource (such as paging links on a person resource).
    /// The links exposed in this class are a set of predefined constants, which can be used to determine if a link is available on a
    /// given resource.
    /// </remarks>
    public class Rel : Gx.Rs.Api.Rel
    {
        /// <summary>
        /// A link that points to the change history resource.
        /// </summary>
        public static readonly String CHANGE_HISTORY = "change-history";
        /// <summary>
        /// A link that points to the comment resource.
        /// </summary>
        public static readonly String COMMENT = "comment";
        /// <summary>
        /// A link that points to the comments resource.
        /// </summary>
        public static readonly String COMMENTS = "comments";
        /// <summary>
        /// A link that points to the current user resource.
        /// </summary>
        public static readonly String CURRENT_USER = "current-user";
        /// <summary>
        /// A link that points to the discussions resource.
        /// </summary>
        public static readonly String DISCUSSIONS = "discussions";
        /// <summary>
        /// A link that points to the merge resource.
        /// </summary>
        public static readonly String MERGE = "merge";
        /// <summary>
        /// A link that points to the merge mirror resource.
        /// </summary>
        public static readonly String MERGE_MIRROR = "merge-mirror";
        /// <summary>
        /// A link that points to the normalized date resource.
        /// </summary>
        public static readonly String NORMALIZED_DATE = "normalized-date";
        /// <summary>
        /// A link that points to the not a matches resource.
        /// </summary>
        public static readonly String NOT_A_MATCHES = "non-matches";
        /// <summary>
        /// A link that points to the not a match resource.
        /// </summary>
        public static readonly String NOT_A_MATCH = "non-match";
        /// <summary>
        /// A link that points to the person matches query resource.
        /// </summary>
        public static readonly String PERSON_MATCHES_QUERY = "person-matches-query";
        /// <summary>
        /// A link that points to the portrait resource.
        /// </summary>
        public static readonly String PORTRAIT = "portrait";
        /// <summary>
        /// A link that points to the portraits resource.
        /// </summary>
        public static readonly String PORTRAITS = "portraits";
        /// <summary>
        /// A link that points to the retore resource.
        /// </summary>
        public static readonly String RESTORE = "restore";
        /// <summary>
        /// A link that points to the father role resource.
        /// </summary>
        public static readonly String FATHER_ROLE = "father-role";
        /// <summary>
        /// A link that points to the mother role resource.
        /// </summary>
        public static readonly String MOTHER_ROLE = "mother-role";
        /// <summary>
        /// A link that points to the person with relationships resource.
        /// </summary>
        public static readonly String PERSON_WITH_RELATIONSHIPS = "person-with-relationships";
        /// <summary>
        /// A link that points to the preferred spouse relationship resource.
        /// </summary>
        public static readonly String PREFERRED_SPOUSE_RELATIONSHIP = "preferred-spouse-relationship";
        /// <summary>
        /// A link that points to the preferred parent relationship resource.
        /// </summary>
        public static readonly String PREFERRED_PARENT_RELATIONSHIP = "preferred-parent-relationship";
    }
}
