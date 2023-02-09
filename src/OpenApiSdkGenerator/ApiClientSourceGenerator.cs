using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OpenApiSdkGenerator.Models;
using Scriban;
using Scriban.Runtime;
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
        private readonly TemplateContext _templateContext = new();
        private ScriptObject _scriptObject;

        public void Initialize(GeneratorInitializationContext context)
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.None,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                MissingMemberHandling = MissingMemberHandling.Ignore,
            };

            var filters = new ScriptObject();
            filters.Import(typeof(TemplateFilter));
            _scriptObject = new ScriptObject
            {
                { "filters", filters }
            };            
#if DEBUG
            Debugger.Launch();
#endif
        }

        public void Execute(GeneratorExecutionContext context)
        {
            const string OPENAPI_SPECIFICATION_FILENAME = "openapi.json";

            var openapiFileName = string.Empty;
            try
            {
                var @namespace = context.Compilation?.AssemblyName ?? "OpenApiSdkGenerator";

                var openapiFile = context.AdditionalFiles
                    .Where(f => f.Path.EndsWith(OPENAPI_SPECIFICATION_FILENAME, StringComparison.InvariantCultureIgnoreCase))
                    .FirstOrDefault();

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


                var apiDefinition = JsonConvert.DeserializeObject<ApiDefinition>(jsonContent.Replace("$ref", "_reference"));
                var apiClientName = apiClientNameDefined ? generatedApiClientName : "ApiClient";
                var content = GetContent(@namespace, apiClientName, apiDefinition);

                context.AddSource($"{apiClientName}.g.cs", SourceText.From(content, Encoding.UTF8));
            }
            catch (Exception ex)
            {
                context.ReportDiagnostic(Diagnostic.Create(InvalidJsonError, Location.None, openapiFileName, ex.Message));
            }
        }

        private string GetContent(string @namespace, string apiClientName, ApiDefinition apiDefinition)
        {
            
            const string boilerplate =  @$"
namespace {{{{ namespace }}}}
{{
    public interface {{{{ api_client_name }}}}
    {{
        {{{{ for operation in operations }}}}
            [{{{{ operation.http_method }}}}(""{{{{ operation.path }}}}"")]
            public Task<ApiResponse<{{{{ operation | filters.get_success_response_type }}}}>> {{{{ operation | filters.get_name }}}}({{{{ operation | filters.get_method_signature }}}});
        {{{{ end }}}}
    }}
}}";
            var template = Template.Parse(boilerplate);
            _scriptObject.Import(new
            {
                Namespace = @namespace,
                ApiClientName = apiClientName,
                ApiDefinition = apiDefinition,
                Operations = apiDefinition.Operations
            });
            _templateContext.PushGlobal(_scriptObject);
            return template.Render(_templateContext);
        }
    }
}
