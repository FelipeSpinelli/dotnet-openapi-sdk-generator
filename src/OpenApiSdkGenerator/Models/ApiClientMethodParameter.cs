namespace OpenApiSdkGenerator.Models;

public sealed record ApiClientMethodParameter
{
    public string Name { get; set; } = null!;
    public string SerializationName { get;set; } = null!;
    public ApiClientMethodParameterSource Source { get; set; }
    public ApiClientModel Type { get; set; } = new();
}
