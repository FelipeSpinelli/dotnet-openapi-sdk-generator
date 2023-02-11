using Newtonsoft.Json;
using OpenApiSdkGenerator.JsonConverters;
using OpenApiSdkGenerator.Models.Enumerators;
using Scriban;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenApiSdkGenerator.Models
{
    public record Parameter
    {
        [JsonProperty("description")]
        public string Description { get; set; } = null!;
        [JsonProperty("name")]
        public string Name { get; set; } = null!;

        [JsonProperty("in")]
        public ParameterLocation In { get; set; }

        [JsonProperty("required")]
        public bool Required { get; set; }

        [JsonProperty("schema")]
        public Schema? Schema { get; set; }

        [JsonProperty("content")]
        [JsonConverter(typeof(DictionaryConverter<MediaType>))]
        public IDictionary<string, MediaType>? Content { get; set; }

        public override string ToString()
        {
            var decorator = In switch
            {
                ParameterLocation.Query => $"[Query(\"{Name}\")]",
                ParameterLocation.Header => $"[Header(\"{Name}\")]",
                _ => string.Empty
            };

            if (Schema != null)
            {
                return $"{decorator} {Schema.GetTypeName()} {Name}";
            }

            return $"{decorator} {Content.First().Value.GetTypeName()} {Name}";
        }

        public static string GetAsQueryClass(string operationName, IEnumerable<Parameter> parameters)
        {
            var properties = parameters
                .Where(p => p.In == ParameterLocation.Query)
                .SelectMany(p => p.Schema?.GetProperties() ?? Array.Empty<string>())
                .Concat(parameters.SelectMany(p => p.Content?.First().Value.GetProperties() ?? Array.Empty<string>()))
                .Distinct()
                .ToList();

            if (properties.Any())
            {
                return string.Empty;
            }

            var template = Template.Parse(CodeBoilerplates.ApiClientType);
            return template.Render(new
            {
                Namespace = ApiDefinition.GetNamespace(),
                Name = GetAsQueryParamsClassName(operationName),
                Properties = properties
            });
        }

        public static string GetAsQueryParamsClassName(string operationName) => $"{operationName}QueryParams";
    }
}
