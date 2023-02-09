using Newtonsoft.Json;

namespace OpenApiSdkGenerator.Models
{
    public record PathItem
    {
        [JsonProperty("_reference")]
        public string Reference { get; set; } = null!;

        [JsonProperty("summary")]
        public string Summary { get; set; } = null!;

        [JsonProperty("description")]
        public string Description { get; set; } = null!;

        [JsonProperty("parameters")]
        public Parameter[] Parameters { get; set; } = null!;

        [JsonProperty("get")]
        public Operation? Get { get; set; }

        [JsonProperty("post")]
        public Operation? Post { get; set; }

        [JsonProperty("put")]
        public Operation? Put { get; set; }

        [JsonProperty("patch")]
        public Operation? Patch { get; set; }

        [JsonProperty("delete")]
        public Operation? Delete { get; set; }

        [JsonProperty("head")]
        public Operation? Head { get; set; }

        [JsonProperty("options")]
        public Operation? Options { get; set; }

        [JsonProperty("trace")]
        public Operation? Trace { get; set; }
    }
}
