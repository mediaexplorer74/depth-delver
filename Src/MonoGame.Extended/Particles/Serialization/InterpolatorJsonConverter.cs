using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MonoGame.Extended.Particles.Modifiers.Interpolators;
using MonoGame.Extended.Serialization.Json;
using Newtonsoft.Json;

namespace MonoGame.Extended.Particles.Serialization
{
    public class InterpolatorJsonConverter : BaseTypeJsonConverter<Interpolator>
    {
        public InterpolatorJsonConverter() 
            : base(GetSupportedTypes(), "Interpolator")
        {
        }

        private static IEnumerable<TypeInfo> GetSupportedTypes()
        {
            return typeof(Interpolator)
                .GetTypeInfo()
                .Assembly
                .DefinedTypes
                .Where(type => typeof(Interpolator).GetTypeInfo().IsAssignableFrom(type) && !type.IsAbstract);
        }

        public override Interpolator ReadJson(JsonReader reader, Type objectType, Interpolator existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, Interpolator value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}