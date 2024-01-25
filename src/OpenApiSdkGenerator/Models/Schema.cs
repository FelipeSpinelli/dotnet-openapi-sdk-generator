using Newtonsoft.Json;
using OpenApiSdkGenerator.Extensions;
using OpenApiSdkGenerator.JsonConverters;
using OpenApiSdkGenerator.Models.Enumerators;
using Scriban;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenApiSdkGenerator.Models
{
    public record Schema
    {
        [JsonProperty("allOf")]
        public Schema[] AllOf { get; set; } = Array.Empty<Schema>();

        [JsonProperty("type")]
        public DataType Type { get; set; }

        [JsonProperty("format")]
        public DataFormat? Format { get; set; }

        [JsonProperty("enum")]
        public string[] EnumValues { get; set; } = Array.Empty<string>();

        [JsonProperty("nullable")]
        public bool Nullable { get; set; }

        [JsonProperty("required")]
        public string[] RequiredProperties { get; set; } = null!;

        [JsonProperty("properties")]
        [JsonConverter(typeof(DictionaryConverter<Schema>))]
        public IDictionary<string, Schema> Properties { get; set; } = new Dictionary<string, Schema>();

        [JsonProperty("_reference")]
        public string? Reference { get; set; }

        [JsonProperty("items")]
        public Schema? Items { get; set; }

        public string Name { get; set; } = null!;
        public string OriginalName { get; set; } = null!;

        public string? GetTypeName()
        {
            const string OBJECT_SCHEMA_NAME = "object";

            if (EnumValues.Any())
            {
                return $"enum{(Nullable ? "?" : string.Empty)}";
            }

            var name = (Type, Format) switch
            {
                (DataType.Reference, _) => Schema.GetByReference(Reference)?.Name ?? OBJECT_SCHEMA_NAME,
                (DataType.Integer, DataFormat.Int32) => "int",
                (DataType.Integer, DataFormat.Int64) => "long",
                (DataType.Integer, _) => "int",
                (DataType.Number, DataFormat.Float) => "float",
                (DataType.Number, DataFormat.Double) => "double",
                (DataType.Number, DataFormat.Decimal) => "decimal",
                (DataType.String, DataFormat.DateTime) => "DateTime",
                (DataType.String, _) => "string",
                (DataType.Array, _) => $"{Items?.GetTypeName() ?? OBJECT_SCHEMA_NAME}[]",
                (DataType.Object, _) => OBJECT_SCHEMA_NAME,
                (DataType.Boolean, _) => "bool",
                (_, _) => OBJECT_SCHEMA_NAME,
            };

            return $"{name}{(Nullable ? "?" : string.Empty)}";
        }

        public override string ToString()
        {
            var typeOptions = SdkOptions.Instance.GetTypeOptions(OriginalName);

            if (typeOptions.Ignore)
            {
                return string.Empty;
            }

            var properties = GetProperties()
                .Concat(AllOf.SelectMany(x => x.GetProperties()))
                .Distinct()
                .ToList();

            var template = Template.Parse(CodeBoilerplates.ApiClientType);
            return template.Render(new
            {
                Namespace = ApiDefinition.GetNamespace(),
                Name = typeOptions.GetName(),
                Properties = properties,
                Type = EnumValues.Any() ? "enum" : "class"
            });
        }

        public string[] GetProperties()
        {
            if (Type == DataType.Reference)
            {
                return Schema.GetByReference(Reference)?.GetProperties() ?? Array.Empty<string>();
            }

            if (EnumValues.Any())
            {
                return EnumValues.Select(x => $"{x},").ToArray();
            }

            return Properties
                .Select(x => $"public {x.Value.GetTypeName()} {x.Key.ToPascalCase()} {{ get; set; }}")
                .ToArray();
        }

        public bool ShouldGenerate() => !SdkOptions.Instance.GetTypeOptions(OriginalName).Ignore;

        public static Schema? GetByReference(string reference) => ApiDefinition.Current.GetSchemaByReference(reference);
    }
}
