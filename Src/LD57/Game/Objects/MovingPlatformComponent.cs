// Decompiled with JetBrains decompiler
// Type: LD57.Objects.MovingPlatformComponent
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Assembly location: C:\Users\Admin\Desktop\RE\DepthDelver\LD57.dll

using LD57.Physics;
using LD57.Spawn;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace LD57.Objects
{
  public class MovingPlatformComponent : GameObjectComponent
  {
    private Vector2 m_start = Vector2.Zero;
    private Vector2 m_end = Vector2.Zero;
    private float m_idleTime = -1f;
    private float m_moveTime = 1f;
    private bool m_reverse;
    private Ease m_ease = new Ease();
    private readonly string[] kDirNames = new string[4]
    {
      "Up",
      "Down",
      "Left",
      "Right"
    };
    private readonly Vector2[] kDirVecs = new Vector2[4]
    {
      new Vector2(0.0f, -1f),
      new Vector2(0.0f, 1f),
      new Vector2(-1f, 0.0f),
      new Vector2(1f, 0.0f)
    };

    public MovingPlatformComponent(
      Entity parent,
      LevelState level,
      SpawnPoint spawnPoint,
      bool solid)
      : base(parent, level, spawnPoint)
    {
      this.m_anim = new AnimComponent(parent, solid ? "PlatformSolid" : "PlatformSemiSolid", this.GetCamera());
      this.m_stateMachine.SetNextState(0);
      this.m_anim.Play("Idle");
      this.m_anim.m_depth = 0.3f;
      this.m_physics = new PhysicsComponent(parent, this.GetLevel().GetPhysicsManager(), true);
      this.m_physics.SetExtents(new Vector2(8f, 8f));
      this.m_physics.m_collideAs = solid ? PhysicsComponent.CollideMask.Solid : PhysicsComponent.CollideMask.SemiSolid;
      this.m_start = this.GetPos();
      this.m_end = this.GetPos();
      if (spawnPoint == null)
        return;
      float propertyFloat = spawnPoint.GetData().GetPropertyFloat("Dist");
      if ((double) propertyFloat > 0.0)
      {
        for (int index = 0; index < ((IEnumerable<string>) this.kDirNames).Count<string>(); ++index)
        {
          if (spawnPoint.GetData().HasProperty(this.kDirNames[index]))
          {
            this.m_end += this.kDirVecs[index] * propertyFloat;
            break;
          }
        }
        this.m_idleTime = spawnPoint.GetData().GetPropertyFloat("IdleTime", this.m_idleTime);
        this.m_moveTime = !spawnPoint.GetData().HasProperty("Speed") ? spawnPoint.GetData().GetPropertyFloat("MoveTime", this.m_moveTime) : (this.m_start - this.m_end).Length() / spawnPoint.GetData().GetPropertyFloat("Speed", 1f);
      }
      if (!spawnPoint.GetData().HasProperty("Spike"))
        return;
      SpikesComponent spikesComponent = new SpikesComponent(parent, this.GetLevel(), (SpawnPoint) null, new Vector2?(new Vector2(0.0f, -16f)));
    }

    public override void InitState()
    {
      switch ((MovingPlatformComponent.State) this.m_stateMachine.GetState())
      {
        case MovingPlatformComponent.State.Move:
          this.m_ease.Init(Ease.EaseType.Linear, this.m_moveTime);
          break;
      }
    }

    public override void UpdateState(GameTime gameTime)
    {
      switch ((MovingPlatformComponent.State) this.m_stateMachine.GetState())
      {
        case MovingPlatformComponent.State.Idle:
          if ((double) this.m_idleTime < 0.0 || (double) this.m_stateMachine.GetStateTime() < (double) this.m_idleTime)
            break;
          this.m_stateMachine.SetNextState(1);
          break;
        case MovingPlatformComponent.State.Move:
          float elapsedSeconds = gameTime.GetElapsedSeconds();
          if ((double) elapsedSeconds <= 0.0)
            break;
          this.m_ease.Update(elapsedSeconds);
          this.m_physics.Move((Vector2.Lerp(this.m_start, this.m_end, this.m_reverse ? 1f - this.m_ease.GetValue() : this.m_ease.GetValue()) - this.GetPos()) / elapsedSeconds);
          if (!this.m_ease.IsDone())
            break;
          this.m_stateMachine.SetNextState(0);
          break;
      }
    }

    public override void ExitState()
    {
      switch ((MovingPlatformComponent.State) this.m_stateMachine.GetLastState())
      {
        case MovingPlatformComponent.State.Move:
          this.m_reverse = !this.m_reverse;
          break;
      }
    }

    private enum State
    {
      Idle,
      Move,
    }
  }
}
