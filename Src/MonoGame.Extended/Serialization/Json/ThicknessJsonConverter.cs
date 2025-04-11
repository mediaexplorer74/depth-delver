using MonoGame.Extended.Particles.Serialization;
using Newtonsoft.Json;
using System;
//using System.Text.Json;
//using System.Text.Json.Serialization;

namespace MonoGame.Extended.Serialization.Json;

/// <summary>
/// Converts a <see cref="Thickness"/> value to or from JSON.
/// </summary>
public class ThicknessJsonConverter : JsonConverter<Thickness>
{
    /// <inheritdoc />
    public /*override*/ bool CanConvert(Type typeToConvert) => typeToConvert == typeof(Thickness);

    /// <inheritdoc />
    public /*override*/ Thickness Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var values = reader.ReadAsMultiDimensional<int>(options);
        return Thickness.FromValues(values);
    }

    public override Thickness ReadJson(JsonReader reader, Type objectType, Thickness existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException">
    /// Throw if <paramref name="writer"/> is <see langword="null"/>
    /// </exception>
    public /*override*/ void Write(Utf8JsonWriter writer, Thickness value, JsonSerializerOptions options)
    {
        if (writer == null)
        {
            throw new ArgumentNullException(nameof(writer));
        }
        writer.WriteStringValue($"{value.Left} {value.Top} {value.Right} {value.Bottom}");
    }

    public override void WriteJson(JsonWriter writer, Thickness value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}
