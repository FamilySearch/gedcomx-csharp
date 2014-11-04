using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gedcomx.Support
{
    /// <summary>
    /// A collection of accept or content types to use in REST API requests.
    /// </summary>
    public class MediaTypes
    {
        /// <summary>
        /// The GEDCOM X XML media type, "application/x-gedcomx-v1+xml"
        /// </summary>
        public static readonly String GEDCOMX_XML_MEDIA_TYPE = "application/x-gedcomx-v1+xml";
        /// <summary>
        /// The GEDCOM X JSON media type, "application/x-gedcomx-v1+json"
        /// </summary>
        public static readonly String GEDCOMX_JSON_MEDIA_TYPE = "application/x-gedcomx-v1+json";
        /// <summary>
        /// The GEDCOM X record set XML media type, "application/x-gedcomx-records-v1+xml"
        /// </summary>
        public static readonly String GEDCOMX_RECORDSET_XML_MEDIA_TYPE = "application/x-gedcomx-records-v1+xml";
        /// <summary>
        /// The GEDCOM X record set JSON media type, "application/x-gedcomx-records-v1+json"
        /// </summary>
        public static readonly String GEDCOMX_RECORDSET_JSON_MEDIA_TYPE = "application/x-gedcomx-records-v1+json";
        /// <summary>
        /// The atom XML media type, "application/atom+xml"
        /// </summary>
        public static readonly String ATOM_XML_MEDIA_TYPE = "application/atom+xml";
        /// <summary>
        /// The atom GEDCOM X JSON media type, "application/x-gedcomx-atom+json"
        /// </summary>
        public static readonly String ATOM_GEDCOMX_JSON_MEDIA_TYPE = "application/x-gedcomx-atom+json";
        /// <summary>
        /// The application octet stream type, "application/octet-stream"
        /// </summary>
        public static readonly String APPLICATION_OCTET_STREAM = "application/octet-stream";
        /// <summary>
        /// The application JSON type, "application/json"
        /// </summary>
        public static readonly String APPLICATION_JSON_TYPE = "application/json";
        /// <summary>
        /// The application URL encoded form type, "application/x-www-form-urlencoded"
        /// </summary>
        public static readonly String APPLICATION_FORM_URLENCODED_TYPE = "application/x-www-form-urlencoded";

        /// <summary>
        /// The wildcard media type, "*"
        /// </summary>
        public static readonly String MEDIA_TYPE_WILDCARD = "*";
        /// <summary>
        /// The wildcard type, "*/*"
        /// </summary>
        public static readonly String WILDCARD_TYPE = "*/*";
        /// <summary>
        /// The application XML type, "application/xml"
        /// </summary>
        public static readonly String APPLICATION_XML_TYPE = "application/xml";
        /// <summary>
        /// The atom XML type, "application/atom+xml"
        /// </summary>
        public static readonly String APPLICATION_ATOM_XML_TYPE = "application/atom+xml";
        /// <summary>
        /// The application XHTML XML type, "application/xhtml+xml"
        /// </summary>
        public static readonly String APPLICATION_XHTML_XML_TYPE = "application/xhtml+xml";
        /// <summary>
        /// The application SVG XML type, "application/svg+xml"
        /// </summary>
        public static readonly String APPLICATION_SVG_XML_TYPE = "application/svg+xml";
        /// <summary>
        /// The multipart form data type, "multipart/form-data"
        /// </summary>
        public static readonly String MULTIPART_FORM_DATA_TYPE = "multipart/form-data";
        /// <summary>
        /// The application octet stream type, "application/octet-stream"
        /// </summary>
        public static readonly String APPLICATION_OCTET_STREAM_TYPE = "application/octet-stream";
        /// <summary>
        /// The plain text type, "text/plain"
        /// </summary>
        public static readonly String TEXT_PLAIN_TYPE = "text/plain";
        /// <summary>
        /// The XML text type, "text/xml"
        /// </summary>
        public static readonly String TEXT_XML_TYPE = "text/xml";
        /// <summary>
        /// The HTML text type, "text/html"
        /// </summary>
        public static readonly String TEXT_HTML_TYPE = "text/html";

        /// <summary>
        /// The plain text type, "text/plain"
        /// </summary>
        public static readonly String TEXT_PLAIN = "text/plain";
    }
}
