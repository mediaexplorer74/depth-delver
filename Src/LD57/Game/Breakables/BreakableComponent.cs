
// Type: LD57.Breakables.BreakableComponent
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Modded by [M]edia[E]xplorer

using LD57.Combat;
using LD57.Objects;
using LD57.Physics;
using LD57.Spawn;
using Microsoft.Xna.Framework;

#nullable disable
namespace LD57.Breakables
{
  public abstract class BreakableComponent : GameObjectComponent, CombatImplementor
  {
    private float m_dir;

    public BreakableComponent(Entity parent, LevelState level, SpawnPoint spawnPoint, string anim)
      : base(parent, level, spawnPoint)
    {
      this.m_anim = new AnimComponent(parent, anim, this.GetCamera());
      this.m_stateMachine.SetNextState(0);
      this.m_anim.Play("Idle");
      this.m_anim.m_depth = 0.3f;
      this.m_physics = new PhysicsComponent(parent, this.GetLevel().GetPhysicsManager(), true);
      this.m_physics.SetExtents(new Vector2(8f, 8f));
      this.m_physics.m_collideAs = PhysicsComponent.CollideMask.Object;
      this.m_combat = new CombatComponent(parent, this.GetLevel().GetCombatManager(), (CombatImplementor) this);
      this.m_combat.m_defenseBoxes.Add(this.m_physics.GetAABB(true));
      this.m_combat.m_defenseMask = CombatComponent.CombatMask.Object;
    }

    protected virtual bool CanBreak() => true;

    protected virtual void OnBreak()
    {
    }

    protected void Gib()
    {
      this.Gib(this.m_dir);
      AudioManager.PlaySFX("Kill");
    }

    public override void InitState()
    {
      switch ((BreakableComponent.State) this.m_stateMachine.GetState())
      {
        case BreakableComponent.State.Idle:
          this.m_anim.Play("Idle");
          break;
        case BreakableComponent.State.Break:
          this.m_anim.m_shake.Init(2.5f, -1f);
          this.OnBreak();
          break;
      }
    }

    public override void UpdateState(GameTime gameTime)
    {
      switch ((BreakableComponent.State) this.m_stateMachine.GetState())
      {
        case BreakableComponent.State.Break:
          if ((double) this.m_stateMachine.GetStateTime() < 0.01666666753590107)
            break;
          this.Gib();
          break;
      }
    }

    public override void ExitState()
    {
      if ((BreakableComponent.State) this.m_stateMachine.GetLastState() == BreakableComponent.State.Idle)
        ;
    }

    public virtual void OnTakeDamage(DamageDesc damage)
    {
      this.m_anim.m_shake.Init(1.5f, 0.0833333358f);
      AudioManager.PlaySFX("Latch");
    }

    public virtual void OnDeath(DamageDesc damage)
    {
      if (this.CanBreak())
      {
        this.m_combat.m_defenseEnabled = false;
        this.m_stateMachine.SetNextState(1);
        this.m_dir = damage.m_dir.X;
        AudioManager.PlaySFX("Latch");
      }
      else
      {
        this.m_combat.m_health = 1;
        this.OnTakeDamage(damage);
      }
    }

        public abstract bool ValidateHit(DamageDesc damage);
        public abstract void OnDealHit(DamageDesc damage);

        private enum State
    {
      Idle,
      Break,
    }
  }
}
