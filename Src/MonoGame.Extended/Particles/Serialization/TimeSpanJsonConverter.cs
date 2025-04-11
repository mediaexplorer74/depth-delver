using MonoGame.Extended.Serialization.Json;
using Newtonsoft.Json;
using System;
//using System.Text.Json;
//using System.Text.Json.Serialization;

namespace MonoGame.Extended.Particles.Serialization;

public class TimeSpanJsonConverter : JsonConverter<TimeSpan>
{
    /// <inheritdoc />
    public /*override*/ bool CanConvert(Type typeToConvert) => typeToConvert == typeof(TimeSpan);

    /// <inheritdoc />
    public /*override*/ TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonToken.Bytes/*.Number*/)
        {
            double seconds = reader.GetDouble();
            return TimeSpan.FromSeconds(seconds);
        }

        return TimeSpan.Zero;
    }

    public override TimeSpan ReadJson(JsonReader reader, Type objectType, TimeSpan existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException">
    /// Throw if <paramref name="writer"/> is <see langword="null"/>.
    /// </exception>
    public /*override*/ void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
    {
        if (writer == null)
        {
            throw new ArgumentNullException(nameof(writer));
        }
        writer.WriteNumberValue(value.TotalSeconds);
    }

    public override void WriteJson(JsonWriter writer, TimeSpan value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}
