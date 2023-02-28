using System;
using System.Linq;

namespace OpenApiSdkGenerator.Models
{
    public record SdkOptions
    {
        public string[] Usings { get; set; } = Array.Empty<string>();
        public string[] DefaultOperationAttributes { get; set; } = Array.Empty<string>();
        public SdkOperationOptions[] Operations { get; set; } = Array.Empty<SdkOperationOptions>();

        public bool ShouldGenerate(Operation operation)
        {
            return Operations.Any(x => x.Name.Equals(operation.GetName()) && !x.Ignore);
        }
    }
}
