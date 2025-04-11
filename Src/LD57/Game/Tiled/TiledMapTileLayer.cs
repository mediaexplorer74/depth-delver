
// Type: LD57.Tiled.TiledMapComponent
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Modded by [M]edia[E]xplorer

using LD57.Spawn;
using System;
using System.Collections.Generic;

namespace LD57.Tiled
{
    public class TiledMapTileLayer
    {
        internal Dictionary<string, string> Properties;
        internal Tile Tiles;

        internal TiledMapTile GetTile(ushort x1, ushort y1)
        {
            throw new NotImplementedException();
        }
    }
}