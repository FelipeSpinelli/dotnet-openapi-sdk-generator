using OpenApiSdkGenerator.Models.OpenApi;
using System.Collections.Generic;

namespace OpenApiSdkGenerator.Models;

public sealed record ApiClientModel
{
    public string Name { get; set; } = null!;
    public string Version { get; set; } = null!;
    public Schema Schema { get; set; } = new();
}
public sealed class ApiClientModelCollection
{
    private readonly List<ApiClientModel> _models = [];


}