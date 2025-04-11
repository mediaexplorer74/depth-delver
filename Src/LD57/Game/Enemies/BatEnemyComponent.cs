// Decompiled with JetBrains decompiler
// Type: LD57.Enemies.BatEnemyComponent
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Assembly location: C:\Users\Admin\Desktop\RE\DepthDelver\LD57.dll

using LD57.Combat;
using LD57.Physics;
using LD57.Spawn;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;

#nullable disable
namespace LD57.Enemies
{
  public class BatEnemyComponent : EnemyComponent
  {
    private const float kSpeed = 50f;
    private const float kSwoopTime = 1f;
    private const float kSwoopDist = 96f;
    private float m_flapTime;
    private float m_flip = 1f;
    private Vector2 m_swoopStart;
    private float m_swoopEnd;
    private Ease m_ease = new Ease();
    private bool m_aggro;

    public BatEnemyComponent(Entity parent, LevelState level, SpawnPoint spawnPoint)
      : base(parent, level, spawnPoint)
    {
      this.m_anim = new AnimComponent(this.GetParent(), "Bat", this.GetCamera());
      this.m_anim.m_depth = 0.4f;
      this.m_anim.Play("Idle");
      this.m_physics = new PhysicsComponent(this.GetParent(), level.GetPhysicsManager());
      this.m_physics.SetExtents(new Vector2(8f, 8f));
      this.m_physics.SetOffset(new Vector2(0.0f, 0.0f));
      this.m_combat = new CombatComponent(this.GetParent(), level.GetCombatManager(), (CombatImplementor) this);
      this.m_combat.m_attackBoxes.Add(new AABB(new Vector2(0.0f, 0.0f), 6f, 6f));
      this.m_combat.m_defenseBoxes.Add(new AABB(new Vector2(0.0f, 0.0f), 8f, 8f));
      this.m_combat.m_attackMask = CombatComponent.CombatMask.Player;
      this.m_combat.m_defenseMask = CombatComponent.CombatMask.Enemy;
      this.m_combat.m_health = this.m_combat.m_maxHealth = 2;
      if ((double) this.GetLevel().m_player.GetPos().Y > (double) this.GetPos().Y + 16.0 && spawnPoint != null && spawnPoint.GetData().HasProperty("RiseOffset"))
      {
        Vector2 pos = this.GetPos();
        pos.X += spawnPoint.GetData().GetPropertyFloat("RiseOffset");
        this.SetPos(pos);
      }
      this.m_stateMachine.SetNextState(1);
    }

    public override void InitState()
    {
      switch (this.m_stateMachine.GetState())
      {
        case 1:
          this.m_anim.Play("Idle");
          this.m_anim.SetPlayrate(0.75f);
          break;
        case 2:
          this.m_anim.Play("Idle");
          this.m_anim.SetPlayrate(1f);
          this.m_aggro = true;
          break;
        case 3:
          this.m_anim.Play("Ready");
          this.m_anim.m_shake.Init(1.5f, -1f);
          this.m_physics.m_velocity = Vector2.Zero;
          this.m_flapTime = 0.0f;
          AudioManager.PlaySFX("Surprise");
          break;
        case 4:
          this.m_anim.Play("Idle");
          this.m_anim.SetPlayrate(2f);
          this.m_swoopStart = this.GetPos();
          this.m_swoopEnd = MathHelper.Clamp(this.GetLevel().m_player.GetLestGroundedY(), 
              this.GetPos().Y + 1.6f, this.GetPos().Y + 64f);
          this.m_ease.Init(Ease.EaseType.Linear, 1f);
          AudioManager.PlaySFX("Swoop");
          break;
        default:
          base.InitState();
          break;
      }
    }

