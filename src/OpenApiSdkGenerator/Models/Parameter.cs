using Newtonsoft.Json;
using OpenApiSdkGenerator.Extensions;
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
            var nameAsPascalCase = Name.ToPascalCase();
            var decorator = In switch
            {
                ParameterLocation.Query => $"[Query(\"{nameAsPascalCase}\")]\r\n",
                ParameterLocation.Header => $"[Header(\"{nameAsPascalCase}\")]\r\n",
                _ => string.Empty
            };

            return $"{decorator}public {(Schema != null ? Schema.GetTypeName() : Content.First().Value.GetTypeName())} {nameAsPascalCase} {{ get; set; }}";
        }

        public static string GetAsQueryClass(string operationName, IEnumerable<Parameter> parameters)
        {
            if (parameters.Any(p => p.In != ParameterLocation.Query))
            {
                throw new ArgumentException("Query parameters class only accepts parameter which In equals 'query'!");
            }

            var properties = parameters
                .Where(p => p.In == ParameterLocation.Query)
                .Select(p => p.ToString())
                .Distinct()
                .ToList();

            if (!properties.Any())
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
