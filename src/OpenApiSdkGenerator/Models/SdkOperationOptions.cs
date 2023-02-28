using System;

namespace OpenApiSdkGenerator.Models
{
    public record SdkOperationOptions
    {
        public string Name { get; set; } = null!;
        public bool Ignore { get; set; }
        public string[] Attributes { get; set; } = Array.Empty<string>();
    }
}
