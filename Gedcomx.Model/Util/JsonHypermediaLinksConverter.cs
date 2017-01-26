using Gx.Links;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gedcomx.Model.Util
{
    public class JsonHypermediaLinksConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(IList<Link>).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            IList<Link> result = new List<Link>();
            var jObject = JObject.Load(reader);

            foreach (var key in jObject)
            {
                var link = Newtonsoft.Json.JsonConvert.DeserializeObject<Gx.Links.Link>(key.Value.ToString());
                link.Rel = key.Key;
                result.Add(link);
            }

            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            IList<Link> links = value as IList<Link>;

            writer.WriteStartObject();

            foreach (var link in links)
            {
                writer.WritePropertyName(link.Rel);
                serializer.Serialize(writer, link);
            }

            writer.WriteEndObject();
        }
    }
}
