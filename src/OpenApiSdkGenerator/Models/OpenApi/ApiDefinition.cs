using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Newtonsoft.Json;
using OpenApiSdkGenerator.Extensions;
using OpenApiSdkGenerator.JsonConverters;
using OpenApiSdkGenerator.Models.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenApiSdkGenerator.Models.OpenApi;
public record ApiDefinition
{
    private static ApiDefinition _current = new();

    private readonly IDictionary<string, Schema> _globalReferences = new Dictionary<string, Schema>();
    private readonly IDictionary<string, string> _queryParamsClass = new Dictionary<string, string>();
    private static string _namespace = string.Empty;

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
        .Select(operation => operation.ApplySdkOptions(ApiClientSettings.Instance))
        .Where(operation => operation.ShouldBeGenerated);

    public static void SetNamespace(string @namespace) => _namespace = @namespace;
    public static string GetNamespace() => _namespace;
    public static ApiDefinition Current => _current;

    public void RegisterReferences()
    {
        _globalReferences.Clear();
        if (Components == null)
        {
            return;
        }

        foreach (var schema in Components.Schemas)
        {
            var typeOptions = ApiClientSettings.Instance.GetTypeOptions(schema.Key);
            var newSchema = schema.Value with { Name = typeOptions.GetName(), OriginalName = typeOptions.Name };
            _globalReferences.Add($"#/components/schemas/{Info.GetVersion()}/{schema.Key}", newSchema);
        }
    }

    public void GenerateTypes(GeneratorExecutionContext context)
    {
        context.AddSource(
            "NoContentResponse.g.cs",
            SourceText.From(
                CodeBoilerplates.NoContentResponse
                    .Replace("{{ namespace }}", _namespace),
                Encoding.UTF8
            )
        );
        context.AddSource(
            "OpenApiSdkGeneratorUtils.g.cs",
            SourceText.From(
                CodeBoilerplates.OpenApiSdkGeneratorUtils
                    .Replace("{{ namespace }}", _namespace)
                    .Replace("{{ types_formatting_list }}", ApiClientSettings.Instance.QuerySerialization.ToString()),
                Encoding.UTF8
            )
        );
        context.AddSource(
            $"{GetApiName()}ServicesCollectionExtensions.g.cs",
            SourceText.From(
                CodeBoilerplates.SdkServicesCollectionExtensions
                    .Replace("{{ namespace }}", _namespace)
                    .Replace("{{ api_name }}", GetApiName())
                    .Replace("{{ api_client_name }}", GetApiClientName()),
                Encoding.UTF8
            )
        );
        context.AddSource(
            "RequestCustomHeadersInjector.g.cs",
            SourceText.From(
                CodeBoilerplates.RequesCustomHeadersInjector
                    .Replace("{{ namespace }}", _namespace),
                Encoding.UTF8
            )
        );

        foreach (var schema in _globalReferences.Where(x => x.Value.ShouldGenerate()).Select(x => x.Value))
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

    public void JoinComponents(Components? components)
    {
        Components ??= new Components();
        Components.Concat(components);
    }

    public void JoinPaths(IDictionary<string, PathItem> paths)
    {
        Paths ??= new Dictionary<string, PathItem>();
        Paths.Merge(paths);
    }

    public Schema? GetSchemaByReference(string reference) => _globalReferences.ContainsKey(reference ?? string.Empty)
        ? _globalReferences[reference]
        : null;

    public string GetApiClientName()
    {
        return $"I{GetApiName()}";
    }

    public string GetApiName()
    {
        return string.IsNullOrWhiteSpace(ApiClientSettings.Instance.ApiName)
            ? ApiClientSettings.DEFAULT_APINAME
            : ApiClientSettings.Instance.ApiName;
    }

    public void SetAsCurrent() => _current = this;

    public static ApiDefinition? LoadJson(string json)
    {
        json ??= string.Empty;
        if (string.IsNullOrWhiteSpace(json))
        {
            return null;
        }

        var info = Info.LoadFrom(json);
        var jsonCleaned = json
            .Replace("$ref", "_reference")
            .Replace("#/components/schemas/", $"#/components/schemas/{info.GetVersion()}/");

        return JsonConvert.DeserializeObject<ApiDefinition>(jsonCleaned);
    }

    public static ApiDefinition? LoadYaml(string yaml)
    {
        yaml ??= string.Empty;
        return null;
    }
}
