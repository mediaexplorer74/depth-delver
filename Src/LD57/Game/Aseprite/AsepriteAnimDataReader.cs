
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

   

    public class AsepriteAnimDataReader : JsonContentTypeReader<AsepriteAnimData>
    {
        protected override AsepriteAnimData Read(ContentReader input, AsepriteAnimData existingInstance)
        {
            AsepriteAnimData animData = new AsepriteAnimData
            {
                //frames = input.ReadObject<List<AsepriteAnimFrame>>(),
                //meta = input.ReadObject<AsepriteAnimMetadata>()
            };
            return animData;
        }
    }
}

