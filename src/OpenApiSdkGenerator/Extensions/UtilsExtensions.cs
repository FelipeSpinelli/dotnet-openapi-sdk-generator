using System;
using System.Buffers;

namespace OpenApiSdkGenerator.Extensions
{
    public static class UtilsExtensions
    {
        public static string ToPascalCase(this string value)
        {
            var buffer = ArrayPool<char>.Shared.Rent(value.Length * 2);
            var valueAsSpan = value.AsSpan();
            var bufferIndex = 0;
            var mustBeUpper = false;

            for (int i = 0; i < valueAsSpan.Length; i++)
            {
                if (i == 0)
                {
                    buffer[bufferIndex++] = char.ToUpper(valueAsSpan[i]);
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

            var valueAsPascalCase = string.Join(string.Empty, buffer).Substring(0, bufferIndex);
            ArrayPool<char>.Shared.Return(buffer, true);

            return valueAsPascalCase;
        }
    }
}
