﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MonoGame.Extended.Particles.Modifiers;
using MonoGame.Extended.Serialization.Json;
using Newtonsoft.Json;

namespace MonoGame.Extended.Particles.Serialization
{
    public class ModifierJsonConverter : BaseTypeJsonConverter<Modifier>
    {
        public ModifierJsonConverter()
            : base(GetSupportedTypes(), "Modifier")
        {
        }

        private static IEnumerable<TypeInfo> GetSupportedTypes()
        {
            return typeof(Modifier)
                .GetTypeInfo()
                .Assembly
                .DefinedTypes
                .Where(type => typeof(Modifier).GetTypeInfo().IsAssignableFrom(type) && !type.IsAbstract);
        }

        public override Modifier ReadJson(JsonReader reader, Type objectType, Modifier existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, Modifier value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}