using System.Collections.Generic;
using System.Text;

namespace OpenApiSdkGenerator.Models.Sdk
{
    public record QuerySerializationSettings
    {
        public bool SerializeAsRawString { get; set; } = true;
        public Dictionary<string, string> TypesFormatting { get; set; } = [];

        public override string ToString()
        {
            if (!SerializeAsRawString)
            {
                return string.Empty;
            }

            var stringBuilder = new StringBuilder();
            foreach (var type in TypesFormatting)
            {
                stringBuilder.AppendLine($"\"{type.Key}\" => (obj as {type.Key}?)?.ToString(\"{type.Value}\") ?? string.Empty,");
            }

            return stringBuilder.ToString();
        }
    }
}
