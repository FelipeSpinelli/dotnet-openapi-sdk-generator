using System.Runtime.Serialization;

namespace OpenApiSdkGenerator.Models.OpenApi.Enumerators;

public enum DataType
{
    Reference,
    [EnumMember(Value = "integer")]
    Integer,
    [EnumMember(Value = "number")]
    Number,
    [EnumMember(Value = "string")]
    String,
    [EnumMember(Value = "array")]
    Array,
    [EnumMember(Value = "object")]
    Object,
    [EnumMember(Value = "boolean")]
    Boolean
}
