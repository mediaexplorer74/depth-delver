
// Type: LD57.Objects.ArrowComponent
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Modded by [M]edia[E]xplorer

using LD57.Combat;
using LD57.Physics;
using LD57.Spawn;
using Microsoft.Xna.Framework;

#nullable disable
namespace LD57.Objects
{
  public class ArrowComponent : GameObjectComponent
  {
    private const float kSpeed = 150f;

    public ArrowComponent(Entity parent, LevelState level)
      : base(parent, level, (SpawnPoint) null)
    {
      this.m_anim = new AnimComponent(parent, "Arrow", this.GetCamera());
      this.m_anim.Play("Idle");
      this.m_anim.m_depth = 0.45f;
      this.m_physics = new PhysicsComponent(parent, level.GetPhysicsManager());
      this.m_physics.SetExtents(new Vector2(5f, 3f));
      this.m_physics.m_collideAs = PhysicsComponent.CollideMask.Object;
      this.m_physics.m_collideWith = PhysicsComponent.CollideMask.WorldAll;
      this.m_combat = new CombatComponent(this.GetParent(), level.GetCombatManager());
      this.m_combat.m_attackBoxes.Add(new AABB(new Vector2(0.0f, 0.0f), 5f, 3f));
      this.m_combat.m_attackMask = CombatComponent.CombatMask.Player;
      this.m_stateMachine.SetNextState(0);
    }

    public override void InitState()
    {
      if (this.m_stateMachine.GetState() != 0)
        return;
      this.m_physics.m_velocity.X = this.GetParent().m_facing.X * 150f;
    }

    public override void UpdateState(GameTime gameTime)
    {
      if (this.m_stateMachine.GetState() != 0)
        return;
      if (!this.m_anim.GetBound().Intersects(this.GetLevel().GetRoomManager().GetCurRoom().GetArea()))
        this.GetParent().Destroy();
      if (!this.m_physics.IsCollideDir(this.GetParent().GetFacingX()))
        return;
      this.Gib(this.GetParent().m_facing.X, 0.35f);
    }

    public override void ExitState() => this.m_stateMachine.GetLastState();

    private enum State
    {
      Idle,
    }
  }
}
