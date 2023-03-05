using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OpenApiSdkGenerator.Models;
using System;
using System.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace OpenApiSdkGenerator
{
    public class Processor
    {
        private const string SDK_DEFINITIONS_FILENAME = "openapi-sdk-generator.yaml";

        private static readonly IDeserializer _deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

        private readonly ApiDefinition _openApiDocument = new();
        private readonly SdkOptions _sdkOptions;
        private readonly GeneratorExecutionContext _context;
        private readonly bool _shouldProcess;

        public Processor(GeneratorExecutionContext context)
        {
            _context = context;
            _sdkOptions = GetSdkOptions(context);

            var openApiDocument = GetOpenApiDocument(context);
            if (openApiDocument == null)
            {
                _shouldProcess = false;
                return;
            }

            _shouldProcess = true;
            _openApiDocument = openApiDocument;
        }

        public void Process()
        {
            if (!_shouldProcess)
            {
                return;
            }

            foreach (var operation in _openApiDocument.Operations)
            {
                var operationStr = operation.ToString();
                Console.WriteLine(operationStr);
            }
        }

        private static SdkOptions GetSdkOptions(GeneratorExecutionContext context)
        {
            var sdkDefinitionsText = context.AdditionalFiles
                .FirstOrDefault(f => f.Path.EndsWith(SDK_DEFINITIONS_FILENAME, StringComparison.InvariantCultureIgnoreCase))?
                .GetText(context.CancellationToken)?.ToString() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(sdkDefinitionsText))
            {
                return new SdkOptions
                {
                    Namespace = context.Compilation?.AssemblyName ?? "MyApiSdk",
                    ApiName = SdkOptions.DEFAULT_APINAME,
                    DefaultOperationAttributes = Array.Empty<string>(),
                    Operations = Array.Empty<SdkOperationOptions>(),
                    Types = Array.Empty<SdkTypeOptions>(),
                    Usings = Array.Empty<string>()
                };
            }

            var sdkOptions = _deserializer.Deserialize<SdkOptions>(sdkDefinitionsText);
            return sdkOptions with
            {
                Namespace = sdkOptions.Namespace ?? context.Compilation?.AssemblyName ?? SdkOptions.DEFAULT_NAMESPACE,
            };
        }

        private static ApiDefinition? GetOpenApiDocument(GeneratorExecutionContext context)
        {
            var openApiFile = context.AdditionalFiles
                .FirstOrDefault(f => !f.Path.EndsWith(SDK_DEFINITIONS_FILENAME, StringComparison.InvariantCultureIgnoreCase));

            if (openApiFile == null)
            {
                return null;
            }

            var openApiFileContent = openApiFile.GetText(context.CancellationToken)?.ToString();
            if (string.IsNullOrEmpty(openApiFileContent))
            {
                return null;
            }

            return openApiFile.Path.EndsWith("json")
                ? JsonConvert.DeserializeObject<ApiDefinition>(openApiFileContent!, new JsonSerializerSettings{
                    Formatting = Formatting.Indented,
                    TypeNameHandling = TypeNameHandling.None,
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    MissingMemberHandling = MissingMemberHandling.Ignore })
                : _deserializer.Deserialize<ApiDefinition>(openApiFileContent!);
        }
    }
}
