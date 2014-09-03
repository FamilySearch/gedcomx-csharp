using Gx.Links;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Gx.Rs.Api.Util
{
    public class HypermediaSerializer : JsonConverter
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
                result.Add(new Link(key.Key, key.Value.ToString()));
            }

            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
        }
    }
}
