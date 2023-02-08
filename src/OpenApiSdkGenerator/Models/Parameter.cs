using Newtonsoft.Json;
using OpenApiSdkGenerator.Models.Enumerators;

namespace OpenApiSdkGenerator.Models
{
    public record Parameter : Contract
    {
        [JsonProperty("name")]
        public string Name { get; set; } = null!;
        
        [JsonProperty("in")]
        public ParameterLocation In { get; set; }        
        
        [JsonProperty("required")]
        public bool Required { get; set; }
        
        [JsonProperty("schema")]
        public Schema? Schema { get; set; }        
    }
}