    public override void UpdateState(GameTime gameTime)
    {
      switch (this.m_stateMachine.GetState())
      {
        case 1:
          if ((double) this.m_stateMachine.GetStateTime() >= 1.0)
          {
            Vector2 vector2 = this.GetPos() - this.GetLevel().m_player.GetPos();
            if (this.m_aggro || (double) Math.Abs(vector2.X) <= 64.0 && (double) vector2.Y <= 16.0 && (double) vector2.Y >= -64.0 && this.IsOnScreen())
              this.m_stateMachine.SetNextState(2);
          }
          if (this.m_spawnPoint != null && this.GetLevel().GetRoomManager().GetCurRoom() == this.m_spawnPoint.GetRoom())
          {
            AABB area = this.GetLevel().GetRoomManager().GetCurRoom().GetArea();
            area.m_extents.X -= 8f;
            float num = area.m_center.X + area.m_extents.X * this.GetParent().m_facing.X;
            if ((double) this.GetPos().X * (double) this.GetParent().m_facing.X > (double) num * (double) this.GetParent().m_facing.X)
              this.m_flip *= -1f;
          }
          this.m_physics.m_velocity = Vector2.Zero;
          this.m_physics.m_velocity.Y += 25f * (float) Math.Cos((double) this.m_flapTime * Math.PI * 2.0 / 2.0);
          this.m_physics.m_velocity.X += (float) (30.0 * Math.Sin((double) this.m_flapTime * Math.PI * 2.0 / 4.0)) * this.m_flip;
          float num1 = (float) Math.Sign(this.m_physics.m_velocity.X);
          if ((double) num1 != 0.0)
          {
            this.GetParent().m_facing.X = num1;
            break;
          }
          break;
        case 2:
          this.m_physics.m_velocity = Vector2.Zero;
          this.m_physics.m_velocity.Y += 25f * (float) Math.Cos((double) this.m_flapTime * Math.PI * 2.0 / 2.0);
          this.m_physics.m_velocity.X += (float) (25.0 * Math.Sin((double) this.m_flapTime * Math.PI * 2.0 / 4.0)) * this.m_flip;
          Vector2 vector2_1 = new Vector2(this.GetLevel().m_player.GetPos().X, this.GetLevel().m_player.GetLestGroundedY() - 34f) - this.GetPos();
          if ((double) Math.Abs(vector2_1.X) <= 32.0 && (double) vector2_1.Y <= 0.0 && (double) vector2_1.Y >= -16.0)
            this.m_stateMachine.SetNextState(3);
          vector2_1.Normalize();
          this.m_physics.m_velocity += vector2_1 * 50f * MathHelper.Clamp(this.m_stateMachine.GetStateTime() / 1f, 0.0f, 1f);
          float num2 = (float) Math.Sign(vector2_1.X);
          if ((double) num2 != 0.0)
          {
            this.GetParent().m_facing.X = num2;
            break;
          }
          break;
        case 3:
          this.m_physics.m_velocity.Y = (float) (-35.0 * (1.0 - (double) MathHelper.Clamp(this.m_stateMachine.GetStateTime() / 0.333333343f, 0.0f, 1f)));
          this.m_anim.m_shake.Init(1.5f, -1f);
          if ((double) this.m_stateMachine.GetStateTime() >= 0.66666668653488159)
          {
            this.m_stateMachine.SetNextState(4);
            break;
          }
          break;
        case 4:
          this.m_ease.Update(gameTime.GetElapsedSeconds());
          Vector2 pos = new Vector2();
          pos.X = MathHelper.Lerp(this.m_swoopStart.X, this.m_swoopStart.X + 96f * this.GetParent().m_facing.X, Ease.CalcValue(Ease.EaseType.QuadInOut, this.m_ease.GetValue()));
          pos.Y = MathHelper.Lerp(this.m_swoopEnd, this.m_swoopStart.Y, Math.Abs(0.5f - Ease.CalcValue(Ease.EaseType.SinInOut, this.m_ease.GetValue())) * 2f);
          if (this.m_spawnPoint != null && this.GetLevel().GetRoomManager().GetCurRoom() == this.m_spawnPoint.GetRoom())
          {
            AABB area = this.GetLevel().GetRoomManager().GetCurRoom().GetArea();
            area.m_extents.X -= 8f;
            float num3 = area.m_center.X + area.m_extents.X * this.GetParent().m_facing.X;
            if ((double) pos.X * (double) this.GetParent().m_facing.X > (double) num3 * (double) this.GetParent().m_facing.X)
              pos.X = num3;
          }
          this.SetPos(pos);
          if (this.m_ease.IsDone())
          {
            this.m_stateMachine.SetNextState(1);
            break;
          }
          break;
        default:
          base.UpdateState(gameTime);
          break;
      }
      this.m_flapTime += gameTime.GetElapsedSeconds();
    }

    public override void ExitState()
    {
      switch (this.m_stateMachine.GetLastState())
      {
        case 1:
          break;
        case 2:
          break;
        case 3:
          this.m_anim.m_shake.Clear();
          break;
        case 4:
          this.m_anim.m_shake.Clear();
          break;
        default:
          base.ExitState();
          break;
      }
    }

    private enum State
    {
      Idle = 1,
      Chase = 2,
      Ready = 3,
      Swoop = 4,
    }
  }
}
