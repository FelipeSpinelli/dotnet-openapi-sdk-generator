using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace OpenApiSdkGenerator.Models
{
    public record MediaType
    {
        [JsonProperty("_reference")]
        public string? Reference { get; set; }

        [JsonProperty("schema")]
        public Schema? Schema { get; set; }

        public string GetTypeName() => (Reference != null ?
            Schema.GetByReference(Reference) : Schema)?.GetTypeName() ?? string.Empty;

        public string[] GetProperties()
        {
            if (string.IsNullOrWhiteSpace(Reference))
            {
                return Schema?.GetProperties() ?? Array.Empty<string>();
            }

            return Schema.GetByReference(Reference)?.GetProperties() ?? Array.Empty<string>();
        }
    }
}
