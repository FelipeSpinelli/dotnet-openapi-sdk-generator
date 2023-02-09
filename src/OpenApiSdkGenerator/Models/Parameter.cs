using Newtonsoft.Json;
using OpenApiSdkGenerator.JsonConverters;
using OpenApiSdkGenerator.Models.Enumerators;
using System.Collections.Generic;

namespace OpenApiSdkGenerator.Models
{
    public record Parameter
    {
        [JsonProperty("description")]
        public string Description { get; set; } = null!;
        [JsonProperty("name")]
        public string Name { get; set; } = null!;
        
        [JsonProperty("in")]
        public ParameterLocation In { get; set; }        
        
        [JsonProperty("required")]
        public bool Required { get; set; }
        
        [JsonProperty("schema")]
        public Schema? Schema { get; set; }

        [JsonProperty("content")]
        [JsonConverter(typeof(DictionaryConverter<MediaType>))]
        public IDictionary<string, MediaType>? Content { get; set; }
    }
}
