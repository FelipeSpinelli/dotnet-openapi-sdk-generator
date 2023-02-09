using Newtonsoft.Json;
using OpenApiSdkGenerator.Models.Enumerators;
using System;

namespace OpenApiSdkGenerator.Models
{
    public record Schema
    {
        [JsonProperty("type")]
        public DataType Type { get; set; }

        [JsonProperty("format")]
        public DataFormat? Format { get; set; }

        [JsonProperty("required")]
        public string[] RequiredProperties { get; set; } = null!;

        [JsonProperty("properties")]
        public Schema[] Properties { get; set; } = Array.Empty<Schema>();

        [JsonProperty("_reference")]
        public string Reference { get; set; } = null!;

        [JsonProperty("items")]
        public Schema? Items { get; set; }
    }
}
