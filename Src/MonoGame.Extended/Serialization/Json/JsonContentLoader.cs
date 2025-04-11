//using System.Text.Json;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Content;
using Newtonsoft.Json;
using System.IO;


namespace MonoGame.Extended.Serialization.Json
{
    public class JsonContentLoader : IContentLoader
    {
        public T Load<T>(ContentManager contentManager, string path)
        {
            using var stream = contentManager.OpenStream(path);
            using var streamReader = new StreamReader(stream); // Convert Stream to StreamReader
            using var jsonReader = new JsonTextReader(streamReader); // Convert StreamReader to JsonReader
            var monoGameSerializerOptions = MonoGameJsonSerializerOptionsProvider.GetOptions(contentManager, path);
            JsonSerializer serializer = JsonSerializer.Create(monoGameSerializerOptions); // Use options if needed
            return serializer.Deserialize<T>(jsonReader); // Deserialize using JsonReader
        }
    }
}
