using Newtonsoft.Json;
using System.Collections.Generic;

namespace OpenApiSdkGenerator.Models
{
    public record Contract
    {
        [JsonProperty("description")]
        public string Description { get; set; } = null!;
        
        [JsonProperty("content")]
        public IDictionary<string, Schema>? Content { get; set; }
    }
}
