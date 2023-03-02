using System.Runtime.Serialization;

namespace OpenApiSdkGenerator.Models.Enumerators
{
    public enum DataFormat
    {
        [EnumMember(Value = "int32")]
        Int32,
        [EnumMember(Value = "int64")]
        Int64,
        [EnumMember(Value = "float")]
        Float,
        [EnumMember(Value = "double")]
        Double,
        [EnumMember(Value = "password")]
        Password,
        [EnumMember(Value = "uri-reference")]
        UriReference,
        [EnumMember(Value = "date-time")]
        DateTime,
        [EnumMember(Value = "email")]
        Email,
        [EnumMember(Value = "binary")]
        Binary
    }
}
