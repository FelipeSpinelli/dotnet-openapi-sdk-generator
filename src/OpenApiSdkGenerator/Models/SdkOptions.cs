using System;
using System.Linq;

namespace OpenApiSdkGenerator.Models
{
    public record SdkOptions
    {
        internal const string DEFAULT_APINAME = "MyApi";
        public string ApiName { get; set; } = DEFAULT_APINAME;
        public string[] Usings { get; set; } = Array.Empty<string>();
        public string[] DefaultOperationAttributes { get; set; } = Array.Empty<string>();
        public SdkOperationOptions[] Operations { get; set; } = Array.Empty<SdkOperationOptions>();
        public SdkTypeOptions[] Types { get; set; } = Array.Empty<SdkTypeOptions>();
        public SdkQuerySerializationOptions QuerySerialization { get; set; } = new();

        public bool ShouldGenerate(Operation operation)
        {
            return Operations.Any(x => x.Name.Equals(operation.GetName()) && !x.Ignore);
        }

        public SdkTypeOptions GetTypeOptions(string typeName)
        {
            if (Types == null)
            {
                return new SdkTypeOptions { Name = typeName };
            }

            var typeOptions = Types.FirstOrDefault(x => x.Name.Equals(typeName));
            
            return typeOptions == null
                ? new SdkTypeOptions { Name = typeName }
                : typeOptions;
        }

        public string GetQueryAttribute()
        {
            return QuerySerialization.SerializeAsRawString 
                ? "[RawQueryString]"
                : "[Query(QuerySerializationMethod.Serialized)]";
        }
    }
}
