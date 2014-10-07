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
    public class DefaultXmlSerialization : GedcomxEntrySerializer, GedcomxEntryDeserializer
    {
        private readonly XmlSerializer serializer;
        private readonly XmlSerializerNamespaces namespaces;
        private XmlWriterSettings writerSettings;
        private XmlReaderSettings readerSettings;
        private static Encoding encoding;

        public DefaultXmlSerialization(params Type[] types)
            : this(true, types)
        {
        }

        public DefaultXmlSerialization(bool pretty, params Type[] types)
        {
            writerSettings = new XmlWriterSettings();
            readerSettings = new XmlReaderSettings();
            writerSettings.Encoding = DefaultXmlSerialization.Encoding;
            KnownContentTypes = new HashSet<String>() { MediaTypes.GEDCOMX_XML_MEDIA_TYPE };
            serializer = NewContext(types);
            namespaces = new GedcomNamespaceManager<Gx.Gedcomx>();
            if (pretty)
            {
                writerSettings.Indent = true;
            }
            else
            {
                writerSettings.Indent = false;
            }
        }

        public T Deserialize<T>(String value)
        {
            T result;

            using (var stream = new MemoryStream(DefaultXmlSerialization.Encoding.GetBytes(value)))
            {
                result = Deserialize<T>(stream);
            }

            return result;
        }

        public T Deserialize<T>(Stream stream)
        {
            using (var xr = XmlReader.Create(stream, readerSettings))
            {
                return (T)this.serializer.Deserialize(xr);
            }
        }

        public String Serialize(Object resource)
        {
            String result = null;

            using (var stream = new MemoryStream())
            {
                Serialize(resource, stream);

                using (var ms = new MemoryStream(stream.ToArray()))
                using (var reader = new StreamReader(ms))
                {
                    result = reader.ReadToEnd();
                }
            }

            return result;
        }

        public void Serialize(Object resource, Stream stream)
        {
            writerSettings.Encoding = Encoding;
            using (var writer = XmlWriter.Create(stream, writerSettings))
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

        public static Encoding Encoding
        {
            get
            {
                return encoding ?? Encoding.UTF8;
            }
            set
            {
                encoding = value;
            }
        }
    }
}
