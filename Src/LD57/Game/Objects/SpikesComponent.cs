
// Type: LD57.Objects.SpikesComponent
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Assembly location: C:\Users\Admin\Desktop\RE\DepthDelver\LD57.dll

using LD57.Combat;
using LD57.Spawn;
using Microsoft.Xna.Framework;

#nullable disable
namespace LD57.Objects
{
  public class SpikesComponent : GameObjectComponent
  {
    public SpikesComponent(
      Entity parent,
      LevelState level,
      SpawnPoint spawnPoint,
      Vector2? offset = null)
      : base(parent, level, spawnPoint)
    {
      offset = new Vector2?(offset ?? Vector2.Zero);
      this.m_anim = new AnimComponent(parent, "Spikes", this.GetCamera());
      this.m_anim.Play("Idle");
      this.m_anim.m_depth = 0.3f;
      this.m_anim.m_localPosition = offset.Value;
      this.m_combat = new CombatComponent(this.GetParent(), level.GetCombatManager());
      this.m_combat.m_attackBoxes.Add(new AABB(new Vector2(0.0f, 4f) + offset.Value, 7f, 4f));
      this.m_combat.m_attackMask = CombatComponent.CombatMask.Player;
      this.m_stateMachine.SetNextState(0);
    }

    public override void InitState() => this.m_stateMachine.GetState();

    public override void UpdateState(GameTime gameTime) => this.m_stateMachine.GetState();

    public override void ExitState() => this.m_stateMachine.GetLastState();

    private enum State
    {
      Idle,
    }
  }
}
