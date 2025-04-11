
// Type: LD57.Objects.HookComponent
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Assembly location: C:\Users\Admin\Desktop\RE\DepthDelver\LD57.dll

using LD57.Spawn;
using Microsoft.Xna.Framework;

#nullable disable
namespace LD57.Objects
{
  public class HookComponent : GameObjectComponent
  {
    public HookComponent(Entity parent, LevelState level, SpawnPoint spawnPoint)
      : base(parent, level, spawnPoint)
    {
      this.m_anim = new AnimComponent(parent, "Hook", this.GetCamera());
      this.m_anim.Play("Idle");
      this.m_anim.m_depth = 0.3f;
      this.m_stateMachine.SetNextState(0);
    }

    public AABB GetArea() => new AABB(this.GetPos(), 4f, 4f);

    public override void InitState() => this.m_stateMachine.GetState();

    public override void UpdateState(GameTime gameTime) => this.m_stateMachine.GetState();

    public override void ExitState() => this.m_stateMachine.GetLastState();

    private enum State
    {
      Idle,
    }
  }
}
