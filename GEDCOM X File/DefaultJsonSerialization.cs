using Gedcomx.Support;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gedcomx.File
{
    /// <summary>
    /// The default JSON serialization class.
    /// </summary>
    public class DefaultJsonSerialization : GedcomxEntrySerializer, GedcomxEntryDeserializer
    {
        private JsonSerializerSettings jsonSettings;
        private static Encoding encoding;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultJsonSerialization"/> class. This overload defaults to using pretty output.
        /// </summary>
        public DefaultJsonSerialization()
            : this(true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultJsonSerialization"/> class.
        /// </summary>
        /// <param name="pretty">If set to <c>true</c> the serialized output will be formatted with whitespace.</param>
        public DefaultJsonSerialization(bool pretty)
        {
            jsonSettings = new JsonSerializerSettings();
            jsonSettings.NullValueHandling = NullValueHandling.Ignore;
            jsonSettings.Formatting = pretty ? Formatting.Indented : Formatting.None;
            KnownContentTypes = new HashSet<String>() { MediaTypes.GEDCOMX_JSON_MEDIA_TYPE };
            jsonSettings.Error += (sender, e) =>
            {
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    System.Diagnostics.Debugger.Break();
                }
            };
        }

        /// <summary>
        /// Serializes the specified object to JSON.
        /// </summary>
        /// <param name="resource">The object to serialize.</param>
        /// <returns>A JSON string representing the serialized object specified.</returns>
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
        /// Serializes the specified object to JSON.
        /// </summary>
        /// <param name="resource">The object to serialize.</param>
        /// <param name="stream">The stream that will contain the JSON output after serialization.</param>
        public void Serialize(Object resource, Stream stream)
        {
            using (var writer = new StreamWriter(stream, DefaultJsonSerialization.Encoding))
            {
                writer.Write(JsonConvert.SerializeObject(resource, jsonSettings));
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
            return KnownContentTypes.Contains(contentType);
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

            using (var stream = new MemoryStream(DefaultJsonSerialization.Encoding.GetBytes(value)))
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
            T result;

            using (var reader = new StreamReader(stream))
            {
                var type = typeof(T);

                if (type.IsPrimitive || type == typeof(String))
                {
                    result = (T)Convert.ChangeType(reader.ReadToEnd(), type);
                }
                else
                {
                    result = JsonConvert.DeserializeObject<T>(reader.ReadToEnd(), jsonSettings);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets or sets the content types known to the serializer.
        /// </summary>
        /// <value>
        /// The content types known to the serializer.
        /// </value>
        public ISet<String> KnownContentTypes
        {
            get;
            set;
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
