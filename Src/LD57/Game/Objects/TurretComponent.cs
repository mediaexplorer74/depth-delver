
// Type: LD57.Objects.TurretComponent
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Modded by [M]edia[E]xplorer

using LD57.Physics;
using LD57.Spawn;
using Microsoft.Xna.Framework;

#nullable disable
namespace LD57.Objects
{
  public class TurretComponent : GameObjectComponent
  {
    private const float kShootRate = 1f;
    private const float kAnticTime = 0.166666672f;

    public TurretComponent(Entity parent, LevelState level, SpawnPoint spawnPoint)
      : base(parent, level, spawnPoint)
    {
      this.m_anim = new AnimComponent(parent, "Turret", this.GetCamera());
      this.m_anim.Play("Idle");
      this.m_anim.m_depth = 0.15f;
      this.m_physics = new PhysicsComponent(parent, level.GetPhysicsManager(), true);
      this.m_physics.SetExtents(new Vector2(8f, 8f));
      this.m_physics.m_collideAs = PhysicsComponent.CollideMask.Solid;
      this.m_stateMachine.SetNextState(0);
    }

    public AABB GetArea() => new AABB(this.GetPos(), 4f, 4f);

    public override void InitState()
    {
      switch (this.m_stateMachine.GetState())
      {
        case 0:
          this.m_anim.Play("Idle");
          break;
        case 1:
          this.m_anim.Play("Ready");
          break;
        case 2:
          this.m_anim.Play("Shoot", 1);
          Entity entity = new Entity(this.GetLevel().GetRootEntity());
          entity.m_position = this.GetPos() + new Vector2(12f * this.GetParent().m_facing.X, 1f);
          entity.m_facing = this.GetParent().m_facing;
          ArrowComponent arrowComponent = new ArrowComponent(entity, this.GetLevel());
          this.GetLevel().AddEntity(entity);
          if (!this.IsOnScreen())
            break;
          AudioManager.PlaySFX("Shoot");
          break;
      }
    }

    public override void UpdateState(GameTime gameTime)
    {
      switch (this.m_stateMachine.GetState())
      {
        case 0:
          if ((double) this.m_stateMachine.GetStateTime() < 1.0)
            break;
          this.m_stateMachine.SetNextState(1);
          break;
        case 1:
          if ((double) this.m_stateMachine.GetStateTime() < 0.1666666716337204)
            break;
          this.m_stateMachine.SetNextState(2);
          break;
        case 2:
          if (!this.m_anim.IsDone())
            break;
          this.m_stateMachine.SetNextState(0);
          break;
      }
    }

    public override void ExitState()
    {
      switch (this.m_stateMachine.GetLastState())
      {
      }
    }

    private enum State
    {
      Idle,
      Ready,
      Shoot,
    }
  }
}
