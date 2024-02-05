using Newtonsoft.Json;
using OpenApiSdkGenerator.JsonConverters;
using System.Collections.Generic;

namespace OpenApiSdkGenerator.Models.OpenApi
{
    public record Response
    {
        [JsonProperty("description")]
        public string Description { get; set; } = null!;

        [JsonProperty("content")]
        [JsonConverter(typeof(DictionaryConverter<MediaType>))]
        public IDictionary<string, MediaType>? Content { get; set; }
    }
}
