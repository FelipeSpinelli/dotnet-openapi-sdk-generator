using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OpenApiSdkGenerator.Models;
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

        private static readonly string[] _openApiFileValidExtensions = new[] { ".json", ".yaml", ".yml" };
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

            //#if DEBUG
            //            Debugger.Launch();
            //#endif
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
                .Where(f => _openApiFileValidExtensions.Contains(f.Path));

            if (openapiFiles is null || !openapiFiles.Any())
            {
                return;
            }

            _openApiFileName = openapiFiles.Path;

            var jsonContent = openapiFiles.GetText(context.CancellationToken)?.ToString();

            if (string.IsNullOrEmpty(jsonContent))
            {
                return;
            }

            _definition = JsonConvert.DeserializeObject<ApiDefinition>(jsonContent!.Replace("$ref", "_reference"));
            if (_definition == null)
            {
                return;
            }

            _definition.SetNamespace(@namespace);
            LoadSdkDefinitions(context, _definition);
            _definition.SetAsCurrent();
            _definition.RegisterReferences();
            _definition.GenerateTypes(context);
        }

        private void AddApiClientInterfaceSource(GeneratorExecutionContext context)
        {
            var content = GetContent();
            context.AddSource($"{ApiDefinition.GetApiClientName()}.g.cs", SourceText.From(content, Encoding.UTF8));
        }

        private static void LoadSdkDefinitions(GeneratorExecutionContext context, ApiDefinition apiDefinition)
        {
            var sdkDefinitionsText = context.AdditionalFiles
                .FirstOrDefault(f => f.Path.EndsWith(SDK_DEFINITIONS_FILENAME, StringComparison.InvariantCultureIgnoreCase))?
                .GetText(context.CancellationToken)?.ToString() ?? string.Empty;

            apiDefinition.LoadApiDefinitionOptions(sdkDefinitionsText);
        }

        private string GetContent()
        {
            var template = Template.Parse(CodeBoilerplates.ApiClientInterface);
            return template.Render(new
            {
                Usings = _definition.Options?.Usings ?? Array.Empty<string>(),
                Namespace = ApiDefinition.GetNamespace(),
                ApiClientName = ApiDefinition.GetApiClientName(),
                ApiDefinition = _definition,
                _definition.Operations
            });
        }
    }
}
