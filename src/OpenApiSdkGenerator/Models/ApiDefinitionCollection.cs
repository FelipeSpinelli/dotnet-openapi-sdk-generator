using System.Collections.Generic;
using System.Linq;

namespace OpenApiSdkGenerator.Models;

public record ApiDefinitionCollection
{
    private readonly ApiDefinition _concatResult = new();
    private readonly List<ApiDefinition> _definitions = [];

    public void Add(ApiDefinition apiDefinition) => _definitions.Add(apiDefinition);

    public ApiDefinition Concat()
    {
        var result = _definitions.Count == 1 ?
            _definitions.Single() :
            InternalConcat();

        result.RegisterReferences();

        return result;
    }

    private ApiDefinition InternalConcat()
    {
        if (!_definitions.Any())
        {
            return _concatResult;
        }

        foreach (var apiDefinition in _definitions)
        {
            _concatResult.JoinComponents(apiDefinition.Components);
            _concatResult.JoinPaths(apiDefinition.Paths);
        }

        return _concatResult;
    }
}
