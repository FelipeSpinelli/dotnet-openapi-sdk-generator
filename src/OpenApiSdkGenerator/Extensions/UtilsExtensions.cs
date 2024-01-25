using OpenApiSdkGenerator.Models;
using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OpenApiSdkGenerator.Extensions;

public static class UtilsExtensions
{
    public static string Sanitize(this string value)
    {
        var invalidChars = "@~`!#$%^&()=+';:\"\\/?>.<,]".ToCharArray();

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
            buffer[bufferIndex++] = '_';
        }

        var sanitizedValue = string.Join(string.Empty, buffer).Substring(0, bufferIndex);
        ArrayPool<char>.Shared.Return(buffer, true);

        return sanitizedValue;
    }

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

    public static IDictionary<TKey, TValue> Merge<TKey, TValue>(this IDictionary<TKey, TValue> source, IDictionary<TKey, TValue>? other)
    {
        other ??= new Dictionary<TKey, TValue>();

        foreach (var x in other)
        {
            source.Add(x.Key, x.Value);
        }

        return source;
    }
}