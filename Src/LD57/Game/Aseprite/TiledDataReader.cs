
// Type: LD57.AsepriteAnimDataReader
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Modded by [M]edia[E]xplorer

//using MonoGame.Extended.Content.ContentReaders;

#nullable disable
using LD57;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace LD57
{

   
    public class TiledDataReader : JsonContentTypeReader<TiledMapObject>
    {
        protected override TiledMapObject Read(ContentReader input, TiledMapObject existingInstance)
        {
            TiledMapObject tiledData = new TiledMapObject
            {
                //frames = input.ReadObject<List<AsepriteAnimFrame>>(),
                //meta = input.ReadObject<AsepriteAnimMetadata>()
            };
            return tiledData;
        }
    }
}

