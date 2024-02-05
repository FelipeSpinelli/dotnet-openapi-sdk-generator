using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OpenApiSdkGenerator.Models.OpenApi;
using OpenApiSdkGenerator.Models.Sdk;
using Scriban;
using System;
#if DEBUG
using System.Diagnostics;
#endif
using System.Linq;
using System.Text;

namespace OpenApiSdkGenerator
{
    [Generator]
    public class ApiClientSourceGenerator : ISourceGenerator
    {
        const string SDK_DEFINITIONS_FILENAME = "openapi-sdk-generator.yaml";

        private static readonly string[] _openApiFileValidExtensions = new[] { ".json" };
        private static readonly DiagnosticDescriptor InvalidJsonError = new(id: "OPENAPISDKGEN001",
                                                                            title: "Couldn't parse json file",
                                                                            messageFormat: "Couldn't parse json file '{0}' Reason[{1}]",
                                                                            category: "OpenApiSdkGenerator",
                                                                            DiagnosticSeverity.Warning,
                                                                            isEnabledByDefault: true);
        private ApiDefinition _definition = new();
        private string _openApiFileName = string.Empty;

        public void Initialize(GeneratorInitializationContext context)
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.None,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                MissingMemberHandling = MissingMemberHandling.Ignore,
            };

#if DEBUG
            Debugger.Launch();
#endif
        }

        public void Execute(GeneratorExecutionContext context)
        {
            try
            {
                LoadDefinitions(context);
                AddApiClientInterfaceSource(context);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error: {ex.Message}\r\nStackTrace: {ex.StackTrace}";
                context.ReportDiagnostic(Diagnostic.Create(InvalidJsonError, Location.None, _openApiFileName, errorMessage));
            }
        }

        private void LoadDefinitions(GeneratorExecutionContext context)
        {
            var @namespace = context.Compilation?.AssemblyName ?? "OpenApiSdkGenerator";

            var openapiFiles = context.AdditionalFiles
                .Where(f => _openApiFileValidExtensions.Any(x => f.Path.EndsWith(x)));

            if (openapiFiles is null || !openapiFiles.Any())
            {
                return;
            }

            ApiDefinition.SetNamespace(@namespace);
            ApiClientSettings.LoadFrom(GetRawSdkOptions(context));
            var apiDefinitions = new ApiDefinitionCollection();

            foreach (var openapiFile in openapiFiles)
            {
                _openApiFileName = openapiFile.Path;
                var json = openapiFile.GetText(context.CancellationToken)?.ToString();

                if (string.IsNullOrEmpty(json))
                {
                    return;
                }

                var definition = ApiDefinition.LoadJson(json);
                if (definition == null)
                {
                    return;
                }

                definition.RegisterReferences();

                apiDefinitions.Add(definition);
            }

            _definition = apiDefinitions.Concat();
            _definition.SetAsCurrent();
            _definition.GenerateTypes(context);
        }

        private void AddApiClientInterfaceSource(GeneratorExecutionContext context)
        {
            var content = GetContent();
            context.AddSource($"{_definition.GetApiClientName()}.g.cs", SourceText.From(content, Encoding.UTF8));
        }

        private static string GetRawSdkOptions(GeneratorExecutionContext context)
        {
            return context.AdditionalFiles
                .FirstOrDefault(f => f.Path.EndsWith(SDK_DEFINITIONS_FILENAME, StringComparison.InvariantCultureIgnoreCase))?
                .GetText(context.CancellationToken)?.ToString() ?? string.Empty;
        }

        private string GetContent()
        {
            var template = Template.Parse(CodeBoilerplates.ApiClientInterface);
            return template.Render(new
            {
                Usings = ApiClientSettings.Instance.Usings ?? Array.Empty<string>(),
                Namespace = ApiDefinition.GetNamespace(),
                ApiClientName = _definition.GetApiClientName(),
                ApiDefinition = _definition,
                _definition.Operations
            });
        }
    }
}
