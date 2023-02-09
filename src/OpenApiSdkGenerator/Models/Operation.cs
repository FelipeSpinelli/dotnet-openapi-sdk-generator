using Newtonsoft.Json;
using OpenApiSdkGenerator.JsonConverters;
using System;
using System.Collections.Generic;

namespace OpenApiSdkGenerator.Models
{
    public record Operation
    {
        [JsonProperty("_reference")]
        public string Reference { get; set; } = null!;

        [JsonProperty("summary")]
        public string Summary { get; set; } = null!;

        [JsonProperty("description")]
        public string Description { get; set; } = null!;

        [JsonProperty("operationId")]
        public string OperationId { get; set; } = null!;
        
        [JsonProperty("tags")]
        public string[] Tags { get; set; } = Array.Empty<string>();
        
        [JsonProperty("parameters")]
        public Parameter[] Parameters { get; set; } = Array.Empty<Parameter>();

        [JsonProperty("requestBody")]
        public RequestBody? RequestBody { get; set; }

        [JsonProperty("responses")]
        [JsonConverter(typeof(DictionaryConverter<Response>))]
        public IDictionary<string, Response> Responses { get; set; }
    }
}
