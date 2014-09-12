using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilySearch.Api
{
    public class Rel : Gx.Rs.Api.Rel
    {
        public static readonly String CHANGE_HISTORY = "change-history";
        public static readonly String COMMENT = "comment";
        public static readonly String COMMENTS = "comments";
        public static readonly String CURRENT_USER = "current-user";
        public static readonly String DISCUSSIONS = "discussions";
        public static readonly String MERGE = "merge";
        public static readonly String MERGE_MIRROR = "merge-mirror";
        public static readonly String NORMALIZED_DATE = "normalized-date";
        public static readonly String NOT_A_MATCHES = "non-matches";
        public static readonly String NOT_A_MATCH = "non-match";
        public static readonly String PERSON_MATCHES_QUERY = "person-matches-query";
        public static readonly String PORTRAIT = "portrait";
        public static readonly String PORTRAITS = "portraits";
        public static readonly String RESTORE = "restore";
        public static readonly String FATHER_ROLE = "father-role";
        public static readonly String MOTHER_ROLE = "mother-role";
        public static readonly String PERSON_WITH_RELATIONSHIPS = "person-with-relationships";
        public static readonly String PREFERRED_SPOUSE_RELATIONSHIP = "preferred-spouse-relationship";
        public static readonly String PREFERRED_PARENT_RELATIONSHIP = "preferred-parent-relationship";
    }
}
