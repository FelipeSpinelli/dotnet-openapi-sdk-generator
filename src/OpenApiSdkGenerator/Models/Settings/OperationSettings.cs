using System;

namespace OpenApiSdkGenerator.Models.Sdk;

public record OperationSettings
{
    public string Name { get; set; } = null!;
    public bool Ignore { get; set; }
    public string[] Attributes { get; set; } = Array.Empty<string>();
    public bool CustomHeadersParameterEnabled { get; set; }
}