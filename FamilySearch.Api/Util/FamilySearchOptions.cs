using Gx.Fs.Artifacts;
using Gx.Fs.Ct;
using Gx.Fs.Tree;
using Gx.Rs.Api.Options;
using Gx.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilySearch.Api.Util
{
    public class FamilySearchOptions
    {
        public static readonly String CANDIDATE_ID = "candidateId";
        public static readonly String COLLECTION = "collection";
        public static readonly String CONFIDENCE = "confidence";
        public static readonly String DEFAULT = "default";
        public static readonly String DESCRIPTION = "description";
        public static readonly String FILENAME = "filename";
        public static readonly String FILTER = "filter";
        public static readonly String INCLUDE_MARRIAGE_DETAILS = "marriageDetails";
        public static readonly String INCLUDE_PERSON_DETAILS = "personDetails";
        public static readonly String INCLUDE_PERSONS = "persons";
        public static readonly String SPOUSE_ID = "spouse";
        public static readonly String STATUS = "status";
        public static readonly String TITLE = "title";
        public static readonly String TYPE = "type";

        private FamilySearchOptions() { }

        public static QueryParameter CandidateId(String[] id)
        {
            return new QueryParameter(true, CANDIDATE_ID, id);
        }

        public static QueryParameter Collection(String value)
        {
            return new QueryParameter(true, COLLECTION, value);
        }

        public static QueryParameter Collection(MatchCollection value)
        {
            return Collection(MatchCollectionQNameUtil.ConvertToKnownQName(value));
        }

        public static QueryParameter Confidence(ConfidenceLevel confidence)
        {
            return new QueryParameter(true, CONFIDENCE, ConfidenceLevelQNameUtil.ConvertToKnownQName(confidence));
        }

        public static QueryParameter DefaultUri(String defaultUri)
        {
            return new QueryParameter(true, DEFAULT, defaultUri);
        }

        public static QueryParameter Description(String description)
        {
            return new QueryParameter(true, DESCRIPTION, description);
        }

        public static QueryParameter Filename(String filename)
        {
            return new QueryParameter(true, FILENAME, filename);
        }

        public static QueryParameter Title(String title)
        {
            return new QueryParameter(true, TITLE, title);
        }

        public static QueryParameter ArtifactType(ArtifactType type)
        {
            return new QueryParameter(true, TYPE, ArtifactTypeQNameUtil.ConvertToKnownQName(type));
        }

        public static QueryParameter MergeAnalysisFilter(MergeAnalysisFilter filter)
        {
            return new QueryParameter(true, FILTER, filter.ToString());
        }

        public static QueryParameter IncludePersons()
        {
            return new QueryParameter(true, INCLUDE_PERSONS, "true");
        }

        public static QueryParameter SpouseId(String spouseId)
        {
            return new QueryParameter(true, SPOUSE_ID, spouseId);
        }

        public static QueryParameter IncludePersonDetails()
        {
            return new QueryParameter(true, INCLUDE_PERSON_DETAILS, "true");
        }

        public static QueryParameter IncludeMarriageDetails()
        {
            return new QueryParameter(true, INCLUDE_MARRIAGE_DETAILS, "true");
        }

        public static QueryParameter MatchStatus(MatchStatus status)
        {
            return new QueryParameter(false, STATUS, MatchStatusQNameUtil.ConvertToKnownQName(status));
        }

        public static HeaderParameter reason(String reason)
        {
            return new HeaderParameter(true, "X-Reason", reason);
        }
    }
}
