using Newtonsoft.Json;
using OpenApiSdkGenerator.JsonConverters;
using System.Collections.Generic;

namespace OpenApiSdkGenerator.Models
{
    public record Components
    {
        [JsonProperty("schemas")]
        [JsonConverter(typeof(DictionaryConverter<Schema>))]
        public IDictionary<string, Schema> Schemas { get; set; } = new Dictionary<string, Schema>();
    }
}
