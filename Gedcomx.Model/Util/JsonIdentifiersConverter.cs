using Gx.Conclusion;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gedcomx.Model.Util
{
    class JsonIdentifiersConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(IList<Identifier>).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            IList<Identifier> result = new List<Identifier>();
            var jObject = JObject.Load(reader);

            foreach (var key in jObject)
            {
                var link = new Identifier();
                link.Type = key.Key;
                link.Value = key.Value.Select(x => ((JValue)x).Value as string).FirstOrDefault();
                result.Add(link);
            }

            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            IList<Identifier> links = value as IList<Identifier>;

            writer.WriteStartObject();

            foreach (var link in links)
            {
                writer.WritePropertyName(link.Type);
                writer.WriteStartArray();
                writer.WriteValue(link.Value);
                writer.WriteEndArray();
            }

            writer.WriteEndObject();
        }
    }
}
