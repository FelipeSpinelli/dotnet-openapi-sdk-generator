using Microsoft.CodeAnalysis;
using OpenApiSdkGenerator.Models.OpenApi;
using OpenApiSdkGenerator.Models.Sdk;
using System.Collections.Generic;
using System.Threading;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace OpenApiSdkGenerator.Models;

public sealed record ApiClient
{
    private static ApiClientSettings _settings = new();
    private List<ApiDefinition> _definitions = [];

    public string Name { get; private set; } = null!;
    public ApiClientMethodCollection Methods { get; set; } = new();
    public ApiClientModelCollection Models { get; set; } = new();

    public void LoadOpenApiFrom(AdditionalText file)
    {
        var content = file.GetText(CancellationToken.None)?.ToString() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(content))
        {
            return;
        }

        var openApiDefinition = file.Path.EndsWith(".json") ?
            ApiDefinition.LoadJson(content) :
            ApiDefinition.LoadYaml(content);

        if (openApiDefinition == null)
        {
            return;
        }

        _definitions.Add(openApiDefinition);
    }

    public void Build()
    {
        Name = _settings.ApiName ?? ApiClientSettings.DEFAULT_APINAME;

    }

    public static void LoadSettingsFrom(string rawSettings)
    {
        if (string.IsNullOrEmpty(rawSettings))
        {
            _settings = new();
            return;
        }

        if (_settings != null)
        {
            return;
        }

        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        _settings = deserializer.Deserialize<ApiClientSettings>(rawSettings);
    }
}