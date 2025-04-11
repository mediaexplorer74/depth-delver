// Decompiled with JetBrains decompiler
// Type: LD57.Enemies.SnakeEnemyComponent
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
  public class SnakeEnemyComponent : EnemyComponent
  {
    private const float kGravity = 640f;
    private const float kSpeed = 125f;
    private float m_flipTime;
    private const float kFlipTime = 0.0833333358f;

    public SnakeEnemyComponent(Entity parent, LevelState level, SpawnPoint spawnPoint)
      : base(parent, level, spawnPoint)
    {
      this.m_anim = new AnimComponent(this.GetParent(), "Snake", this.GetCamera());
      this.m_anim.m_depth = 0.4f;
      this.m_anim.Play("Walk");
      this.m_physics = new PhysicsComponent(this.GetParent(), level.GetPhysicsManager());
      this.m_physics.SetExtents(new Vector2(7f, 8f));
      this.m_physics.SetOffset(new Vector2(0.0f, 2f));
      this.m_physics.m_acceleration.Y = 640f;
      this.m_physics.m_collideAs = PhysicsComponent.CollideMask.Enemy;
      this.m_physics.m_collideWith = PhysicsComponent.CollideMask.WorldAll;
      this.m_combat = new CombatComponent(this.GetParent(), level.GetCombatManager(), (CombatImplementor) this);
      this.m_combat.m_attackBoxes.Add(new AABB(new Vector2(0.0f, 2f), 7f, 8f));
      this.m_combat.m_defenseBoxes.Add(new AABB(new Vector2(0.0f, 2f), 7f, 8f));
      this.m_combat.m_attackMask = CombatComponent.CombatMask.Player;
      this.m_combat.m_defenseMask = CombatComponent.CombatMask.Enemy;
      this.m_combat.m_health = this.m_combat.m_maxHealth = 2;
      this.m_stateMachine.SetNextState(1);
    }

    public override void InitState()
    {
      if (this.m_stateMachine.GetState() == 1)
      {
        this.m_anim.Play("Walk");
        this.m_flipTime = 0.0f;
      }
      else
        base.InitState();
    }

    public override void UpdateState(GameTime gameTime)
    {
      if (this.m_stateMachine.GetState() == 1)
      {
        if (this.m_physics.IsCollideDir(this.GetParent().GetFacingX()) || !this.GetLevel().GetPhysicsManager().Overlap(new AABB(this.GetPos() + new Vector2(this.GetParent().m_facing.X * 16f, 8f), 6f, 8f), this.m_physics.m_collideWith, true))
        {
          this.GetParent().m_facing.X *= -1f;
          this.m_flipTime = 0.0833333358f;
        }
        if (this.m_anim.GetFrameIndex() != 0)
        {
          float sequenceFrameDuration = this.m_anim.GetCurSequenceFrameDuration(0);
          float num1 = this.m_anim.GetCurSequenceDuration() - sequenceFrameDuration;
          float num2 = (float) ((0.5 - (double) Math.Abs((this.m_anim.GetSequenceTime() - sequenceFrameDuration) / num1 - 0.5f)) * 2.0);
          if ((double) this.m_flipTime > 0.0)
          {
            num2 = MathHelper.Lerp(num2, 0.0f, this.m_flipTime / 0.0833333358f);
            this.m_flipTime -= gameTime.GetElapsedSeconds();
          }
          this.m_physics.Move(125f * this.GetParent().GetFacingX() 
              * Ease.CalcValue(Ease.EaseType.QuadInOut, MathHelper.Clamp(num2, 0.0f, 1f)));
        }
        else
          this.m_flipTime = 0.0f;
      }
      else
        base.UpdateState(gameTime);
    }

    public override void ExitState()
    {
      if (this.m_stateMachine.GetLastState() == 1)
        return;
      base.ExitState();
    }

    private enum State
    {
      Walk = 1,
    }
  }
}
