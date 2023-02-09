using Newtonsoft.Json;
using OpenApiSdkGenerator.JsonConverters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenApiSdkGenerator.Models
{
    public record ApiDefinition
    {
        [JsonProperty("openapi")]
        public string OpenApiVersion { get; set; } = "3.0.1";

        [JsonProperty("info")]
        public Info Info { get; set; } = new();

        [JsonProperty("servers")]
        public Server[] Servers { get; set; } = Array.Empty<Server>();

        [JsonProperty("paths")]
        [JsonConverter(typeof(DictionaryConverter<PathItem>))]
        public IDictionary<string, PathItem> Paths { get; set; } = new Dictionary<string, PathItem>(0);

        public IEnumerable<Operation> Operations => Paths.SelectMany(path => path.Value.GetOperations(path.Key));
    }
}
