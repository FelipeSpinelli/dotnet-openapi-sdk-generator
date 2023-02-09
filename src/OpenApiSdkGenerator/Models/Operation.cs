using Newtonsoft.Json;
using OpenApiSdkGenerator.JsonConverters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace OpenApiSdkGenerator.Models
{
    public record Operation
    {
        private const string VALID_NAME_CHARACTERS_PATTERN = "[^0-9a-zA-Z]+";

        [JsonIgnore]
        public string Path { get; set; } = null!;

        [JsonIgnore]
        public string HttpMethod { get; set; } = null!;

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

        public string Name => OperationId ?? Tags[0] ?? Regex.Replace($"{HttpMethod}{Path}", VALID_NAME_CHARACTERS_PATTERN, "");
        public string SuccessResponseType => Responses
            .Where(x => short.TryParse(x.Key, out var statusCode) && statusCode >= 200 && statusCode < 300)
            .First()
            .Value.Content.First().Value
            .GetName();

        public string MethodSignature => "";
    }
}
