using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Newtonsoft.Json;
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
        public void Initialize(GeneratorInitializationContext context)
        {
#if DEBUG
            Debugger.Launch();
#endif
        }

        public void Execute(GeneratorExecutionContext context)
        {
            const string OPENAPI_SPECIFICATION_FILENAME = "openapi.json";

            var @namespace = context.Compilation?.AssemblyName ?? "OpenApiSdkGenerator";

            var openapiFile = context.AdditionalFiles
                .Where(f => f.Path.EndsWith(OPENAPI_SPECIFICATION_FILENAME, StringComparison.InvariantCultureIgnoreCase))
                .FirstOrDefault();

            if (openapiFile == null)
            {
                return;
            }

            var apiClientNameDefined = context
                .AnalyzerConfigOptions
                .GetOptions(openapiFile)
                .TryGetValue("build_metadata.AdditionalFiles.GeneratedApiClientName", out var generatedApiClientName);

            var jsonContent = openapiFile.GetText(context.CancellationToken)?.ToString();

            if (jsonContent == null)
            {
                return;
            }

            var apiDefinition = JsonConvert.DeserializeObject<ApiDefinition>(jsonContent);

            var content = GetContent(@namespace, apiDefinition);

            context.AddSource($"{(apiClientNameDefined ? generatedApiClientName : "ApiClient")}.g.cs", SourceText.From(content, Encoding.UTF8));
        }

        static string GetContent(string @namespace, ApiDefinition apiDefinition)
        {
            return "namespace A { public class X { } }";
            //var template = Template.Parse(CodeBoilerplates.MockController);
            //return template.Render(new { Namespace = @namespace, Mocks = mocks });
        }
    }
}
