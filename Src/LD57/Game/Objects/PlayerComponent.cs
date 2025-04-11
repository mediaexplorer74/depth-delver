
// Type: LD57.Objects.PlayerComponent
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Modded by [M]edia[E]xplorer

using LD57.Combat;
using LD57.Combat.GameManager.Combat;
using LD57.Physics;
using LD57.Pickups;
using LD57.Spawn;
using Microsoft.Xna.Framework;
//using MonoGame.Extended;
using System;

#nullable disable
namespace LD57.Objects
{
  public class PlayerComponent : GameObjectComponent, CombatImplementor
  {
    private const float kGravity = 640f;
    private const float kGravityFast = 1600f;
    private const float kGroundSpeed = 100f;
    private const float kGroundAccel = 450f;
    private const float kGroundDeaccel = 1250f;
    private const float kJumpVel = -270f;
    private const float kAirSpeed = 100f;
    private const float kAirSpeedBack = 90f;
    private const float kAirAccel = 450f;
    private const float kAirAccelBack = 300f;
    private const float kAirDeaccel = 1250f;
    private const float kJumpBuffer = 0.116666667f;
    private const float kAttackBuffer = 0.116666667f;
    private const float kCoytoteTime = 0.166666672f;
    private AnimComponent m_whip;
    private PlayerComponent.Ability m_abilities;
    private Vector2 m_move = Vector2.Zero;
    private float m_jumpBuffer;
    private float m_attackBuffer;
    private float m_coyoteTime;
    private Color kWhipColor = new Color(121, 65, 0);
    private float m_attackTime;
    private float m_whipDist;
    private float m_whipEndTime;
    private const float kWhipAntic = 0.166666672f;
    private const float kWhipDist = 36f;
    private const float kWhipSpeed = 450f;
    private const float kZipSpeed = 350f;
    private const float kWhipEndTime = 0.2f;
    private bool m_wasOnGround = true;
    private float m_iFrames;
    private const float kIFrames = 1f;
    private float m_lastGroundedY;
    private HookComponent m_hook;
    private PhysicsComponent m_grapplePhyscis;
    private Vector2 m_grapplePoint = Vector2.Zero;
    private bool m_grappleBlocked;

    public PlayerComponent(Entity parent, LevelState level)
      : base(parent, level, (SpawnPoint) null)
    {
      this.m_anim = new AnimComponent(this.GetParent(), "Player", this.GetCamera());
      this.m_anim.Play("Idle");
      this.m_whip = new AnimComponent(this.GetParent(), "Whip", this.GetCamera());
      this.m_whip.Play(this.GetLevel().m_canGrapple ? "Gold" : "Idle");
      this.m_whip.m_visible = false;
      this.m_whip.m_depth = this.m_anim.m_depth + 1f / 1000f;
      this.m_physics = new PhysicsComponent(this.GetParent(), this.GetLevel().GetPhysicsManager());
      this.m_physics.SetExtents(new Vector2(4f, 9f));
      this.m_physics.SetOffset(new Vector2(0.0f, 1f));
      this.m_physics.m_acceleration.Y = 640f;
      this.m_physics.m_collideAs = PhysicsComponent.CollideMask.Player;
      this.m_physics.m_collideWith = PhysicsComponent.CollideMask.WorldAll;
      this.m_combat = new CombatComponent(this.GetParent(), level.GetCombatManager(), this);
      this.m_combat.m_attackBoxes.Add(new AABB(new Vector2(0.0f, 0.0f), 1f, 1f));
      this.m_combat.m_attackBoxes.Add(new AABB(new Vector2(0.0f, 0.0f), 2f, 2f));
      this.m_combat.m_defenseBoxes.Add(new AABB(new Vector2(0.0f, 1f), 7f, 8f));
      this.m_combat.m_attackMask = CombatComponent.CombatMask.Enemy | CombatComponent.CombatMask.Object;
      this.m_combat.m_defenseMask = CombatComponent.CombatMask.Player;
      this.m_combat.m_attackEnabled = false;
      this.m_combat.m_health = this.m_combat.m_maxHealth = 3;
      this.m_combat.m_damage.m_timeout = 2f;
      this.m_lastGroundedY = this.GetPos().Y;
      this.m_stateMachine.SetNextState(0);
    }

