using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace OpenApiSdkGenerator.Models
{
    public record ApiDefinition
    {
        [JsonProperty("info")]
        public Info Info { get; set; } = new();
        
        [JsonProperty("paths")]
        public IDictionary<string, PathItem> Paths { get; set; } = new Dictionary<string, PathItem>(0);
        
        [JsonProperty("servers")]
        public Server[] Servers { get; set; } = Array.Empty<Server>();
    }
}
