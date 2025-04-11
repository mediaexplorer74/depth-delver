
// Type: LD57.Camera.RoomManager
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System.Collections;
//using System.Drawing;
using Windows.Foundation;

namespace LD57.Camera
{
    internal class TiledMapObject 
    {
        public string Name;
        public int Id;
        public int X;
        public int Y;
        public int Width;
        public int Height;
        public string Type;
        public string Properties;
        internal Vector2 Position;
        internal Size Size;

        public TiledMapObject(string name, int id, int x, int y, int width, int height, string type,
            string properties)
        {
            Name = name;
            Id = id;
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Type = type;
            Properties = properties;
        }
      
    }
}