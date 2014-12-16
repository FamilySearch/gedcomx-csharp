using JsonLD.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gx.Rs.Api.Util
{
    /// <summary>
    /// This is an extension helper class to simplify working with the RDF data model.
    /// </summary>
    public static class RdfExtensions
    {
        /// <summary>
        /// Gets the predicate quad with the specified value.
        /// </summary>
        /// <param name="this">The collection of all RDF quads to be searched.</param>
        /// <param name="value">The value to search for among all predicates from the specified collection.</param>
        /// <returns>The first <see cref="RDFDataset.Quad"/> with a matching predicate value to that of the one specified.</returns>
        public static RDFDataset.Quad GetPredicateQuad(this IEnumerable<RDFDataset.Quad> @this, String value)
        {
            return @this.FirstOrDefault(x => x.GetPredicate().GetValue() == value);
        }

        /// <summary>
        /// Gets all predicate quads with the specified value.
        /// </summary>
        /// <param name="this">The collection of all RDF quads to be searched.</param>
        /// <param name="value">The value to search for among all predicates from the specified collection.</param>
        /// <returns>The list of all <see cref="RDFDataset.Quad"/>s with matching predicate values to that of the one specified.</returns>
        public static IEnumerable<RDFDataset.Quad> GetPredicateQuads(this IEnumerable<RDFDataset.Quad> @this, String value)
        {
            return @this.Where(x => x.GetPredicate().GetValue() == value);
        }

        /// <summary>
        /// Determines whether the specified collection has any predicates with the specified values.
        /// </summary>
        /// <param name="this">The collection of all RDF quads to be searched.</param>
        /// <param name="value">The value to search for among all predicates from the specified collection.</param>
        /// <returns>
        /// <c>true</c> if the collection has any predicates with a matching predicate value; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasPredicateQuad(this IEnumerable<RDFDataset.Quad> @this, String value)
        {
            return @this.Any(x => x.GetPredicate() != null && x.GetPredicate().GetValue() != null && x.GetPredicate().GetValue() == value);
        }

        /// <summary>
        /// Gets all subject quads with the specified value.
        /// </summary>
        /// <param name="this">The collection of all RDF quads to be searched.</param>
        /// <param name="value">The value to search for among all subjects from the specified collection.</param>
        /// <returns>The list of all <see cref="RDFDataset.Quad"/>s with matching subject values to that of the one specified.</returns>
        public static IEnumerable<RDFDataset.Quad> GetSubjectQuads(this IEnumerable<RDFDataset.Quad> @this, String value)
        {
            return @this.Where(x => x.GetSubject().GetValue() == value);
        }
    }
}
