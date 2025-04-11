using System;
//using System.Text.Json;
//using System.Text.Json.Serialization;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Particles.Serialization;
using Newtonsoft.Json;

namespace MonoGame.Extended.Serialization.Json;

/// <summary>
/// Converts a <see cref="Vector2"/> value to or from JSON.
/// </summary>
public class Vector2JsonConverter : JsonConverter<Vector2>
{
    /// <inheritdoc />
    public /*override*/ bool CanConvert(Type objectType) => objectType == typeof(Vector2);

    /// <inheritdoc />
    /// <exception cref="JsonException">
    /// Thrown if the JSON property does not contain a properly formatted <see cref="Vector2"/> value
    /// </exception>
    public override Vector2 ReadJson(JsonReader reader, Type objectType, Vector2 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType != JsonToken.StartArray)
        {
            throw new JsonException("Expected StartArray token");
        }

        reader.Read();
        if (reader.TokenType != JsonToken.Float && reader.TokenType != JsonToken.Integer)
        {
            throw new JsonException("Expected Float or Integer token for X value");
        }
        var x = Convert.ToSingle(reader.Value);

        reader.Read();
        if (reader.TokenType != JsonToken.Float && reader.TokenType != JsonToken.Integer)
        {
            throw new JsonException("Expected Float or Integer token for Y value");
        }
        var y = Convert.ToSingle(reader.Value);

        reader.Read();
        if (reader.TokenType != JsonToken.EndArray)
        {
            throw new JsonException("Expected EndArray token");
        }

        return new Vector2(x, y);
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="writer"/> is <see langword="null"/>.
    /// </exception>
    public override void WriteJson(JsonWriter writer, Vector2 value, JsonSerializer serializer)
    {
        if (writer == null)
        {
            throw new ArgumentNullException(nameof(writer));
        }

        writer.WriteStartArray();
        writer.WriteValue(value.X);
        writer.WriteValue(value.Y);
        writer.WriteEndArray();
    }
}
