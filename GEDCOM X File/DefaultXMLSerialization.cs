using Gedcomx.Support;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Gedcomx.File
{
    public class DefaultXMLSerialization : GedcomxEntrySerializer, GedcomxEntryDeserializer
    {
        private readonly XmlSerializer serializer;
        private readonly XmlSerializerNamespaces namespaces;
        private XmlWriterSettings settings;

        public DefaultXMLSerialization(params Type[] types)
            : this(true, types)
        {
        }

        public DefaultXMLSerialization(bool pretty, params Type[] types)
        {
            settings = new XmlWriterSettings();
            //settings.Encoding = new UTF8Encoding(false);
            KnownContentTypes = new HashSet<String>() { MediaTypes.GEDCOMX_XML_MEDIA_TYPE };
            serializer = NewContext(types);
            namespaces = new GedcomNamespaceManager<Gx.Gedcomx>();
            if (pretty)
            {
                settings.Indent = true;
            }
            else
            {
                settings.Indent = false;
            }
        }

        public Object Deserialize(Stream stream)
        {
            using (var xr = XmlReader.Create(stream))
            {
                return this.serializer.Deserialize(xr);
            }
        }

        public void Serialize(Object resource, Stream stream)
        {
            using (var writer = XmlWriter.Create(stream, settings))
            {
                this.serializer.Serialize(writer, resource, namespaces);
            }
        }

        public bool IsKnownContentType(String contentType)
        {
            return this.KnownContentTypes.Contains(contentType);
        }

        public ISet<String> KnownContentTypes
        {
            get;
            set;
        }

        /**
         * Factory method for creating a new instance of a <code>JAXBContext</code> appropriate for reading and/or writing a GEDCOM X file.
         *
         * The created <code>JAXBContext</code> references the following classes by default:
         *   org.gedcomx.conclusion.Person
         *   org.gedcomx.conclusion.Relationship
         *   org.gedcomx.metadata.dc.ObjectFactory
         *   org.gedcomx.metadata.foaf.Person
         *   org.gedcomx.contributor.Agent
         *   org.gedcomx.metadata.rdf.Description
         * Any additional classes needed can be passed to this call to supplement (not override) these defaults
         *
         * @param classes Additional classes to supplement (not override) the provided defaults
         * @return A JAXBContext
         *
         * @throws JAXBException
         */
        private static XmlSerializer NewContext(params Type[] types)
        {
            return new XmlSerializer(typeof(Gx.Gedcomx), types);
        }
    }
}
