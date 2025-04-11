using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace LD57
{
    public class JsonContentTypeReader<T> : ContentTypeReader<T>
        where T : class
    {
        protected override T Read(ContentReader input, T existingInstance)
        {
            //var animData = new AsepriteAnimData
            //{
                //frames = input.ReadObject<List<AsepriteAnimFrame>>(),
                //meta = input.ReadObject<AsepriteAnimMetadata>()
            //};
            return default;
        }
    }
 
}