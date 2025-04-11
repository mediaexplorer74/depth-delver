using Newtonsoft.Json;
using System;

namespace MonoGame.Extended.Particles.Serialization
{
    public class Utf8JsonReader : JsonReader
    {
        public override bool Read()
        {
            // TODO: Implement a basic Read method
            // For now, return false to ensure all code paths return a value
            return false;
        }

        internal double GetDouble()
        {
            throw new NotImplementedException();
        }

        internal int GetInt32()
        {
            throw new NotImplementedException();
        }

        internal float GetSingle()
        {
            throw new NotImplementedException();
        }

        internal string GetString()
        {
            throw new NotImplementedException();
        }

        internal T[] ReadAsDelimitedString<T>()
        {
            throw new NotImplementedException();
        }

        internal T[] ReadAsJArray<T>(JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        internal T ReadAsMultiDimensional<T>(JsonReader reader)
        {
            // Fix: Use the correct overload of JsonSerializer.Deserialize that accepts a JsonReader
            return JsonSerializer.CreateDefault().Deserialize<T>(reader);
        }

        internal T[] ReadAsSingleValue<T>(JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}