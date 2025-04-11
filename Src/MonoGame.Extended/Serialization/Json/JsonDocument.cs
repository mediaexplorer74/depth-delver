using MonoGame.Extended.Particles.Serialization;
using System;
using Windows.UI.Xaml;

namespace MonoGame.Extended.Serialization.Json
{
    internal class JsonDocument : IDisposable
    {
        internal XamlRoot RootElement;

        internal static JsonDocument ParseValue(ref Utf8JsonReader reader)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}