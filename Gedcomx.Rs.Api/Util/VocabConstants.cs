using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gx.Rs.Api.Util
{
    /// <summary>
    /// This is a collection of constants used with RDF processing.
    /// </summary>
    /// <remarks>
    /// At this time, only namespaces are defined here.
    /// </remarks>
    public class VocabConstants
    {
        /// <summary>
        /// The RDF namespace, <see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#" />.
        /// </summary>
        public static readonly String RDF_NAMESPACE = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";
        /// <summary>
        /// The RDFS namespace, <see cref="http://www.w3.org/2000/01/rdf-schema#" />.
        /// </summary>
        public static readonly String RDFS_NAMESPACE = "http://www.w3.org/2000/01/rdf-schema#";
        /// <summary>
        /// The DC namespace, <see cref="http://purl.org/dc/terms/" />.
        /// </summary>
        public static readonly String DC_NAMESPACE = "http://purl.org/dc/terms/";
        /// <summary>
        /// The XML namespace, <see cref="http://www.w3.org/XML/1998/namespace" />.
        /// </summary>
        public static readonly String XML_NAMESPACE = "http://www.w3.org/XML/1998/namespace";
        /// <summary>
        /// The RDF Sequence namespace, <see cref="http://www.w3.org/1999/02/22-rdf-syntax-ns#" />.
        /// </summary>
        public static readonly String RDF_SEQUENCE_TYPE = RDF_NAMESPACE + "Seq";
    }
}
