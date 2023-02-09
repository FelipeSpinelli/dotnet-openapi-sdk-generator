using Newtonsoft.Json;
using OpenApiSdkGenerator.JsonConverters;
using System.Collections.Generic;

namespace OpenApiSdkGenerator.Models
{
    public record RequestBody
    {
        [JsonProperty("_reference")]
        public string Reference { get; set; } = null!;

        [JsonProperty("description")]
        public string Description { get; set; } = null!;

        [JsonProperty("content")]
        [JsonConverter(typeof(DictionaryConverter<MediaType>))]
        public IDictionary<string, MediaType>? Content { get; set; }

        [JsonProperty("required")]
        public bool Required { get; set; }
    }
}
