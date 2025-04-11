
// Type: LD57.Breakables.GibComponent
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Assembly location: C:\Users\Admin\Desktop\RE\DepthDelver\LD57.dll

using LD57.Objects;
using LD57.Physics;
using LD57.Spawn;
using Microsoft.Xna.Framework;

#nullable disable
namespace LD57.Breakables
{
  public class GibComponent : GameObjectComponent
  {
    private Color m_color;

    public GibComponent(Entity parent, LevelState level, Color color)
      : base(parent, level, (SpawnPoint) null)
    {
      this.m_color = color;
      this.m_physics = new PhysicsComponent(parent, level.GetPhysicsManager());
      this.m_physics.m_acceleration.Y = 960f;
    }

    public override void InitState()
    {
    }

    public override void UpdateState(GameTime gameTime)
    {
      AABB other = new AABB(this.GetPos(), 1f, 1f);
      if (this.GetCamera() != null)
      {
        if (this.GetCamera().GetViewBound().Intersects(other))
          return;
        this.GetParent().Destroy();
      }
      else
      {
        if (new AABB(new Vector2(0.0f, 0.0f), new Vector2(256f, 144f)).Intersects(other))
          return;
        this.GetParent().Destroy();
      }
    }

    public override void ExitState()
    {
    }

    public override void Draw(GameTime gameTime)
    {
      SpriteManager.DrawAABB(new AABB(this.GetPos(), 0.5f, 0.5f), this.m_color, this.GetCamera(), 0.2f);
    }
  }
}
