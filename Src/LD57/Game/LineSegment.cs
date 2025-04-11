
// Type: LD57.AABB
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;

namespace LD57
{
    public class LineSegment
    {
        public Vector2 Direction;

        public Origin Origin;
        private Vector2 center;
        private Vector2 offset;

        public LineSegment(Vector2 center, Vector2 offset)
        {
            this.center = center;
            this.offset = offset;
        }

        internal static LineSegment FromOrigin(Vector2 m_center, Vector2 offset)
        {
            throw new NotImplementedException();
        }
    }
}