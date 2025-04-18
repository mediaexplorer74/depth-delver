using MonoGame.Extended.Particles.Serialization;
using Newtonsoft.Json;
using System;
//using System.Text.Json;
//using System.Text.Json.Serialization;

namespace MonoGame.Extended.Serialization.Json;

/// <summary>
/// Converts a <see cref="RectangleF"/> value to or from JSON.
/// </summary>
public class RectangleFJsonConverter : JsonConverter<RectangleF>
{
    /// <inheritdoc />
    public /*override*/ bool CanConvert(Type typeToConvert) => typeToConvert == typeof(RectangleF);

    /// <inheritdoc />
    public /*override*/ RectangleF Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var values = reader.ReadAsMultiDimensional<float>(options);
        return new RectangleF(values[0], values[1], values[2], values[3]);
    }

    public override RectangleF ReadJson(JsonReader reader, Type objectType, RectangleF existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException">
    /// Throw if <paramref name="writer"/> is <see langword="null"/>.
    /// </exception>
    public /*override*/ void Write(Utf8JsonWriter writer, RectangleF value, JsonSerializerOptions options)
    {
        if (writer == null)
        {
            throw new ArgumentNullException(nameof(writer));
        }
        writer.WriteStringValue($"{value.Left} {value.Top} {value.Width} {value.Height}");
    }

    public override void WriteJson(JsonWriter writer, RectangleF value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}
