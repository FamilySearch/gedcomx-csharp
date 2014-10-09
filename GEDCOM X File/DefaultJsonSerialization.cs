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
    public class DefaultJsonSerialization : GedcomxEntrySerializer, GedcomxEntryDeserializer
    {
        private JsonSerializerSettings jsonSettings;
        private static Encoding encoding;

        public DefaultJsonSerialization()
            : this(true)
        {
        }

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
            using (var writer = new StreamWriter(stream, DefaultJsonSerialization.Encoding))
            {
                writer.Write(JsonConvert.SerializeObject(resource, jsonSettings));
            }
        }

        public bool IsKnownContentType(String contentType)
        {
            return KnownContentTypes.Contains(contentType);
        }

        public T Deserialize<T>(String value)
        {
            T result;

            using (var stream = new MemoryStream(DefaultJsonSerialization.Encoding.GetBytes(value)))
            {
                result = Deserialize<T>(stream);
            }

            return result;
        }

        public T Deserialize<T>(Stream stream)
        {
            T result;

            using (var reader = new StreamReader(stream))
            {
                result = JsonConvert.DeserializeObject<T>(reader.ReadToEnd(), jsonSettings);
            }

            return result;
        }

        public ISet<String> KnownContentTypes
        {
            get;
            set;
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
