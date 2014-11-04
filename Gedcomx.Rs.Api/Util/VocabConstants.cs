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
        /// The RDF namespace, <a href="http://www.w3.org/1999/02/22-rdf-syntax-ns#">http://www.w3.org/1999/02/22-rdf-syntax-ns#</a>.
        /// </summary>
        public static readonly String RDF_NAMESPACE = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";
        /// <summary>
        /// The RDFS namespace, <a href="http://www.w3.org/2000/01/rdf-schema#">http://www.w3.org/2000/01/rdf-schema#</a>.
        /// </summary>
        public static readonly String RDFS_NAMESPACE = "http://www.w3.org/2000/01/rdf-schema#";
        /// <summary>
        /// The DC namespace, <a href="http://purl.org/dc/terms/">http://purl.org/dc/terms/</a>.
        /// </summary>
        public static readonly String DC_NAMESPACE = "http://purl.org/dc/terms/";
        /// <summary>
        /// The XML namespace, <a href="http://www.w3.org/XML/1998/namespace">http://www.w3.org/XML/1998/namespace</a>.
        /// </summary>
        public static readonly String XML_NAMESPACE = "http://www.w3.org/XML/1998/namespace";
        /// <summary>
        /// The RDF Sequence namespace, <a href="http://www.w3.org/1999/02/22-rdf-syntax-ns#Seq">http://www.w3.org/1999/02/22-rdf-syntax-ns#Seq</a>.
        /// </summary>
        public static readonly String RDF_SEQUENCE_TYPE = RDF_NAMESPACE + "Seq";
    }
}
