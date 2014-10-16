using JsonLD.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gx.Rs.Api.Util
{
    public static class RdfExtensions
    {
        public static RDFDataset.Quad GetPredicateQuad(this IEnumerable<RDFDataset.Quad> @this, String value)
        {
            return @this.FirstOrDefault(x => x.GetPredicate().GetValue() == value);
        }

        public static IEnumerable<RDFDataset.Quad> GetPredicateQuads(this IEnumerable<RDFDataset.Quad> @this, String value)
        {
            return @this.Where(x => x.GetPredicate().GetValue() == value);
        }

        public static bool HasPredicateQuad(this IEnumerable<RDFDataset.Quad> @this, String value)
        {
            return @this.Any(x => x.GetPredicate() != null && x.GetPredicate().GetValue() != null && x.GetPredicate().GetValue() == value);
        }

        public static IEnumerable<RDFDataset.Quad> GetSubjectQuads(this IEnumerable<RDFDataset.Quad> @this, String value)
        {
            return @this.Where(x => x.GetSubject().GetValue() == value);
        }
    }
}
