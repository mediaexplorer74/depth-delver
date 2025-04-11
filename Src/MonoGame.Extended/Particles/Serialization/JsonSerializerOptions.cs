using MonoGame.Extended.Serialization.Json;
using Newtonsoft.Json;

namespace MonoGame.Extended.Particles.Serialization
{
    public class JsonSerializerOptions : JsonSerializerSettings
    {
        internal JsonNamingPolicy PropertyNamingPolicy;
        internal object DefaultIgnoreCondition;

        public JsonSerializerOptions()
        {
            // Set default options for the JSON serializer
            Formatting = Formatting.Indented;
            NullValueHandling = NullValueHandling.Ignore;
            TypeNameHandling = TypeNameHandling.Auto;
            PreserveReferencesHandling = PreserveReferencesHandling.Objects;
        }

        public bool WriteIndented { get; internal set; }
    }
  
}