using Newtonsoft.Json;
using OpenApiSdkGenerator.JsonConverters;
using System.Collections.Generic;
using System.Linq;

namespace OpenApiSdkGenerator.Models.OpenApi
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

        public override string ToString()
        {
            const string REQUESTBODY_TEMPLATE = "[Body] {0} requestBody";

            if (!string.IsNullOrWhiteSpace(Reference))
            {
                return string.Format(REQUESTBODY_TEMPLATE, Schema.GetByReference(Reference)?.GetTypeName() ?? "object");
            }

            return string.Format(REQUESTBODY_TEMPLATE, Content.First().Value.GetTypeName());
        }
    }
}
