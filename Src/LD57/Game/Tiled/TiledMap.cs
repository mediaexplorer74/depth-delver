
// Type: LD57.Tiled.TiledMapComponent
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Modded by [M]edia[E]xplorer

using LD57.Camera;
using System;

namespace LD57.Tiled
{
    public class TiledMap
    {
        public TileLayer TileLayers;
        public int TileWidth;
        public int TileHeight;

        //RnD: pObjectLayer
        internal TiledMapObjectLayer[] ObjectLayers;
        internal sbyte Height;
        internal sbyte Width;
        internal TileSet Tilesets;

        internal int GetTilesetFirstGlobalIdentifier(TiledMapTileset tilesetByIdentifyer)
        {
            throw new NotImplementedException();
        }
    }
}