    public override void Update(GameTime gameTime)
    {
      if (InputManager.IsPressed("Jump"))
        this.m_jumpBuffer = 0.116666667f;
      else if ((double) this.m_jumpBuffer > 0.0)
        this.m_jumpBuffer -= gameTime.GetElapsedSeconds();
      if (InputManager.IsPressed("Attack"))
        this.m_attackBuffer = 0.116666667f;
      else if ((double) this.m_attackBuffer > 0.0)
        this.m_attackBuffer -= gameTime.GetElapsedSeconds();
      this.UpdateLocomotion(gameTime);
      if ((double) this.m_iFrames > 0.0)
      {
        this.m_iFrames -= gameTime.GetElapsedSeconds();
        if ((double) this.m_iFrames <= 0.0 || (double) this.m_iFrames * 30.0 % 2.0 < 1.0)
          this.m_anim.m_color = Color.White;
        else
          this.m_anim.m_color = new Color(1f, 0.25f, 0.25f);
      }
      base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
      if (!this.m_whip.m_visible)
        return;
      Vector2 vector2_1 = this.GetParent().m_position + new Vector2(5f, -1f) * this.GetParent().m_facing;
      Vector2 vector2_2 = vector2_1 + new Vector2(this.m_whip.m_localPosition.X - 7f * this.GetParent().m_facing.X, 0.0f);
      SpriteManager.DrawAABB(new AABB((vector2_1 + vector2_2) / 2f, (float) ((double) Math.Abs(vector2_1.X - vector2_2.X) / 2.0 + 0.5), 1f), this.kWhipColor, this.GetCamera(), this.m_anim.m_depth - 1f / 1000f);
    }

