using Gedcomx.Model.Util;
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
    /// <summary>
    /// Represents FamilySearch specific query string parameter to use in REST API requests.
    /// </summary>
    public class FamilySearchOptions
    {
        /// <summary>
        /// The candidate ID query parameter.
        /// </summary>
        public static readonly String CANDIDATE_ID = "candidateId";
        /// <summary>
        /// The collection query parameter.
        /// </summary>
        public static readonly String COLLECTION = "collection";
        /// <summary>
        /// The confidence query parameter.
        /// </summary>
        public static readonly String CONFIDENCE = "confidence";
        /// <summary>
        /// The default URI query parameter.
        /// </summary>
        public static readonly String DEFAULT = "default";
        /// <summary>
        /// The description query parameter.
        /// </summary>
        public static readonly String DESCRIPTION = "description";
        /// <summary>
        /// The file name query parameter.
        /// </summary>
        public static readonly String FILENAME = "filename";
        /// <summary>
        /// The filter query parameter.
        /// </summary>
        public static readonly String FILTER = "filter";
        /// <summary>
        /// The include marriage details query parameter.
        /// </summary>
        public static readonly String INCLUDE_MARRIAGE_DETAILS = "marriageDetails";
        /// <summary>
        /// The include person details query parameter.
        /// </summary>
        public static readonly String INCLUDE_PERSON_DETAILS = "personDetails";
        /// <summary>
        /// The include persons query parameter.
        /// </summary>
        public static readonly String INCLUDE_PERSONS = "persons";
        /// <summary>
        /// The spouse ID query parameter.
        /// </summary>
        public static readonly String SPOUSE_ID = "spouse";
        /// <summary>
        /// The status query parameter.
        /// </summary>
        public static readonly String STATUS = "status";
        /// <summary>
        /// The title query parameter.
        /// </summary>
        public static readonly String TITLE = "title";
        /// <summary>
        /// The type query parameter.
        /// </summary>
        public static readonly String TYPE = "type";

        private FamilySearchOptions() { }

        /// <summary>
        /// Creates a candidate ID query string parameter.
        /// </summary>
        /// <param name="id">The array of IDs to use in the candidate ID query string parameter. See remarks.</param>
        /// <returns>A candidate ID query string parameter.</returns>
        /// <remarks>
        /// This could be used, for example, to specify a match candidate for use with certain merge operations.
        /// </remarks>
        public static QueryParameter CandidateId(String[] id)
        {
            return new QueryParameter(true, CANDIDATE_ID, id);
        }

        /// <summary>
        /// Creates a collection query string parameter.
        /// </summary>
        /// <param name="value">The value to use in the collection query string parameter. See remarks.</param>
        /// <returns>A collection query string parameter.</returns>
        /// <remarks>
        /// This could be used, for example, to specify what collection to read person records from.
        /// </remarks>
        public static QueryParameter Collection(String value)
        {
            return new QueryParameter(true, COLLECTION, value);
        }

        /// <summary>
        /// Creates a collection query string parameter.
        /// </summary>
        /// <param name="value">The value to use in the collection query string parameter. See remarks.</param>
        /// <returns>A collection query string parameter.</returns>
        /// <remarks>
        /// This could be used, for example, to specify what collection to read person records from.
        /// </remarks>
        public static QueryParameter Collection(MatchCollection value)
        {
            return Collection(XmlQNameEnumUtil.GetNameValue(value));
        }

        /// <summary>
        /// Creates a confidence query string parameter.
        /// </summary>
        /// <param name="confidence">The value to use in the confidence query string parameter. See remarks.</param>
        /// <returns>A confidence query string parameter.</returns>
        /// <remarks>
        /// This could be used, for example, to specify the level of confidence required during certain merge operations.
        /// </remarks>
        public static QueryParameter Confidence(ConfidenceLevel confidence)
        {
            return new QueryParameter(true, CONFIDENCE, XmlQNameEnumUtil.GetNameValue(confidence));
        }

        /// <summary>
        /// Creates a default URI query string parameter.
        /// </summary>
        /// <param name="defaultUri">The value to use in the default URI query string parameter. See remarks.</param>
        /// <returns>A default URI query string parameter.</returns>
        /// <remarks>
        /// This could be used, for example, to create a default image URI to use for person records when a person does not have an image.
        /// </remarks>
        public static QueryParameter DefaultUri(String defaultUri)
        {
            return new QueryParameter(true, DEFAULT, defaultUri);
        }

        /// <summary>
        /// Creates a description query string parameter.
        /// </summary>
        /// <param name="description">The value to use in the description query string parameter. See remarks.</param>
        /// <returns>A description query string parameter.</returns>
        /// <remarks>
        /// This could be used, for example, to specify the description of an artifact.
        /// </remarks>
        public static QueryParameter Description(String description)
        {
            return new QueryParameter(true, DESCRIPTION, description);
        }

        /// <summary>
        /// Creates a file name query string parameter.
        /// </summary>
        /// <param name="filename">The value to use in the file name query string parameter. See remarks.</param>
        /// <returns>A file name query string parameter.</returns>
        /// <remarks>
        /// This could be used, for example, to specify the file name of an artifact.
        /// </remarks>
        public static QueryParameter Filename(String filename)
        {
            return new QueryParameter(true, FILENAME, filename);
        }

        /// <summary>
        /// Creates a title query string parameter.
        /// </summary>
        /// <param name="title">The value to use in the title query string parameter. See remarks.</param>
        /// <returns>A title query string parameter.</returns>
        /// <remarks>
        /// This could be used, for example, to specify the title of an artifact.
        /// </remarks>
        public static QueryParameter Title(String title)
        {
            return new QueryParameter(true, TITLE, title);
        }

        /// <summary>
        /// Creates a artifact type query string parameter.
        /// </summary>
        /// <param name="type">The value to use in the artifact type query string parameter. See remarks.</param>
        /// <returns>A artifact type query string parameter.</returns>
        /// <remarks>
        /// This could be used, for example, to specify the type of an artifact.
        /// </remarks>
        public static QueryParameter ArtifactType(ArtifactType type)
        {
            return new QueryParameter(true, TYPE, XmlQNameEnumUtil.GetNameValue(type));
        }

        /// <summary>
        /// Creates a merge analysis filter query string parameter.
        /// </summary>
        /// <param name="filter">The value to use in the merge analysis filter query string parameter. See remarks.</param>
        /// <returns>A merge analysis filter query string parameter.</returns>
        /// <remarks>
        /// This could be used, for example, to filter the type of merge analysis data you would want to see in merge analysis results.
        /// </remarks>
        public static QueryParameter MergeAnalysisFilter(MergeAnalysisFilter filter)
        {
            return new QueryParameter(true, FILTER, filter.ToString());
        }

        /// <summary>
        /// Creates a include persons query string parameter.
        /// </summary>
        /// <returns>A include persons query string parameter.</returns>
        /// <remarks>
        /// This could be used, for example, to include spouse person records when reading a person.
        /// </remarks>
        public static QueryParameter IncludePersons()
        {
            return new QueryParameter(true, INCLUDE_PERSONS, "true");
        }

        /// <summary>
        /// Creates a spouse ID query string parameter.
        /// </summary>
        /// <param name="spouseId">The value to use in the spouse ID query string parameter. See remarks.</param>
        /// <returns>A spouse ID query string parameter.</returns>
        /// <remarks>
        /// This could be used, for example, to specify a spouse filter when searching for a person.
        /// </remarks>
        public static QueryParameter SpouseId(String spouseId)
        {
            return new QueryParameter(true, SPOUSE_ID, spouseId);
        }

        /// <summary>
        /// Creates an include person details query string parameter.
        /// </summary>
        /// <returns>An include person details query string parameter.</returns>
        /// <remarks>
        /// This could be used, for example, to preload person data when reading a person, rather than the default behavior to only load essential person data.
        /// </remarks>
        public static QueryParameter IncludePersonDetails()
        {
            return new QueryParameter(true, INCLUDE_PERSON_DETAILS, "true");
        }

        /// <summary>
        /// Creates an include marriage details query string parameter.
        /// </summary>
        /// <returns>An include marriage details query string parameter.</returns>
        /// <remarks>
        /// This could be used, for example, to preload marriage data when reading a person, rather than the default behaior to only load essential person data.
        /// </remarks>
        public static QueryParameter IncludeMarriageDetails()
        {
            return new QueryParameter(true, INCLUDE_MARRIAGE_DETAILS, "true");
        }

        /// <summary>
        /// Creates a match status query string parameter.
        /// </summary>
        /// <param name="status">The value to use in the match status query string parameter. See remarks.</param>
        /// <returns>A match status query string parameter.</returns>
        /// <remarks>
        /// This could be used, for example, to filter the types of matches for a given person.
        /// </remarks>
        public static QueryParameter MatchStatus(MatchStatus status)
        {
            return new QueryParameter(false, STATUS, XmlQNameEnumUtil.GetNameValue(status));
        }

        /// <summary>
        /// Creates a reason request header parameter.
        /// </summary>
        /// <param name="reason">The value to use in the reason request header parameter. See remarks.</param>
        /// <returns>A reason request header parameter.</returns>
        /// <remarks>
        /// This could be used, for example, to specify a reason for a particular action, such as the reason for modifying a person record.
        /// </remarks>
        public static HeaderParameter Reason(String reason)
        {
            return new HeaderParameter(true, "X-Reason", reason);
        }
    }
}
