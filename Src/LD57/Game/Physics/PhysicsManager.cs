
// Type: LD57.Physics.PhysicsManager
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Assembly location: C:\Users\Admin\Desktop\RE\DepthDelver\LD57.dll

using LD57.Tiled;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace LD57.Physics
{
  public class PhysicsManager
  {
    private List<PhysicsComponent> m_physicsComponents;
    private List<PhysicsComponent> m_walls;
    private TiledMapComponent m_map;
    private const float kSkinWidth = 0.001f;
    private List<AABB> m_col = new List<AABB>();
    private bool m_debugDraw;

    public PhysicsManager(TiledMapComponent map)
    {
      this.m_physicsComponents = new List<PhysicsComponent>();
      this.m_walls = new List<PhysicsComponent>();
      this.m_map = map;
    }

    public void AddPhysics(PhysicsComponent physics, bool wall = false)
    {
      if (wall)
        this.m_walls.Add(physics);
      else
        this.m_physicsComponents.Add(physics);
    }

    public void Update(GameTime gameTime)
    {
      float totalSeconds = (float) gameTime.ElapsedGameTime.TotalSeconds;
      if (this.m_debugDraw)
        this.m_col.Clear();
      this.UpdatePhysicsList(this.m_physicsComponents, totalSeconds);
      this.UpdatePhysicsList(this.m_walls, totalSeconds, true);
      for (int index = 0; index < this.m_walls.Count<PhysicsComponent>(); ++index)
        this.UpdateCarryMove(this.m_walls[index]);
    }

    private void UpdatePhysicsList(List<PhysicsComponent> list, float time, bool isWall = false)
    {
      for (int index = 0; index < list.Count<PhysicsComponent>(); ++index)
      {
        if (list[index].GetParent().IsDestroyed())
        {
          list.RemoveAt(index);
          --index;
        }
        else
          this.UpdatePhysics(list[index], time, isWall);
      }
    }

    private void UpdatePhysics(PhysicsComponent physics, float time, bool isWall = false)
    {
      physics.m_lastCollideDir = PhysicsComponent.CollideDir.None;
      physics.m_velocity += physics.m_acceleration * time;
      if ((double) physics.m_terminalVel != 0.0)
        physics.m_velocity.Y = Math.Min(physics.m_velocity.Y, physics.m_terminalVel);
      Vector2 vector2 = (physics.m_velocity + physics.m_frameVelocity) * time;
      physics.m_frameVelocity = Vector2.Zero;
      physics.m_lastDisplacement = Vector2.Zero;
      this.MoveAxis(physics, Vector2.UnitX, vector2.X, isWall);
      this.MoveAxis(physics, Vector2.UnitY, vector2.Y, isWall);
    }

    private void MoveAxis(PhysicsComponent physics, Vector2 axis, float move, bool isWall = false)
    {
      if ((double) move == 0.0)
        return;
      PhysicsComponent.CollideMask collideWith = physics.m_collideWith;
      if (collideWith != PhysicsComponent.CollideMask.None)
      {
        AABB aabb1 = physics.GetAABB();
        AABB m_aabb = aabb1;
        m_aabb.m_center += move * axis;
        List<AABB> hit = new List<AABB>();
        Vector2 vector1 = Vector2.Zero;
        float num1 = float.MaxValue;
        if ((double) axis.Y <= 0.0 || (double) move <= 0.0)
          collideWith &= ~PhysicsComponent.CollideMask.SemiSolid;
        if (this.m_map.CheckCollision(m_aabb, collideWith, hit, new AABB?(aabb1)))
        {
          for (int index = 0; index < hit.Count; ++index)
          {
            float time;
            Vector2 normal;
            hit[index].SweepAABB(aabb1, axis * move, out time, out normal);
            if ((double) time < (double) num1)
            {
              num1 = time;
              vector1 = normal;
            }
            if (this.m_debugDraw)
              this.m_col.Add(hit[index]);
          }
          move = (float) ((double) move * (double) num1 + 1.0 / 1000.0 * (double) Math.Sign(vector1.Dot(axis)));
        }
        float num2 = float.MaxValue;
        m_aabb = aabb1;
        m_aabb.m_center += move * axis;
        PhysicsComponent physicsComponent = (PhysicsComponent) null;
        for (int index = 0; index < this.m_walls.Count<PhysicsComponent>(); ++index)
        {
          PhysicsComponent wall = this.m_walls[index];
          if (physics != wall && !wall.GetParent().IsDestroyed() && (wall.m_collideAs & collideWith) != PhysicsComponent.CollideMask.None)
          {
            AABB aabb2 = wall.GetAABB();
            float time;
            Vector2 normal;
            if (!aabb2.Intersects(aabb1) && aabb2.SweepAABB(aabb1, axis * move, out time, out normal))
            {
              if ((double) time < (double) num2)
              {
                num2 = time;
                vector1 = normal;
                physicsComponent = wall;
              }
              if (this.m_debugDraw)
                this.m_col.Add(aabb2);
            }
          }
        }
        if (physicsComponent != null)
        {
          if ((double) vector1.Y < 0.0)
            physicsComponent.m_carry.Add(physics);
          move = (float) ((double) move * (double) num2 + 1.0 / 1000.0
                        * (double) Math.Sign(vector1.Dot(axis)));
        }
        if ((double) vector1.Dot(axis) != 0.0)
        {
          bool flag = (double) vector1.Dot(axis) > 0.0;
          float num3 = !flag
                        ? Math.Min(physics.m_velocity.Dot(axis), 0.0f) 
                        : Math.Max(physics.m_velocity.Dot(axis), 0.0f);
          if ((double) axis.X != 0.0)
          {
            physics.m_velocity.X = num3;
            physics.m_lastCollideDir |= flag 
                            ? PhysicsComponent.CollideDir.Left 
                            : PhysicsComponent.CollideDir.Right;
          }
          else
          {
            physics.m_velocity.Y = num3;
            physics.m_lastCollideDir |= flag 
                            ? PhysicsComponent.CollideDir.Up 
                            : PhysicsComponent.CollideDir.Down;
          }
        }
      }
      if (isWall && physics.m_collideAs != PhysicsComponent.CollideMask.None)
      {
        AABB aabb3 = physics.GetAABB();
        aabb3.m_center += move * axis;
        for (int index = 0; index < this.m_physicsComponents.Count<PhysicsComponent>(); ++index)
        {
          PhysicsComponent physicsComponent = this.m_physicsComponents[index];
          if (!physicsComponent.GetParent().IsDestroyed() && (physicsComponent.m_collideWith & physics.m_collideAs) != PhysicsComponent.CollideMask.None && !physics.m_carry.Contains(physicsComponent))
          {
            AABB aabb4 = physicsComponent.GetAABB();
            if (!aabb4.Intersects(aabb3) && aabb4.SweepAABB(aabb3, move * axis, out float _, out Vector2 _))
              this.MoveAxis(physicsComponent, axis, move);
          }
        }
      }
      physics.GetParent().m_position += move * axis;
      physics.m_lastDisplacement += move * axis;
    }

    private void UpdateCarryMove(PhysicsComponent wall)
    {
      if (wall.m_lastDisplacement != Vector2.Zero)
      {
        for (int index = 0; index < wall.m_carry.Count; ++index)
        {
          PhysicsComponent physics = wall.m_carry[index];
          this.MoveAxis(physics, Vector2.UnitX, wall.m_lastDisplacement.X);
          this.MoveAxis(physics, Vector2.UnitY, wall.m_lastDisplacement.Y);
        }
      }
      wall.m_carry.Clear();
    }

    public bool Overlap(AABB aabb, PhysicsComponent.CollideMask mask, bool semiSolide = false)
    {
      if (this.m_map.CheckCollision(aabb, mask))
        return true;
      for (int index = 0; index < this.m_walls.Count<PhysicsComponent>(); ++index)
      {
        PhysicsComponent wall = this.m_walls[index];
        if (!wall.GetParent().IsDestroyed() && (wall.m_collideAs & mask) != PhysicsComponent.CollideMask.None)
        {
          AABB aabb1 = wall.GetAABB();
          if (aabb.Intersects(aabb1))
            return true;
        }
      }
      return false;
    }

    public bool SweepAABB(
      AABB aabb1,
      AABB aabb2,
      Vector2 offset,
      PhysicsComponent.CollideMask mask,
      out float time)
    {
      PhysicsComponent hit = (PhysicsComponent) null;
      return this.SweepAABB(aabb1, aabb2, offset, mask, out time, out hit);
    }

    public bool SweepAABB(
      AABB aabb1,
      AABB aabb2,
      Vector2 offset,
      PhysicsComponent.CollideMask mask,
      out float time,
      out PhysicsComponent hit)
    {
      hit = (PhysicsComponent) null;
      List<AABB> hit1 = new List<AABB>();
      bool flag = false;
      float num = float.MaxValue;
      if (this.m_map.CheckCollision(aabb2, mask, hit1))
      {
        for (int index = 0; index < hit1.Count; ++index)
        {
          float time1;
          if (hit1[index].SweepAABB(aabb1, offset, out time1, out Vector2 _))
          {
            flag = true;
            if ((double) time1 < (double) num)
              num = time1;
          }
          if (this.m_debugDraw)
            this.m_col.Add(hit1[index]);
        }
      }
      for (int index = 0; index < this.m_walls.Count<PhysicsComponent>(); ++index)
      {
        PhysicsComponent wall = this.m_walls[index];
        float time2;
        if (!wall.GetParent().IsDestroyed() && (wall.m_collideAs & mask) != PhysicsComponent.CollideMask.None && wall.GetAABB().SweepAABB(aabb1, offset, out time2, out Vector2 _))
        {
          flag = true;
          if ((double) time2 < (double) num)
          {
            hit = wall;
            num = time2;
          }
        }
      }
      time = num;
      return flag;
    }

    public void Draw()
    {
      if (this.m_col == null)
        return;
      Color blue = Color.Blue with { A = 100 };
      for (int index = 0; index < this.m_col.Count; ++index)
        SpriteManager.DrawAABB(this.m_col[index], blue, SpriteManager.s_camera, 1f);
    }
  }
}
