
// Type: LD57.Physics.PhysicsComponent
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Assembly location: C:\Users\Admin\Desktop\RE\DepthDelver\LD57.dll

using Microsoft.Xna.Framework;
using System.Collections.Generic;

#nullable disable
namespace LD57.Physics
{
  public class PhysicsComponent : Component
  {
    private PhysicsManager m_manager;
    public Vector2 m_velocity = Vector2.Zero;
    public Vector2 m_acceleration = Vector2.Zero;
    public Vector2 m_frameVelocity = Vector2.Zero;
    public Vector2 m_lastDisplacement = Vector2.Zero;
    private AABB m_aabb;
    public bool m_debugDraw;
    private bool m_wall;
    public List<PhysicsComponent> m_carry;
    public PhysicsComponent.CollideDir m_lastCollideDir;
    public PhysicsComponent.CollideMask m_collideAs;
    public PhysicsComponent.CollideMask m_collideWith;
    public float m_terminalVel;

    public PhysicsComponent(Entity parent, PhysicsManager manager, bool wall = false)
      : base(parent)
    {
      this.m_manager = manager;
      this.m_manager.AddPhysics(this, wall);
      this.m_aabb = new AABB(Vector2.Zero, Vector2.Zero);
      this.m_wall = wall;
      this.m_carry = new List<PhysicsComponent>();
    }

    public AABB GetAABB(bool local = false)
    {
      if (local)
        return this.m_aabb;
      AABB aabb = this.m_aabb;
      aabb.m_center += this.GetParent().m_position;
      return aabb;
    }

    public void SetOffset(Vector2 offset) => this.m_aabb.m_center = offset;

    public void SetExtents(Vector2 extents) => this.m_aabb.m_extents = extents;

    public void Move(Vector2 move) => this.m_frameVelocity += move;

    public bool IsCollideGround() => this.IsCollideDir(PhysicsComponent.CollideDir.Down);

    public bool IsCollideDir(PhysicsComponent.CollideDir dir) => (this.m_lastCollideDir & dir) != 0;

    public bool IsCollideDir(Vector2 dir)
    {
      return (double) dir.X > 0.0 && this.IsCollideDir(PhysicsComponent.CollideDir.Right) || (double) dir.X < 0.0 && this.IsCollideDir(PhysicsComponent.CollideDir.Left) || (double) dir.Y > 0.0 && this.IsCollideDir(PhysicsComponent.CollideDir.Down) || (double) dir.Y < 0.0 && this.IsCollideDir(PhysicsComponent.CollideDir.Up);
    }

    public override void Update(GameTime gameTime)
    {
    }

    public override void Draw(GameTime gameTime)
    {
      if (!this.m_debugDraw)
        return;
      Color red = Color.Red with { A = 100 };
      SpriteManager.DrawAABB(this.GetAABB(), red, SpriteManager.s_camera, 1f);
    }

    public enum CollideDir
    {
      None = 0,
      Up = 1,
      Down = 2,
      Left = 4,
      Right = 8,
    }

    public enum CollideMask
    {
      None = 0,
      Solid = 1,
      SemiSolid = 2,
      Player = 4,
      Enemy = 8,
      Object = 16, // 0x00000010
      WorldAll = 19, // 0x00000013
    }
  }
}