    public override void InitState()
    {
      switch (this.m_stateMachine.GetState())
      {
        case 0:
          this.m_abilities = PlayerComponent.Ability.GroundMove
                        | PlayerComponent.Ability.Jump | PlayerComponent.Ability.Fall
                        | PlayerComponent.Ability.AirMove | PlayerComponent.Ability.Attack;
          this.m_anim.Play("Idle");
          break;
        case 1:
          this.m_abilities = PlayerComponent.Ability.GroundMove
                        | PlayerComponent.Ability.Jump | PlayerComponent.Ability.Fall 
                        | PlayerComponent.Ability.AirMove | PlayerComponent.Ability.Attack;
          this.m_anim.Play("Walk");
          break;
        case 2:
          this.m_abilities = PlayerComponent.Ability.AirMove 
                        | PlayerComponent.Ability.Attack | PlayerComponent.Ability.FastFall;
          this.m_anim.Play("Jump", 1, forceReset: true);
          this.m_physics.m_velocity.Y = -270f;
          this.m_coyoteTime = 0.0f;
          this.m_jumpBuffer = 0.0f;
          AudioManager.PlaySFX("Jump");
          break;
        case 3:
          this.m_abilities = PlayerComponent.Ability.Jump | PlayerComponent.Ability.AirMove | PlayerComponent.Ability.Attack;
          this.m_anim.Play("Fall", 1, forceReset: true);
          break;
        case 4:
          this.m_abilities = PlayerComponent.Ability.GroundMove | PlayerComponent.Ability.Jump | PlayerComponent.Ability.Fall | PlayerComponent.Ability.Attack;
          this.m_anim.Play("Land", 1, forceReset: true);
          break;
        case 5:
          this.m_abilities = PlayerComponent.Ability.AttackMove | this.m_abilities & PlayerComponent.Ability.FastFall;
          this.m_anim.Play(this.m_physics.IsCollideGround() ? "AttackReady" : "AttackReadyAir", 1, forceReset: true);
          this.m_attackTime = 0.0f;
          this.m_attackBuffer = 0.0f;
          break;
        case 6:
          this.m_abilities = PlayerComponent.Ability.AttackMove | this.m_abilities & PlayerComponent.Ability.FastFall;
          this.m_anim.Play(this.m_physics.IsCollideGround() ? "Attack" : "AttackAir", 1, forceReset: true);
          this.m_whip.m_visible = true;
          this.m_whip.Play(this.GetLevel().m_canGrapple ? "Gold" : "Idle");
          this.m_whipDist = 0.0f;
          this.m_whipEndTime = 0.0f;
          this.UpdateWhipPos();
          this.m_combat.m_attackEnabled = true;
          this.m_combat.m_damage.CycleGUID();
          this.m_combat.m_damage.m_dir = this.GetParent().GetFacingX();
          this.m_grappleBlocked = false;
          AudioManager.PlaySFX("Whip");
          break;
        case 7:
          this.m_abilities = (PlayerComponent.Ability) 0;
          this.m_iFrames = 1f;
          this.m_physics.m_velocity = new Vector2(-125f * this.GetParent().m_facing.X, -110f);
          this.m_anim.Play("GetHit");
          this.GetCamera().StartShake(2.5f, 0.0833333358f);
          break;
        case 8:
          this.m_abilities = (PlayerComponent.Ability) 0;
          this.m_combat.m_defenseEnabled = false;
          this.m_anim.Play("DeathRise");
          this.m_physics.m_velocity = new Vector2(0.0f, -250f);
          this.m_physics.m_collideWith = PhysicsComponent.CollideMask.None;
          this.GetCamera().StartShake(2.5f, 0.333333343f);
          this.m_anim.m_shake.Init(2.5f, 0.166666672f);
          this.GetCamera().SetTracking((GameObjectComponent) null);
          this.GetLevel().StopMusic();
          AudioManager.PlaySFX("Death", 3f);
          break;
        case 9:
          this.m_abilities = (PlayerComponent.Ability) 0;
          this.m_combat.m_defenseEnabled = false;
          this.m_combat.m_attackEnabled = true;
          this.m_anim.Play("AttackAir");
          this.m_physics.m_velocity = Vector2.Zero;
          this.m_physics.m_acceleration.Y = 0.0f;
          this.m_physics.m_collideWith = PhysicsComponent.CollideMask.None;
          Vector2 pos = this.GetPos();
          Vector2 vector2 = this.m_hook != null ? this.m_hook.GetPos() : this.m_grapplePoint;
          pos.Y = vector2.Y + 1f;
          this.SetPos(pos);
          this.m_whipDist = Math.Abs(this.GetPos().X - vector2.X) - 7f;
          this.m_whip.m_visible = true;
          if ((double) this.m_whipDist <= 1.0)
            this.m_stateMachine.SetNextState(this.m_hook != null ? 10 : 11);
          else if (this.UpdateZipPos())
            this.m_stateMachine.SetNextState(3);
          if (this.m_grapplePhyscis != null)
            this.m_grapplePhyscis.m_carry.Add(this.m_physics);
          AudioManager.PlaySFX("Latch");
          break;
        case 10:
          this.m_abilities = PlayerComponent.Ability.AirMove | PlayerComponent.Ability.Attack;
          this.m_anim.Play("Jump", 1, forceReset: true);
          this.m_physics.m_velocity.Y = -225f;
          this.m_physics.m_velocity.X = (float) (50.0 
                        * (double) this.GetParent().m_facing.X + 10.0 * (double) this.GetInputDir());
          this.m_coyoteTime = 0.0f;
          this.m_jumpBuffer = 0.0f;
          AudioManager.PlaySFX("Jump");
          break;
        case 11:
          this.m_abilities = (PlayerComponent.Ability) 0;
          this.SetPos(this.m_grapplePoint + new Vector2(this.GetParent().m_facing.X * -5f, 1f));
          this.m_physics.m_velocity = Vector2.Zero;
          this.m_physics.m_acceleration.Y = 0.0f;
          if (this.m_grapplePhyscis == null)
            break;
          this.m_grapplePhyscis.m_carry.Add(this.m_physics);
          break;
        case 12:
          this.m_abilities = PlayerComponent.Ability.AttackMove;
          this.m_attackTime = 0.0f;
          this.m_anim.Play("Upgrade");
          this.m_combat.m_defenseEnabled = false;
          break;
      }
    }

