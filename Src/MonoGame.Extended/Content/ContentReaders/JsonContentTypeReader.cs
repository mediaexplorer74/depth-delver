using System.IO;
//using System.Text.Json;
using Microsoft.Xna.Framework.Content;
using Newtonsoft.Json;


namespace MonoGame.Extended.Content.ContentReaders
{
    public class JsonContentTypeReader<T> : ContentTypeReader<T>
    {
        protected override T Read(ContentReader reader, T existingInstance)
        {
            var json = reader.ReadString();
            using (var stringReader = new StringReader(json))
            using (var jsonReader = new JsonTextReader(stringReader))
            {
                var serializer = JsonSerializer.CreateDefault();
                return serializer.Deserialize<T>(jsonReader);
            }
        }
    }
}
