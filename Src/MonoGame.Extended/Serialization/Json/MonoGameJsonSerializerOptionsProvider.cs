//using System.Text.Json;
//using System.Text.Json.Serialization;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Particles.Serialization;

namespace MonoGame.Extended.Serialization.Json;

public static class MonoGameJsonSerializerOptionsProvider
{
    public static JsonSerializerOptions GetOptions(ContentManager contentManager, string contentPath)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = default,//JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        // RnD / TODO
        //options.Converters.Add(new RangeJsonConverter<int>());
        //options.Converters.Add(new RangeJsonConverter<float>());
        //options.Converters.Add(new RangeJsonConverter<HslColor>());
        //options.Converters.Add(new ThicknessJsonConverter());
        //options.Converters.Add(new RectangleFJsonConverter());
        //options.Converters.Add(new TextureAtlasJsonConverter(contentManager, contentPath));
        //options.Converters.Add(new Size2JsonConverter());

        return options;
    }
}
