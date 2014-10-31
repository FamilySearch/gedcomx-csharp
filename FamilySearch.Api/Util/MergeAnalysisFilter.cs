using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilySearch.Api.Util
{
    /// <summary>
    /// Represents the type of merge analysis to filter by.
    /// </summary>
    public enum MergeAnalysisFilter
    {
        /// <summary>
        /// Only the couple relationships merge analysis should be returned.
        /// </summary>
        CoupleRelationships,
        /// <summary>
        /// Only the non vital conclusions merge analysis should be returned.
        /// </summary>
        NonVitalConclusions,
        /// <summary>
        /// Only the parent child relationships as child merge analysis should be returned.
        /// </summary>
        ParentChildRelationshipsAsChild,
        /// <summary>
        /// Only the parent child relationships as parent merge analysis should be returned.
        /// </summary>
        ParentChildRelationshipsAsParent,
        /// <summary>
        /// Only the source references merge analysis should be returned.
        /// </summary>
        SourceReferences,
        /// <summary>
        /// Only the vital conclusions merge analysis should be returned.
        /// </summary>
        VitalConclusions
    }
}