    public override void UpdateState(GameTime gameTime)
    {
      switch (this.m_stateMachine.GetState())
      {
        case 0:
          if ((double) this.m_move.X != 0.0)
          {
            this.m_stateMachine.SetNextState(1);
            break;
          }
          break;
        case 1:
          if ((double) this.m_move.X == 0.0)
          {
            this.m_stateMachine.SetNextState(0);
            break;
          }
          break;
        case 2:
        case 10:
          if (this.m_stateMachine.GetStateFrameCount() > 0 && this.m_physics.IsCollideGround())
          {
            this.OnLandStateTransition();
            break;
          }
          if ((double) this.m_physics.m_velocity.Y > 0.0)
          {
            this.m_stateMachine.SetNextState(3);
            break;
          }
          break;
        case 3:
          if (this.m_physics.IsCollideGround())
          {
            this.OnLandStateTransition();
            break;
          }
          break;
        case 4:
          if ((double) this.m_move.X != 0.0)
          {
            this.m_stateMachine.SetNextState(1);
            break;
          }
          if (this.m_anim.IsDone())
          {
            this.m_stateMachine.SetNextState(0);
            break;
          }
          break;
        case 5:
          if (this.m_physics.IsCollideGround() 
                        && (double) this.m_stateMachine.GetStateTime() < 0.0833333358168602)
          {
            float inputDir = this.GetInputDir();
            if ((double) inputDir != 0.0)
              this.GetParent().m_facing.X = inputDir;
          }
          if ((double) this.m_stateMachine.GetStateTime() >= 0.1666666716337204)
          {
            this.m_stateMachine.SetNextState(6);
            break;
          }
          if (this.m_physics.IsCollideGround() != this.m_wasOnGround)
          {
            this.m_anim.Swap(this.m_physics.IsCollideGround() ? "AttackReady" : "AttackReadyAir");
            break;
          }
          break;
        case 6:
          if ((double) this.m_whipDist < 36.0)
            this.m_whipDist = Math.Min(this.m_whipDist + 450f * gameTime.GetElapsedSeconds(), 36f);
          else if ((double) this.m_whipEndTime >= 0.20000000298023224)
            this.m_stateMachine.SetNextState(0);
          else
            this.m_whipEndTime += gameTime.GetElapsedSeconds();
          if (this.m_physics.IsCollideGround() != this.m_wasOnGround)
            this.m_anim.Swap(this.m_physics.IsCollideGround() ? "Attack" : "AttackAir");
          this.UpdateWhipPos();
          AABB attackBox = this.m_combat.m_attackBoxes[0];
          attackBox.m_center += this.GetPos();
          foreach (HookComponent hook in this.GetLevel().GetHooks())
          {
            if (hook.GetArea().Intersects(attackBox))
            {
              this.m_hook = hook;
              this.m_stateMachine.SetNextState(9);
              break;
            }
          }
          if (this.GetLevel().m_canGrapple && !this.m_grappleBlocked && this.m_hook == null
                        && !this.m_physics.IsCollideGround())
          {
            float time = 0.0f;
            Vector2 vector2_1 = new Vector2(5f, -1f) * this.GetParent().m_facing;
            Vector2 vector2_2 = vector2_1 + new Vector2(this.m_whip.m_localPosition.X - 3f 
                * this.GetParent().m_facing.X, 0.0f);
            AABB aabb1 = new AABB(vector2_1 + this.GetPos(), 1f, 1f);
            if (this.GetLevel().GetPhysicsManager().SweepAABB(aabb1, attackBox, vector2_2 - vector2_1, 
                PhysicsComponent.CollideMask.Solid, out time, out this.m_grapplePhyscis))
            {
              this.m_grapplePoint = this.GetPos() + vector2_1 + (vector2_2 - vector2_1) * time;
              this.m_stateMachine.SetNextState(9);
              break;
            }
            break;
          }
          break;
        case 7:
          if (this.m_stateMachine.GetStateFrameCount() > 0 && this.m_physics.IsCollideGround())
          {
            this.OnLandStateTransition();
            break;
          }
          break;
        case 8:
          if ((double) this.m_physics.m_velocity.Y > 0.0)
            this.m_anim.Play("DeathFall", 1);
          if (this.m_stateMachine.CrossedTime(0.5f))
          {
            this.GetLevel().OnDeath();
            break;
          }
          break;
        case 9:
          this.m_whipDist = Math.Max(this.m_whipDist - 350f * gameTime.GetElapsedSeconds(), 0.0f);
          if ((double) this.m_whipDist <= 1.0)
            this.m_stateMachine.SetNextState(this.m_hook != null ? 10 : 11);
          if (this.UpdateZipPos())
          {
            if (this.m_hook != null)
            {
              this.m_stateMachine.SetNextState(10);
              break;
            }
            this.m_grapplePoint = this.GetPos() + new Vector2(this.GetParent().m_facing.X * 5f, -1f);
            this.m_stateMachine.SetNextState(11);
            break;
          }
          break;
        case 11:
          if ((double) this.m_jumpBuffer > 0.0)
          {
            this.GetParent().m_facing.X *= -1f;
            this.m_stateMachine.SetNextState(10);
            break;
          }
          if (this.m_grapplePhyscis != null)
          {
            AABB aabb = this.m_physics.GetAABB();
            Vector2 point = aabb.m_center + aabb.m_extents * this.GetParent().GetFacingX();
            if ((double) Math.Abs(this.m_grapplePhyscis.GetAABB().GetClosestPoint(point).X - point.X) > 4.0)
            {
              this.m_stateMachine.SetNextState(3);
              break;
            }
            this.m_grapplePhyscis.m_carry.Add(this.m_physics);
            break;
          }
          break;
        case 12:
          if (this.m_anim.IsDone())
          {
            this.m_stateMachine.SetNextState(0);
            break;
          }
          break;
      }
      this.m_wasOnGround = this.m_physics.IsCollideGround();
      if ((double) this.GetParent().m_position.Y <= 2000.0)
        return;
      this.m_combat.m_health = 0;
      this.m_stateMachine.SetNextState(8);
    }

