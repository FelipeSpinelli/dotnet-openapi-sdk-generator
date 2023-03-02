using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Newtonsoft.Json;
using OpenApiSdkGenerator.JsonConverters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;

namespace OpenApiSdkGenerator.Models
{
    public record ApiDefinition
    {
        private static ApiDefinition _current = new();

        private readonly IDictionary<string, Schema> _globalReferences = new Dictionary<string, Schema>();
        private readonly IDictionary<string, string> _queryParamsClass = new Dictionary<string, string>();
        private string _namespace;
        public SdkOptions? Options { get; private set; }

        [JsonProperty("openapi")]
        public string OpenApiVersion { get; set; } = "3.0.1";

        [JsonProperty("info")]
        public Info Info { get; set; } = new();

        [JsonProperty("servers")]
        public Server[] Servers { get; set; } = Array.Empty<Server>();

        [JsonProperty("paths")]
        [JsonConverter(typeof(DictionaryConverter<PathItem>))]
        public IDictionary<string, PathItem> Paths { get; set; } = new Dictionary<string, PathItem>(0);

        [JsonProperty("components")]
        public Components? Components { get; set; }

        public IEnumerable<Operation> Operations => Paths
            .SelectMany(path => path.Value.GetOperations(path.Key))
            .Select(operation => operation.ApplySdkOptions(Options))
            .Where(operation => operation.ShouldBeGenerated);

        public void SetNamespace(string @namespace) => _namespace = @namespace;
        public static string GetNamespace() => _current._namespace;

        public void LoadApiDefinitionOptions(string rawOptions)
        {
            if (string.IsNullOrEmpty(rawOptions))
            {
                Options = new();
                return;
            }

            if (Options != null)
            {
                return;
            }

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            Options = deserializer.Deserialize<SdkOptions>(rawOptions);
        }
        public void RegisterReferences()
        {
            _globalReferences.Clear();
            if (Components == null)
            {
                return;
            }

            foreach (var schema in Components.Schemas)
            {
                schema.Value.SetName(schema.Key);
                _globalReferences.Add($"#/components/schemas/{schema.Key}", schema.Value);
            }
        }

        public void GenerateTypes(GeneratorExecutionContext context)
        {
            context.AddSource("NoContentResponse.g.cs", SourceText.From(CodeBoilerplates.NoContentResponse, Encoding.UTF8));

            foreach (var schema in _globalReferences.Select(x => x.Value))
            {
                context.AddSource($"{schema.Name}.g.cs", SourceText.From(schema.ToString(), Encoding.UTF8));
            }

            foreach (var operation in Operations)
            {
                var queryParameters = operation.Parameters.Where(x => x.In == Enumerators.ParameterLocation.Query);

                if (!queryParameters.Any())
                {
                    continue;
                }

                var queryParamsClassName = Parameter.GetAsQueryParamsClassName(operation.GetName());

                if (_queryParamsClass.ContainsKey(queryParamsClassName))
                {
                    continue;
                }

                var content = Parameter.GetAsQueryClass(operation.GetName(), queryParameters);

                if (string.IsNullOrWhiteSpace(content))
                {
                    continue;
                }

                context.AddSource($"{queryParamsClassName}.g.cs", SourceText.From(content, Encoding.UTF8));
                _queryParamsClass.Add(queryParamsClassName, content);
            }
        }

        public static Schema? GetSchemaByReference(string reference) => _current._globalReferences.ContainsKey(reference ?? string.Empty)
            ? _current._globalReferences[reference]
            : null;

        public void SetAsCurrent() => _current = this;
    }
}
