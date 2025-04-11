
// Type: LD57.Enemies.EnemyComponent
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Assembly location: C:\Users\Admin\Desktop\RE\DepthDelver\LD57.dll

using LD57.Combat;
using LD57.Objects;
using LD57.Physics;
using LD57.Spawn;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

#nullable disable
namespace LD57.Enemies
{
  public abstract class EnemyComponent : GameObjectComponent, CombatImplementor
  {
    private const float kHitStun = 0.25f;
    private float m_hitStun;

    public EnemyComponent(Entity parent, LevelState level, SpawnPoint spawnPoint)
      : base(parent, level, spawnPoint)
    {
    }

    public override void Update(GameTime gameTime)
    {
      if ((double) this.m_hitStun > 0.0)
      {
        this.m_hitStun -= gameTime.GetElapsedSeconds();
        if ((double) this.m_hitStun > 0.0)
          return;
        this.m_anim.m_shake.Clear();
        this.m_anim.m_color = Color.White;
      }
      else
        base.Update(gameTime);
    }

    public override void InitState()
    {
      if (this.m_stateMachine.GetState() != /*0*/(int)EnemyState.Death)
        return;
      this.m_combat.m_defenseEnabled = false;
      this.m_combat.m_attackEnabled = false;
      this.m_physics.m_velocity = new Vector2(-110f * this.GetParent().m_facing.X, -170f);
      this.m_anim.Play("Death");
      this.m_anim.m_shake.Init(2.5f, 0.166666672f);
      this.m_physics.m_collideWith = PhysicsComponent.CollideMask.None;
      this.m_physics.m_acceleration = new Vector2(0.0f, 640f);
    }

    public override void UpdateState(GameTime gameTime)
    {
      if (this.m_stateMachine.GetState() != /*0*/(int)EnemyState.Death)
        return;
      if (!this.IsOnScreen())
        this.GetParent().Destroy();
      else
        this.m_anim.m_visible = (double) this.m_stateMachine.GetStateTime() * 30.0 % 3.0 > 1.0;
    }

    public override void ExitState() => this.m_stateMachine.GetLastState();

    public virtual bool ValidateHit(DamageDesc damage) => true;

    public virtual void OnDealHit(DamageDesc damage)
    {
    }

    public virtual void OnTakeDamage(DamageDesc damage)
    {
      this.m_hitStun = 0.25f;
      this.m_anim.m_shake.Init(2.5f, -1f);
      this.m_anim.m_color = new Color(1f, 0.25f, 0.25f);
    }

    public virtual void OnDeath(DamageDesc damage)
    {
      this.m_combat.m_defenseEnabled = false;
      if ((double) damage.m_dir.X != 0.0)
        this.GetParent().m_facing.X = -damage.m_dir.X;
      AudioManager.PlaySFX("Kill");
      this.m_stateMachine.SetNextState(0);
    }

    protected enum EnemyState
    {
      Death,
      Extend,
    }
  }
}