    public override void ExitState()
    {
      switch (this.m_stateMachine.GetLastState())
      {
        case 6:
          this.m_whip.m_visible = false;
          this.m_combat.m_attackEnabled = false;
          break;
        case 7:
          this.m_physics.m_velocity = Vector2.Zero;
          break;
        case 9:
          this.m_whip.m_visible = false;
          this.m_combat.m_attackEnabled = false;
          this.m_combat.m_defenseEnabled = true;
          this.m_physics.m_collideWith = PhysicsComponent.CollideMask.WorldAll;
          this.m_physics.m_acceleration.Y = 640f;
          this.m_hook = (HookComponent) null;
          if (this.m_stateMachine.GetState() == 11 || this.m_grapplePhyscis == null)
            break;
          this.m_grapplePhyscis.m_carry.Remove(this.m_physics);
          this.m_grapplePhyscis = (PhysicsComponent) null;
          break;
        case 10:
          this.m_physics.m_velocity.X = 0.0f;
          break;
        case 11:
          this.m_physics.m_acceleration.Y = 640f;
          if (this.m_grapplePhyscis == null)
            break;
          this.m_grapplePhyscis.m_carry.Remove(this.m_physics);
          this.m_grapplePhyscis = (PhysicsComponent) null;
          break;
        case 12:
          this.m_combat.m_defenseEnabled = true;
          break;
      }
    }

    public virtual bool ValidateHit(DamageDesc damage) => (double) this.m_iFrames <= 0.0;

    public virtual void OnDealHit(DamageDesc damage)
    {
      this.m_grappleBlocked = true;
      AudioManager.PlaySFX("Hit");
    }

    public virtual void OnTakeDamage(DamageDesc damage)
    {
      this.GetLevel().GetHUD().ForceHealth(this.m_combat.m_health + damage.m_damage);
      this.m_stateMachine.SetNextStateNow(7, true);
      AudioManager.PlaySFX("Hurt");
    }

    public virtual void OnDeath(DamageDesc damage)
    {
      this.m_combat.m_defenseEnabled = false;
      this.m_stateMachine.SetNextState(8);
      AudioManager.PlaySFX("Hurt");
    }

