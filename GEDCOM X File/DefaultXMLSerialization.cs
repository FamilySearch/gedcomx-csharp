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
    /// <summary>
    /// The default XML serialization class.
    /// </summary>
    public class DefaultXmlSerialization : IGedcomxEntrySerializer, IGedcomxEntryDeserializer
    {
        private readonly XmlSerializer serializer;
        private readonly XmlSerializerNamespaces namespaces;
        private XmlWriterSettings writerSettings;
        private XmlReaderSettings readerSettings;
        private static Encoding encoding;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultXmlSerialization"/> class. This overload defaults to using pretty output.
        /// </summary>
        /// <param name="types">The types the serializer is to know about for serialization.</param>
        public DefaultXmlSerialization(params Type[] types)
            : this(true, types)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultXmlSerialization"/> class.
        /// </summary>
        /// <param name="pretty">If set to <c>true</c> the serialized output will be formatted with whitespace.</param>
        /// <param name="types">The types the serializer is to know about for serialization.</param>
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

        /// <summary>
        /// Deserializes the specified string.
        /// </summary>
        /// <typeparam name="T">The type of object the specified string represents.</typeparam>
        /// <param name="value"></param>
        /// <returns>
        /// An instance of T upon successful deserialization.
        /// </returns>
        public T Deserialize<T>(String value)
        {
            T result;

            using (var stream = new MemoryStream(DefaultXmlSerialization.Encoding.GetBytes(value)))
            {
                result = Deserialize<T>(stream);
            }

            return result;
        }

        /// <summary>
        /// Deserializes the specified stream.
        /// </summary>
        /// <typeparam name="T">The type of object the specified stream represents.</typeparam>
        /// <param name="stream">The stream to be deserialized.</param>
        /// <returns>
        /// An instance of T upon successful deserialization.
        /// </returns>
        public T Deserialize<T>(Stream stream)
        {
            using (var xr = XmlReader.Create(stream, readerSettings))
            {
                return (T)this.serializer.Deserialize(xr);
            }
        }

        /// <summary>
        /// Serializes the specified object and returns the string result.
        /// </summary>
        /// <param name="resource">The object to be serialized.</param>
        /// <returns>
        /// A string representation of the serialized object.
        /// </returns>
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

        /// <summary>
        /// Serializes the specified object to the specified stream.
        /// </summary>
        /// <param name="resource">The object to be serialized.</param>
        /// <param name="stream">The stream that will contain the serialization result.</param>
        public void Serialize(Object resource, Stream stream)
        {
            writerSettings.Encoding = Encoding;
            using (var writer = XmlWriter.Create(stream, writerSettings))
            {
                this.serializer.Serialize(writer, resource, namespaces);
            }
        }

        /// <summary>
        /// Determines whether the content type is known to the serializer. This is currently not used in any meaningful way.
        /// </summary>
        /// <param name="contentType">The content type to check.</param>
        /// <returns>
        ///   <c>true</c> if the content type is known to the serializer; otherwise, false.
        /// </returns>
        public bool IsKnownContentType(String contentType)
        {
            return this.KnownContentTypes.Contains(contentType);
        }

        /// <summary>
        /// Gets or sets the known content types.
        /// </summary>
        /// <value>
        /// The known content types.
        /// </value>
        public ISet<String> KnownContentTypes
        {
            get;
            set;
        }

        private static XmlSerializer NewContext(params Type[] types)
        {
            XmlSerializer result = null;

            if (types != null && types.Length > 0)
            {
                var remaining = new List<Type>(types.Skip(1).ToArray());
                remaining.Add(typeof(Gx.Gedcomx));
                result = new XmlSerializer(types.First(), remaining.ToArray());
            }
            else
            {
                result = new XmlSerializer(typeof(Gx.Gedcomx), types);
            }

            return result;
        }

        /// <summary>
        /// Gets or sets the encoding to use for all serialization reading and writing. Changing this will take effect on subsequent reads or writes by this serializer.
        /// </summary>
        /// <value>
        /// The encoding to use for all serialization reading and writing. Changing this will take effect on subsequent reads or writes by this serializer.
        /// </value>
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
