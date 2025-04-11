using MonoGame.Extended.Serialization.Json;
using Newtonsoft.Json;
using System;
using System.Reflection;
//using System.Text.Json;
//using System.Text.Json.Serialization;

namespace MonoGame.Extended.Particles.Serialization;

/// <summary>
/// Converts a <see cref="ParticleModifierExecutionStrategy"/> value to or from JSON.
/// </summary>
public class ModifierExecutionStrategyJsonConverter : JsonConverter<ParticleModifierExecutionStrategy>
{
    /// <inheritdoc />
    public /*override*/ bool CanConvert(Type typeToConvert) =>
        typeToConvert == typeof(ParticleModifierExecutionStrategy) ||
        typeToConvert.GetTypeInfo().BaseType == typeof(ParticleModifierExecutionStrategy);

    /// <inheritdoc />
    public /*override*/ ParticleModifierExecutionStrategy Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString(); // Fix: Use Utf8JsonReader's GetString method to read the string value
        return ParticleModifierExecutionStrategy.Parse(value);
    }

    public override ParticleModifierExecutionStrategy ReadJson(JsonReader reader, Type objectType, ParticleModifierExecutionStrategy existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException">
    /// Throw if <paramref name="writer"/> is <see langword="null"/>.
    /// </exception>
    public /*override*/ void Write(Utf8JsonWriter writer, ParticleModifierExecutionStrategy value, JsonSerializerOptions options)
    {
        if (writer == null) // Fix: Replace ArgumentNullException.ThrowIfNull with a null check
        {
            throw new ArgumentNullException(nameof(writer));
        }
        writer.WriteStringValue(value.ToString());
    }

    public override void WriteJson(JsonWriter writer, ParticleModifierExecutionStrategy value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}
