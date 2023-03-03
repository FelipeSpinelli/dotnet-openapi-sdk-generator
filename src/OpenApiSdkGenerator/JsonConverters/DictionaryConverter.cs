using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace OpenApiSdkGenerator.JsonConverters
{
    public class DictionaryConverter<T> : JsonConverter<IDictionary<string, T>>
    {
        private readonly Dictionary<string, T> _empty = new Dictionary<string, T>(0);
        private const string VALID_PATH_CHARACTERS_PATTERN = "[^0-9a-zA-Z/{}_-]+";

        public override IDictionary<string, T>? ReadJson(JsonReader reader, Type objectType, IDictionary<string, T>? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jToken = JToken.Load(reader);
            if (jToken == null)
            {
                return _empty;
            }

            var result = new Dictionary<string, T>(jToken.Children().Count());
            foreach (var child in jToken.Children())
            {
                var obj = JsonConvert.DeserializeObject<T>(jToken.SelectToken(child.Path).ToString());
                if (obj == null)
                {
                    continue;
                }

                result[Regex.Replace(child.Path, VALID_PATH_CHARACTERS_PATTERN, string.Empty)] = obj;
            }

            return result;
        }

        public override void WriteJson(JsonWriter writer, IDictionary<string, T>? value, JsonSerializer serializer)
        {
            throw new InvalidOperationException("Serialization not expected!");
        }

        public override bool CanWrite => false;
    }
}
