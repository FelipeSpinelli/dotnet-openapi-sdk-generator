using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OpenApiSdkGenerator.Models;
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

            try
            {
                var apiDefinition = JsonConvert.DeserializeObject<ApiDefinition>(jsonContent.Replace("$ref", "_reference"));
                
                var content = GetContent(@namespace, apiDefinition);

                context.AddSource($"{(apiClientNameDefined ? generatedApiClientName : "ApiClient")}.g.cs", SourceText.From(content, Encoding.UTF8));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        static string GetContent(string @namespace, ApiDefinition apiDefinition)
        {
            return "namespace A { public class X { } }";
        //    var template = Template.Parse(CodeBoilerplates.MockController);
        //    return template.Render(new { Namespace = @namespace, Mocks = mocks });
        }
    }
}
