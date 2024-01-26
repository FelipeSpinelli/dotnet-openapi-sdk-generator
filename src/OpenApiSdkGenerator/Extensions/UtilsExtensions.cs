using System;
using System.Buffers;
using System.Linq;

namespace OpenApiSdkGenerator.Extensions
{
    public static class UtilsExtensions
    {
        public static string Sanitize(this string value)
        {
            var invalidChars = "@~`!#$%^&()=+';:\"\\/?>.<,]-".ToCharArray();

            var buffer = ArrayPool<char>.Shared.Rent(value.Length * 2);
            var valueAsSpan = value.AsSpan();
            var bufferIndex = 0;

            for (int i = 0; i < valueAsSpan.Length; i++)
            {
                if (!invalidChars.Contains(valueAsSpan[i]))
                {
                    buffer[bufferIndex++] = valueAsSpan[i];
                    continue;
                }

                buffer[bufferIndex++] = '_';
            }

            var sanitizedValue = string.Join(string.Empty, buffer).Substring(0, bufferIndex);
            ArrayPool<char>.Shared.Return(buffer, true);

            return sanitizedValue.ToCamelCase();
        }

        public static string ToPascalCase(this string value)
        {
            return InternalCaseConvertion(value, mustUpperFirstChar: true);
        }

        public static string ToCamelCase(this string value)
        {
            return InternalCaseConvertion(value, mustUpperFirstChar: false);
        }

        private static string InternalCaseConvertion(string value, bool mustUpperFirstChar)
        {
            var buffer = ArrayPool<char>.Shared.Rent(value.Length * 2);
            var valueAsSpan = value.AsSpan();
            var bufferIndex = 0;
            var mustBeUpper = false;

            for (int i = 0; i < valueAsSpan.Length; i++)
            {
                if (i == 0)
                {
                    buffer[bufferIndex++] = mustUpperFirstChar ? char.ToUpper(valueAsSpan[i]) : valueAsSpan[i];
                    continue;
                }

                if (valueAsSpan[i] == '_' || valueAsSpan[i] == ' ' || valueAsSpan[i] == '-')
                {
                    mustBeUpper = true;
                    continue;
                }

                buffer[bufferIndex++] = mustBeUpper ? char.ToUpper(valueAsSpan[i]) : valueAsSpan[i];
                mustBeUpper = false;
            }

            var result = string.Join(string.Empty, buffer).Substring(0, bufferIndex);
            ArrayPool<char>.Shared.Return(buffer, true);

            return result;
        }
    }
}