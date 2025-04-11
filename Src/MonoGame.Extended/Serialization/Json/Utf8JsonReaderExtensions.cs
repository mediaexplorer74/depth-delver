using MonoGame.Extended.Particles.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
//using System.Text.Json;

namespace MonoGame.Extended.Serialization.Json;

/// <summary>
/// Provides extension methods for working with <see cref="Utf8JsonReader"/>.
/// </summary>
public static class Utf8JsonReaderExtensions
{
    private static readonly Dictionary<Type, Func<string, object>> s_stringParsers = new Dictionary<Type, Func<string, object>>
    {
        {typeof(int), s => int.Parse(s, CultureInfo.InvariantCulture.NumberFormat)},
        {typeof(float), s => float.Parse(s, CultureInfo.InvariantCulture.NumberFormat)},
        {typeof(HslColor), s => ColorExtensions.FromHex(s).ToHsl() }
    };

    /// <summary>
    /// Reads a multi-dimensional JSON array and converts it to an array of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of the array elements.</typeparam>
    /// <param name="reader">The <see cref="Utf8JsonReader"/> to read from.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    /// <returns>An array of the specified type.</returns>
    /// <exception cref="NotSupportedException">Thrown when the token type is not supported.</exception>
    public static T[] ReadAsMultiDimensional<T>(this Utf8JsonReader reader, JsonSerializerOptions options) // Removed 'ref' keyword
    {
        var tokenType = reader.TokenType;

        switch (tokenType)
        {
            case JsonToken.StartArray:
                return reader.ReadAsJArray<T>(options);

            case JsonToken.String:
                return reader.ReadAsDelimitedString<T>();

            case JsonToken.Bytes:
                return reader.ReadAsSingleValue<T>(options);

            default:
                throw new NotSupportedException($"{tokenType} is not currently supported in the multi-dimensional parser");
        }
    }

    private static T[] ReadAsSingleValue<T>(this Utf8JsonReader reader, JsonSerializerOptions options) // Removed 'ref' keyword
    {
        var token = JsonDocument.ParseValue(ref reader).RootElement;
        T value = default;//JsonSerializer.Deserialize<T>(token.GetRawText(), options);
        return new T[] { value };
    }

    private static T[] ReadAsJArray<T>(this Utf8JsonReader reader, JsonSerializerOptions options) // Removed 'ref' keyword
    {
        var items = new List<T>();
        while (reader.Read() && reader.TokenType != JsonToken.EndArray)
        {
            if (reader.TokenType == JsonToken.EndArray)
            {
                break;
            }

            //RnD
            var jsonDocument = JsonDocument.ParseValue(ref reader);
            T deserializedItem = default;//JsonSerializer.Deserialize<T>(jsonDocument.RootElement.GetRawText(), options);
            items.Add(deserializedItem);
        }

        return items.ToArray();
    }

    private static T[] ReadAsDelimitedString<T>(this Utf8JsonReader reader) // Removed 'ref' keyword
    {
        var value = reader.GetString();
        if (string.IsNullOrEmpty(value))
        {
            return Array.Empty<T>();
        }

        Span<string> values = value.Split(' ');
        var result = new T[values.Length];
        var parser = s_stringParsers[typeof(T)];

        for (int i = 0; i < values.Length; i++)
        {
            result[i] = (T)parser(values[i]);
        }

        return result;
    }
}
