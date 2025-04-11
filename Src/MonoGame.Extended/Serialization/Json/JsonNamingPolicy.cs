using System;

namespace MonoGame.Extended.Serialization.Json
{
    internal class JsonNamingPolicy
    {
        public static JsonNamingPolicy CamelCase { get; internal set; }

        internal string ConvertName(string name)
        {
            throw new NotImplementedException();
        }
    }
}