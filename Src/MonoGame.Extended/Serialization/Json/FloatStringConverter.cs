using MonoGame.Extended.Particles.Serialization;
using Newtonsoft.Json;
using System;
//using System.Text.Json;
//using System.Text.Json.Serialization;

namespace MonoGame.Extended.Serialization.Json;

public class FloatStringConverter : JsonConverter<float>
{
    /// <inheritdoc />
    public /*override*/ bool CanConvert(Type typeToConvert) => typeToConvert == typeof(float) || typeToConvert == typeof(string);

    /// <inheritdoc />
    public /*override*/ float Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == /*JsonTokenType.String*/JsonToken.String) // Fixed comparison to use JsonTokenType.String
        {
            if (float.TryParse(reader.GetString(), out float value))
                return value;
        }
        else if (reader.TokenType == /*JsonTokenType.Number*/JsonToken.Bytes)
        {
            return reader.GetSingle();
        }

        throw new JsonException($"Unable to convert value of type {reader.TokenType} to {typeof(float)}");
    }

    public override float ReadJson(JsonReader reader, Type objectType, float existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public /*override*/ void Write(Utf8JsonWriter writer, float value, JsonSerializerOptions options)
    {
        if (writer == null) // Replaced ArgumentNullException.ThrowIfNull with a null check
            throw new ArgumentNullException(nameof(writer));
        writer.WriteNumberValue(value);
    }

    public override void WriteJson(JsonWriter writer, float value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}
