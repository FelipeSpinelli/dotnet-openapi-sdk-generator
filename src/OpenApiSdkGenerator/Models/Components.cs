using Newtonsoft.Json;
using OpenApiSdkGenerator.Extensions;
using OpenApiSdkGenerator.JsonConverters;
using System.Collections.Generic;

namespace OpenApiSdkGenerator.Models;

public record Components
{
    [JsonProperty("schemas")]
    [JsonConverter(typeof(DictionaryConverter<Schema>))]
    public IDictionary<string, Schema> Schemas { get; set; } = new Dictionary<string, Schema>();

    public void Concat(Components? components)
    {
        if (components == null)
        {
            return;
        }

        Schemas ??= new Dictionary<string, Schema>();
        Schemas.Merge(components.Schemas);
    }
}
