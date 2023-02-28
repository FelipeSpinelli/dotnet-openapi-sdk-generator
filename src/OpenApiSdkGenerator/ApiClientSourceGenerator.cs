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
        private static readonly DiagnosticDescriptor InvalidJsonError = new DiagnosticDescriptor(id: "OPENAPISDKGEN001",
                                                                                              title: "Couldn't parse json file",
                                                                                              messageFormat: "Couldn't parse json file '{0}' Reason[{1}].",
                                                                                              category: "OpenApiSdkGenerator",
                                                                                              DiagnosticSeverity.Error,
                                                                                              isEnabledByDefault: true);
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
            const string SDK_DEFINITIONS_FILENAME = "openapi-sdk-generator.yaml";

            var openapiFileName = string.Empty;
            try
            {
                var @namespace = context.Compilation?.AssemblyName ?? "OpenApiSdkGenerator";

                var openapiFile = context.AdditionalFiles
                    .FirstOrDefault(f => !f.Path.EndsWith(SDK_DEFINITIONS_FILENAME, StringComparison.InvariantCultureIgnoreCase));

                if (openapiFile == null)
                {
                    return;
                }

                openapiFileName = openapiFile.Path;

                var apiClientNameDefined = context
                    .AnalyzerConfigOptions
                    .GetOptions(openapiFile)
                    .TryGetValue("build_metadata.AdditionalFiles.GeneratedApiClientName", out var generatedApiClientName);

                var jsonContent = openapiFile.GetText(context.CancellationToken)?.ToString();

                if (jsonContent == null)
                {
                    return;
                }

                ApiDefinition.SetNamespace(@namespace);
                LoadSdkDefinitions(context, SDK_DEFINITIONS_FILENAME);
                var apiDefinition = JsonConvert.DeserializeObject<ApiDefinition>(jsonContent.Replace("$ref", "_reference"));
                if (apiDefinition == null)
                {
                    return;
                }


                apiDefinition.RegisterReferences();
                apiDefinition.GenerateTypes(context);                

                var apiClientName = apiClientNameDefined ? generatedApiClientName : "ApiClient";
                var content = GetContent(apiClientName, apiDefinition);

                context.AddSource($"{apiClientName}.g.cs", SourceText.From(content, Encoding.UTF8));
            }
            catch (Exception ex)
            {
                context.ReportDiagnostic(Diagnostic.Create(InvalidJsonError, Location.None, openapiFileName, ex.Message));
            }
        }

        private static void LoadSdkDefinitions(GeneratorExecutionContext context, string SDK_DEFINITIONS_FILENAME)
        {
            var sdkDefinitionsText = context.AdditionalFiles
                .FirstOrDefault(f => f.Path.EndsWith(SDK_DEFINITIONS_FILENAME, StringComparison.InvariantCultureIgnoreCase))?
                .GetText(context.CancellationToken)?.ToString() ?? string.Empty;

            ApiDefinition.LoadApiDefinitionOptions(sdkDefinitionsText);
        }

        private string GetContent(string apiClientName, ApiDefinition apiDefinition)
        {
            var template = Template.Parse(CodeBoilerplates.ApiClientInterface);
            return template.Render(new
            {
                Namespace = ApiDefinition.GetNamespace(),
                ApiClientName = apiClientName,
                ApiDefinition = apiDefinition,
                apiDefinition.Operations
            });
        }
    }
}
