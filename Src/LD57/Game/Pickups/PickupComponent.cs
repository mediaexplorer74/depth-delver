
// Type: LD57.Pickups.PickupComponent
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Assembly location: C:\Users\Admin\Desktop\RE\DepthDelver\LD57.dll

using LD57.Objects;
using LD57.Physics;
using LD57.Spawn;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;

#nullable disable
namespace LD57.Pickups
{
  public abstract class PickupComponent : GameObjectComponent
  {
    private const float kBigTime = 2f;
    private Vector2 m_startPos;
    private Ease m_ease = new Ease();
    private AABB m_aabb;
    private bool m_big;
    protected string m_sound = "Pickup";

    public PickupComponent(
      Entity parent,
      LevelState level,
      SpawnPoint spawnPoint,
      string anim,
      bool big = false)
      : base(parent, level, spawnPoint)
    {
      this.m_anim = new AnimComponent(parent, anim, this.GetCamera());
      this.m_stateMachine.SetNextState(0);
      this.m_anim.Play("Idle");
      this.m_anim.m_depth = 0.2f;
      this.m_startPos = this.GetPos();
      this.m_aabb = new AABB(Vector2.Zero, 8f, 8f);
      this.m_big = big;
    }

    protected abstract void OnCollect();

    protected abstract void OnCollectEnd();

    protected virtual bool CanCollect() => true;

    public override void InitState()
    {
      switch (this.m_stateMachine.GetState())
      {
        case 1:
          this.m_anim.Play("Collect", 1);
          this.m_cullable = false;
          this.m_ease.Init(Ease.EaseType.SinOut, this.m_big ? 2f : 0.333333343f);
          this.m_startPos = this.GetPos();
          this.OnCollect();
          if (this.m_spawnPoint != null)
            this.m_spawnPoint.SetDead();
          if (this.m_big)
            this.GetLevel().m_player.UpgradeStart();
          AudioManager.PlaySFX(this.m_sound);
          break;
      }
    }

    public override void UpdateState(GameTime gameTime)
    {
      switch (this.m_stateMachine.GetState())
      {
        case 0:
          this.SetPos(this.m_startPos + new Vector2(0.0f, 2.5f * (float) Math.Sin(((double) this.m_stateMachine.GetStateTime() + (double) this.m_startPos.X * 0.10000000149011612) * Math.PI * 2.0 / 2.0)));
          if (!this.CanCollect() || this.GetLevel().m_player.IsDead())
            break;
          AABB aabb = this.m_aabb;
          aabb.m_center += this.GetPos();
          if (!this.GetLevel().m_player.GetPhysics().GetAABB().Intersects(aabb) || this.GetLevel().GetPhysicsManager().Overlap(new AABB(this.GetPos(), 1f, 1f), PhysicsComponent.CollideMask.Object))
            break;
          this.m_stateMachine.SetNextState(1);
          break;
        case 1:
          if (!this.m_ease.IsDone())
          {
            this.m_ease.Update(gameTime.GetElapsedSeconds());
            if (this.m_big)
              this.SetPos(Vector2.Lerp(this.m_startPos, this.GetLevel().m_player.GetPos() + new Vector2(0.0f, -32f), this.m_ease.GetValue()));
            else
              this.SetPos(this.m_startPos + new Vector2(0.0f, -24f * this.m_ease.GetValue()));
          }
          if ((double) this.m_stateMachine.GetStateTime() < (this.m_big ? 2.0833332538604736 : 0.4166666567325592))
            break;
          this.m_stateMachine.SetNextState(2);
          break;
        case 2:
          float num = MathHelper.Lerp(0.0f, 1000f, MathHelper.Clamp(this.m_stateMachine.GetStateTime() / 1f, 0.0f, 1f)) * gameTime.GetElapsedSeconds();
          Vector2 vector2 = this.GetLevel().m_player.GetPos() - this.GetPos();
          if ((double) vector2.LengthSquared() <= (double) num * (double) num)
          {
            this.OnCollectEnd();
            if (this.m_big)
              this.GetLevel().m_player.UpgradeEnd();
            else
              AudioManager.PlaySFX("Collect");
            this.GetParent().Destroy();
            break;
          }
          this.SetPos(this.GetPos() + vector2.NormalizedCopy() * num);
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
      Collect,
      Follow,
    }
  }
}
