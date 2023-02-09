using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace OpenApiSdkGenerator.JsonConverters
{
    public class SingleObjectConverter<T> : JsonConverter<T>
    {
        public override T? ReadJson(JsonReader reader, Type objectType, T? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jToken = JToken.Load(reader);
            if (jToken == null)
            {
                return default;
            }

            var obj = JsonConvert.DeserializeObject<T>(jToken.ToString());
            return obj;
        }

        public override void WriteJson(JsonWriter writer, T? value, JsonSerializer serializer)
        {
            throw new InvalidOperationException("Serialization not expected!");
        }

        public override bool CanWrite => false;
    }
}
