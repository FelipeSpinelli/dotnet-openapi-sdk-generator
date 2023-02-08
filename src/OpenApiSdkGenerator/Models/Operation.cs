using Newtonsoft.Json;
using System;

namespace OpenApiSdkGenerator.Models
{
    public record Operation
    {
        [JsonProperty("operationId")]
        public string OperationId { get; set; } = null!;
        
        [JsonProperty("tags")]
        public string[] Tags { get; set; } = Array.Empty<string>();
        
        [JsonProperty("parameters")]
        public Parameter[] Parameters { get; set; } = Array.Empty<Parameter>();

        [JsonProperty("requestBody")]
        public Schema? RequestBody { get; set; }
    }
}