    private void UpdateLocomotion(GameTime gameTime)
    {
      float elapsedSeconds = gameTime.GetElapsedSeconds();
      if (this.m_physics.IsCollideGround())
        this.m_coyoteTime = 0.166666672f;
      else if ((double) this.m_coyoteTime > 0.0)
        this.m_coyoteTime -= elapsedSeconds;
      if ((this.m_abilities & PlayerComponent.Ability.GroundMove) != (PlayerComponent.Ability) 0 
                && this.m_physics.IsCollideGround())
      {
        float inputDir = this.GetInputDir();
        if ((double) inputDir != 0.0)
        {
          this.AccelToSpeed(450f, 1250f, 100f * inputDir, elapsedSeconds);
          this.GetParent().m_facing.X = inputDir;
        }
        else if ((double) Math.Abs(this.m_move.X) > 0.0)
          this.AccelToSpeed(450f, 1250f, 0.0f, elapsedSeconds);
        this.m_physics.Move(this.m_move);
      }
      if ((this.m_abilities & PlayerComponent.Ability.AirMove) != (PlayerComponent.Ability) 0
                && !this.m_physics.IsCollideGround())
      {
        float inputDir = this.GetInputDir();
        if ((double) inputDir != 0.0)
        {
          int num1 = (double) inputDir == (double) this.GetParent().m_facing.X ? 1 : 0;
          float num2 = num1 != 0 ? 100f : 90f;
          this.AccelToSpeed(num1 != 0 ? 450f : 300f, 1250f, num2 * inputDir, elapsedSeconds);
        }
        else if ((double) Math.Abs(this.m_move.X) > 0.0)
          this.AccelToSpeed(450f, 1250f, 0.0f, elapsedSeconds);
        this.m_physics.Move(this.m_move);
      }
      if ((this.m_abilities & PlayerComponent.Ability.AttackMove) != (PlayerComponent.Ability) 0)
      {
        float inputDir = this.GetInputDir();
        if (this.m_physics.IsCollideGround())
        {
          if ((double) inputDir != 0.0)
            this.AccelToSpeed(450f, 1250f, MathHelper.Lerp(100f, 0.0f,
                MathHelper.Clamp(this.m_attackTime / 0.25f, 0.0f, 1f)) * inputDir, elapsedSeconds);
          else
            this.AccelToSpeed(450f, 1250f, 0.0f, elapsedSeconds);
        }
        else if ((double) inputDir != 0.0)
        {
          float num3 = (double) inputDir == (double) this.GetParent().m_facing.X ? 100f : 90f;
          float accel = 300f;
          float num4 = Math.Min(MathHelper.Lerp(num3, num3 / 1.25f,
              MathHelper.Clamp(this.m_attackTime / 0.25f, 0.0f, 1f)), Math.Max(50f, Math.Abs(this.m_move.X)));
          this.AccelToSpeed(accel, 1250f, num4 * inputDir, elapsedSeconds);
          this.AccelToSpeed(450f, 1250f, num4 * inputDir, elapsedSeconds);
        }
        else
          this.AccelToSpeed(450f, 1250f, 0.0f, elapsedSeconds);
        this.m_physics.Move(this.m_move);
        this.m_attackTime += elapsedSeconds;
      }
      if ((this.m_abilities & PlayerComponent.Ability.Jump) != (PlayerComponent.Ability) 0 
                && (double) this.m_coyoteTime > 0.0 && (double) this.m_jumpBuffer > 0.0)
        this.m_stateMachine.SetNextState(2);
      if ((this.m_abilities & PlayerComponent.Ability.FastFall) != (PlayerComponent.Ability) 0)
      {
        if (!this.m_physics.IsCollideGround())
        {
          if ((double) this.m_physics.m_velocity.Y > 0.0)
            this.m_physics.m_acceleration.Y = 640f;
          else if (!InputManager.IsHeld("Jump"))
            this.m_physics.m_acceleration.Y = 1600f;
        }
      }
      else if ((double) this.m_physics.m_acceleration.Y == 1600.0)
        this.m_physics.m_acceleration.Y = 640f;
      if ((this.m_abilities & PlayerComponent.Ability.Fall) != (PlayerComponent.Ability)
                0 && !this.m_physics.IsCollideGround() && this.m_stateMachine.GetNextState() != 2)
        this.m_stateMachine.SetNextState(3);
      if ((this.m_abilities & PlayerComponent.Ability.Attack) != (PlayerComponent.Ability)
                0 && (double) this.m_attackBuffer > 0.0)
        this.m_stateMachine.SetNextState(5);
      if (!this.m_physics.IsCollideGround())
        return;
      this.m_lastGroundedY = this.GetPos().Y;
    }

