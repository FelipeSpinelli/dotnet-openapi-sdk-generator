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
            var decorator = In switch
            {
                ParameterLocation.Query => $"[Query(\"{Name}\")]\r\n",
                ParameterLocation.Header => $"[Header(\"{Name}\")]\r\n",
                _ => string.Empty
            };

            return $"{decorator}public {(Schema != null ? Schema.GetTypeName() : Content.First().Value.GetTypeName())} {Name} {{ get; set; }}";
        }

        public static string GetAsQueryClass(string operationName, IEnumerable<Parameter> parameters)
        {
            var typeOptions = ApiDefinition.GetTypeOptions(GetAsQueryParamsClassName(operationName));

            if (typeOptions.Ignore)
            {
                return string.Empty;
            }

            if (parameters.Any(p => p.In != ParameterLocation.Query))
            {
                throw new ArgumentException("Query parameters class only accepts parameter which In equals 'query'!");
            }

            var properties = parameters
                .Where(p => p.In == ParameterLocation.Query)
                .Select(p => p.ToString().Replace($" {p.Name} ", $" {p.Name.ToPascalCase()} "))
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
                Name = typeOptions.GetName(),
                Properties = properties,
                Type = "class",
                ToString = CodeBoilerplates.ApiQueryClassToStringMethod
            });
        }

        public static string GetAsQueryParamsClassName(string operationName)
        {
            var name = $"{operationName}QueryParams";
            var typeOptions = ApiDefinition.GetTypeOptions(name);

            return typeOptions.GetName();
        }
    }
}
