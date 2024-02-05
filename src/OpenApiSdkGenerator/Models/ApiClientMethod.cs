using OpenApiSdkGenerator.Models.Sdk;
using OpenApiSdkGenerator.Models.Settings.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenApiSdkGenerator.Models;

public sealed record ApiClientMethod
{
    public string Name { get; set; } = null!;
    public uint Version { get; set; } = 0;
    public IEnumerable<string> Attributes { get; set; } = Enumerable.Empty<string>();
    public ApiClientModel ReturnType { get; set; } = null!;
    public List<ApiClientMethodParameter> Parameters { get; set; } = [];
}
public sealed class ApiClientMethodCollection
{
    private readonly List<ApiClientMethod> _methods = [];

    public void Add(ApiClientMethod apiClientMethod)
    {
        _methods.Add(apiClientMethod);
    }

    public void ApplySettings(ApiClientSettings settings)
    {
        var methods = new List<ApiClientMethod>(_methods.Count);

        foreach (var method in _methods.OrderBy(x => x.Name).ThenBy(x => x.Version))
        {
            ApiClientMethod? handledMethod = ApplyVersioningApproach(settings, method);
            if (handledMethod is null)
            {
                continue;
            }

            handledMethod = handledMethod with
            {
                
            }

            methods.Add(handledMethod);
        }
    }

    private ApiClientMethod? ApplyVersioningApproach(ApiClientSettings settings, ApiClientMethod? method)
    {
        var hasGreaterVersion = _methods.Any(x => x.Name.Equals(method.Name) && x.Version > method.Version);

        Func<ApiClientMethod?> versionHandling = (hasGreaterVersion, settings.DuplicatedOperationApproach) switch
        {
            (false, _) => () => method,
            (_, DuplicatedOperationApproachType.IgnoreLessVersions) => () => null,
            (_, DuplicatedOperationApproachType.GenerateWithVersionSuffix) => () => method with { Name = $"{method.Name}V{method.Version}" },
            (_, DuplicatedOperationApproachType.SimplifyNamesAndMarkAsObsolete) => () => method with { Attributes = new List<string> { "[Obselete]" } },
            (_, _) => () => method
        };

        var handledMethod = versionHandling();
        return handledMethod;
    }
}