    public float GetLestGroundedY() => this.m_lastGroundedY;

    private float GetInputDir()
    {
      if (InputManager.IsHeld("Left"))
      {
        if (!InputManager.IsHeld("Right") || InputManager.IsPressed("Left"))
          return -1f;
        return InputManager.IsPressed("Right") ? 1f : this.GetParent().m_facing.X;
      }
      return InputManager.IsHeld("Right") ? 1f : 0.0f;
    }

    private void AccelToSpeed(float accel, float deaccel, float speed, float time)
    {
      if ((double) this.m_move.X == (double) speed)
        return;
      float num1 = (float) Math.Sign(speed);
      float num2 = accel;
      if ((double) Math.Abs(this.m_move.X) > (double) Math.Abs(speed) || (double) this.m_move.X != 0.0 && (double) Math.Sign(this.m_move.X) != (double) num1)
        num2 = deaccel;
      float num3 = num2 * time;
      if ((double) Math.Abs(this.m_move.X - speed) <= (double) num3)
        this.m_move.X = speed;
      else
        this.m_move.X += num3 * (float) Math.Sign(speed - this.m_move.X);
    }

    private void OnLandStateTransition()
    {
      if ((double) this.GetInputDir() != 0.0)
        this.m_stateMachine.SetNextState(1);
      else
        this.m_stateMachine.SetNextState(4);
    }

    private void UpdateWhipPos()
    {
      this.m_whip.m_localPosition = new Vector2(7f + this.m_whipDist, -1f) * this.GetParent().m_facing;
      Vector2 vector2 = new Vector2(5f, -1f) * this.GetParent().m_facing;
      Vector2 center = vector2 + new Vector2(this.m_whip.m_localPosition.X - 3f * this.GetParent().m_facing.X, 0.0f);
      this.m_combat.m_attackBoxes[0] = new AABB((vector2 + center) / 2f, Math.Abs(vector2.X - center.X) / 2f, 1f);
      center.X -= this.GetParent().m_facing.X * 2f;
      this.m_combat.m_attackBoxes[1] = new AABB(center, 2f, 2f);
    }

    private bool UpdateZipPos()
    {
      this.UpdateWhipPos();
      if (this.m_grapplePhyscis != null)
        this.m_grapplePoint += this.m_grapplePhyscis.m_lastDisplacement;
      Vector2 pos = (this.m_hook != null ? this.m_hook.GetPos() : this.m_grapplePoint) - this.m_whip.m_localPosition;
      float time = 0.0f;
      Vector2 offset = pos - this.GetPos();
      AABB aabb = this.GetPhysics().GetAABB();
      AABB aabb2 = aabb;
      aabb2.m_center += offset;
      PhysicsComponent hit = (PhysicsComponent) null;
      if (this.m_hook == null && this.GetLevel().GetPhysicsManager().SweepAABB(aabb, aabb2, offset, PhysicsComponent.CollideMask.Solid | PhysicsComponent.CollideMask.Object, out time, out hit))
      {
        this.m_grapplePhyscis = hit;
        this.SetPos(Vector2.Lerp(this.GetPos(), pos, time) - this.GetParent().GetFacingX() * (1f / 1000f));
        return true;
      }
      this.SetPos(pos);
      return false;
    }

    public void UpgradeStart()
    {
      this.m_stateMachine.SetNextState(12);
      this.GetLevel().StopMusic();
    }

    public void UpgradeEnd()
    {
      if (this.m_stateMachine.GetState() != 12)
        return;
      this.m_anim.Play("Collect", 1);
      this.GetLevel().PlayMusic();
    }

    private enum State
    {
      Idle,
      Walk,
      Jump,
      Fall,
      Land,
      Whip_Ready,
      Whip_Attack,
      GetHit,
      Death,
      Zip,
      ZipJump,
      Hang,
      Upgrade,
    }

    private enum Ability
    {
      GroundMove = 1,
      Jump = 2,
      Fall = 4,
      AirMove = 8,
      Attack = 16, // 0x00000010
      AttackMove = 32, // 0x00000020
      FastFall = 64, // 0x00000040
    }
  }
}
