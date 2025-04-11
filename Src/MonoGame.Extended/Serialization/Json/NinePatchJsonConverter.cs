using System;
//using System.Text.Json;
//using System.Text.Json.Serialization;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.Particles.Serialization;
using Newtonsoft.Json;

namespace MonoGame.Extended.Serialization.Json;

/// <summary>
/// Converts a <see cref="NinePatch"/> value to or from JSON.
/// </summary>
public class NinePatchJsonConverter : JsonConverter<NinePatch>
{
    private readonly ITextureRegionService _textureRegionService;

    /// <summary>
    /// Initializes a new instance of the <see cref="NinePatchJsonConverter"/> class.
    /// </summary>
    /// <param name="textureRegionService">The texture region service used to retrieve texture regions.</param>
    public NinePatchJsonConverter(ITextureRegionService textureRegionService)
    {
        _textureRegionService = textureRegionService;
    }

    /// <inheritdoc />
    public /*override*/ bool CanConvert(Type typeToConvert) => typeToConvert == typeof(NinePatch);

    /// <inheritdoc />
    /// <exception cref="JsonException">
    /// Thrown if the JSON property does not contain a properly formatted <see cref="NinePatch"/> value
    /// </exception>
    public /*override*/ NinePatch Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonToken.StartObject)
        {
            throw new JsonException($"Expected {nameof(JsonToken.StartObject)} token");
        }

        string padding = string.Empty;
        string regionName = string.Empty;

        while (reader.Read())
        {
            if (reader.TokenType == JsonToken.EndObject)
            {
                break;
            }

            if (reader.TokenType == JsonToken.PropertyName)
            {
                var propertyName = reader.GetString();
                reader.Read();

                if (propertyName.Equals("Padding", StringComparison.Ordinal))
                {
                    padding = reader.GetString();
                }
                else if (propertyName.Equals("TextureRegion", StringComparison.Ordinal))
                {
                    regionName = reader.GetString();
                }
            }
        }

        if (string.IsNullOrEmpty(padding) || string.IsNullOrEmpty(regionName))
        {
            throw new JsonException($"Missing required properties \"Padding\" and \"TextureRegion\"");
        }

        var thickness = Thickness.Parse(padding);
        var region = _textureRegionService.GetTextureRegion(regionName);
        return region.CreateNinePatch(thickness);
    }

    public override NinePatch ReadJson(JsonReader reader, Type objectType, NinePatch existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException">
    /// Throw if <paramref name="writer"/> is <see langword="null"/>.
    /// </exception>
    public /*override*/ void Write(Utf8JsonWriter writer, NinePatch value, JsonSerializerOptions options)
    {
        if (writer == null)
        {
            throw new ArgumentNullException(nameof(writer));
        }

        if (value is null)
        {
            writer.WriteNull(); // Fix: Replaced WriteNullValue() with WriteNull()
            return;
        }

        writer.WriteStartObject();
        writer.WriteString("TextureRegion", value.Name);
        writer.WriteString("Padding", value.Padding.ToString());
        writer.WriteEndObject();
    }

    public override void WriteJson(JsonWriter writer, NinePatch value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}
