//using System.Text.Json;
//using System.Text.Json.Serialization;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Serialization.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MonoGame.Extended.Particles.Serialization;

public static class ParticleJsonSerializerOptionsProvider
{
    public static JsonSerializerSettings GetOptions(ITextureRegionService textureRegionService)
    {
        var options = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        options.Converters.Add(new Vector2JsonConverter());
        options.Converters.Add(new Size2JsonConverter());
        options.Converters.Add(new ColorJsonConverter());
        options.Converters.Add(new TextureRegion2DJsonConverter(textureRegionService));
        options.Converters.Add(new ProfileJsonConverter());
        options.Converters.Add(new ModifierJsonConverter());
        options.Converters.Add(new InterpolatorJsonConverter());
        options.Converters.Add(new TimeSpanJsonConverter());
        options.Converters.Add(new RangeJsonConverter<int>());
        options.Converters.Add(new RangeJsonConverter<float>());
        options.Converters.Add(new RangeJsonConverter<HslColor>());
        options.Converters.Add(new HslColorJsonConverter());
        options.Converters.Add(new ModifierExecutionStrategyJsonConverter());

        return options;
    }
}
