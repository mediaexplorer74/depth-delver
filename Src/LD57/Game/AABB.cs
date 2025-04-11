
// Type: LD57.AABB
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Particles;
using System;

#nullable disable
namespace LD57
{
  public struct AABB
  {
    public Vector2 m_center;
    public Vector2 m_extents;

    public AABB(Vector2 center, float xExtent, float yExtent)
    {
      this.m_center = new Vector2(center.X, center.Y);
      this.m_extents = new Vector2(xExtent, yExtent);
    }

    public AABB(Vector2 min, Vector2 max)
    {
      this.m_center = (min + max) / 2f;
      this.m_extents = (min - max) / 2f;
      this.m_extents.X = Math.Abs(this.m_extents.X);
      this.m_extents.Y = Math.Abs(this.m_extents.Y);
    }

    public Vector2 GetMin() => this.m_center - this.m_extents;

    public Vector2 GetMax() => this.m_center + this.m_extents;

    public bool IsXAligned(Vector2 point)
    {
      return (double) Math.Abs(this.m_center.X - point.X) < (double) this.m_extents.X;
    }

    public bool IsYAligned(Vector2 point)
    {
      return (double) Math.Abs(this.m_center.Y - point.Y) < (double) this.m_extents.Y;
    }

    public bool Contains(Vector2 point) => this.IsXAligned(point) && this.IsYAligned(point);

    public Vector2 GetClosestPoint(Vector2 point)
    {
      Vector2 min = this.GetMin();
      Vector2 max = this.GetMax();
      point.X = MathHelper.Clamp(point.X, min.X, max.X);
      point.Y = MathHelper.Clamp(point.Y, min.Y, max.Y);
      return point;
    }

    public bool Intersects(LineSegment seg, Vector2? padding = null)
    {
      float time = 0.0f;
      Vector2 normal = Vector2.Zero;
      return this.Intersects(seg, out time, out normal, padding);
    }

    public bool Intersects(LineSegment seg, out float time, out Vector2 normal, Vector2? padding = null)
    {
      time = 0.0f;
      normal = Vector2.Zero;
      if (seg.Direction == Vector2.Zero)
        return this.Contains(seg.Origin);
      Vector2 vector1 = this.m_extents + (padding ?? Vector2.Zero);
      if ((double) seg.Direction.X == 0.0 || (double) seg.Direction.Y == 0.0)
      {
        Vector2 vector2_1 = new Vector2((float) Math.Abs(Math.Sign(seg.Direction.Y)), (float) Math.Abs(Math.Sign(seg.Direction.X)));
        if ((double) Math.Abs(this.m_center.Dot(vector2_1) - seg.Origin.Dot(vector2_1)) >= (double) vector1.Dot(vector2_1))
          return false;
        Vector2 vector2_2 = new Vector2(vector2_1.Y, vector2_1.X);
        normal = new Vector2((float) -Math.Sign(seg.Direction.X), (float) -Math.Sign(seg.Direction.Y));
        float num1 = 1f / seg.Direction.Dot(Vector2.One);
        float num2 = (float) Math.Sign(num1);
        float num3 = (this.m_center.Dot(vector2_2) - num2 * vector1.Dot(vector2_2) - seg.Origin.Dot(vector2_2)) * num1;
        double num4 = ((double) this.m_center.Dot(vector2_2) + (double) num2 * (double) vector1.Dot(vector2_2) - (double) seg.Origin.Dot(vector2_2)) * (double) num1;
        time = num3;
        return num4 >= 0.0 && (double) num3 < 1.0;
      }
      Vector2 vector2_3 = Vector2.One / seg.Direction;
      Vector2 vector2_4 = new Vector2((float) Math.Sign(vector2_3.X), (float) Math.Sign(vector2_3.Y));
      Vector2 vector2_5 = (this.m_center - vector2_4 * vector1 - seg.Origin) * vector2_3;
      Vector2 vector2_6 = (this.m_center + vector2_4 * vector1 - seg.Origin) * vector2_3;
      if ((double) vector2_5.X > (double) vector2_6.X || (double) vector2_5.Y > (double) vector2_6.Y)
        return false;
      if ((double) vector2_5.X > (double) vector2_5.Y)
      {
        time = vector2_5.X;
        normal = new Vector2(-vector2_4.X, 0.0f);
      }
      else
      {
        time = vector2_5.Y;
        normal = new Vector2(0.0f, -vector2_4.Y);
      }
      return (double) Math.Min(vector2_6.X, vector2_6.Y) >= 0.0 && (double) time < 1.0;
    }

    public bool IsXAligned(AABB other)
    {
      return (double) Math.Abs(this.m_center.X - other.m_center.X) < (double) this.m_extents.X + (double) other.m_extents.X;
    }

    public bool IsYAligned(AABB other)
    {
      return (double) Math.Abs(this.m_center.Y - other.m_center.Y) < (double) this.m_extents.Y + (double) other.m_extents.Y;
    }

    public bool Intersects(AABB other) => this.IsXAligned(other) && this.IsYAligned(other);

    public bool SweepAABB(AABB other, Vector2 offset)
    {
      return this.Intersects(new LineSegment(other.m_center, offset), new Vector2?(other.m_extents));
    }

    public bool SweepAABB(AABB other, Vector2 offset, out float time, out Vector2 normal)
    {
      time = 0.0f;
      return this.Intersects(LineSegment.FromOrigin(other.m_center, offset), out time, out normal, new Vector2?(other.m_extents));
    }
  }
}
