using OpenApiSdkGenerator.Models.OpenApi;
using OpenApiSdkGenerator.Models.Settings.Enumerators;
using System;
using System.Linq;

namespace OpenApiSdkGenerator.Models.Sdk;

public record ApiClientSettings
{
    private static ApiClientSettings? _instance;

    internal const string DEFAULT_APINAME = "MyApi";

    private string _apiName = DEFAULT_APINAME;
    public string ApiName
    {
        get {  return _apiName; }
        set { _apiName = string.IsNullOrWhiteSpace(value) ? DEFAULT_APINAME : value; }
    }

    public string[] Usings { get; set; } = Array.Empty<string>();
    public string[] DefaultOperationAttributes { get; set; } = Array.Empty<string>();
    public OperationSettings[] Operations { get; set; } = Array.Empty<OperationSettings>();
    public DuplicatedOperationApproachType DuplicatedOperationApproach { get; set; }
    public TypeSettings[] Types { get; set; } = Array.Empty<TypeSettings>();
    public QuerySerializationSettings QuerySerialization { get; set; } = new();

    public static ApiClientSettings Instance => _instance ?? new();

    public bool ShouldGenerate(Operation operation)
    {
        return Operations.Any(x => x.Name.Equals(operation.GetName()) && !x.Ignore);
    }

    public TypeSettings GetTypeOptions(string typeName, string apiVersion)
    {
        if (Types == null)
        {
            return TypeSettings.Default(typeName);
        }

        var typeOptions = Types.FirstOrDefault(x => x.Name.Equals(typeName));

        return typeOptions == null ?
            TypeSettings.Default(typeName) :
            typeOptions;
    }

    public string GetQueryAttribute()
    {
        return QuerySerialization.SerializeAsRawString ?
            "[RawQueryString]" :
            "[Query(QuerySerializationMethod.Serialized)]";
    }

    public static void LoadFrom(string raw)
    {
        
    }
}
