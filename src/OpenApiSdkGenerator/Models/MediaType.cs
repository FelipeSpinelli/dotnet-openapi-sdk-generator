using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace OpenApiSdkGenerator.Models
{
    public record MediaType
    {
        [JsonProperty("_reference")]
        public string? Reference { get; set; }

        [JsonProperty("schema")]
        public Schema? Schema { get; set; }
    }
}
