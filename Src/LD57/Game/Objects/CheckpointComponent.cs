
// Type: LD57.Objects.CheckpointComponent
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Modded by [M]edia[E]xplorer

using LD57.Spawn;
using Microsoft.Xna.Framework;

#nullable disable
namespace LD57.Objects
{
  public class CheckpointComponent : GameObjectComponent
  {
    public bool m_debugDraw;

    public CheckpointComponent(Entity parent, LevelState level, SpawnPoint spawnPoint)
      : base(parent, level, spawnPoint)
    {
      this.m_anim = new AnimComponent(parent, "Checkpoint", this.GetCamera());
      this.m_anim.Play("Idle");
      this.m_anim.m_depth = 0.3f;
      this.m_stateMachine.SetNextState(level.m_playerSpawn == this.GetParent().m_position ? 2 : 0);
    }

    public override void InitState()
    {
      switch (this.m_stateMachine.GetState())
      {
        case 0:
          this.m_anim.Play("Idle");
          break;
        case 1:
          this.m_anim.Play("Activate", 1);
          this.GetLevel().SetActiveCheckpoint(this);
          break;
        case 2:
          this.m_anim.Play("Active");
          break;
      }
    }

    public override void UpdateState(GameTime gameTime)
    {
      switch (this.m_stateMachine.GetState())
      {
        case 0:
          if (this.GetLevel().m_player.IsDead() || !this.GetLevel().m_player.GetPhysics().GetAABB().Intersects(this.GetArea()))
            break;
          AudioManager.PlaySFX("Checkpoint");
          this.m_stateMachine.SetNextState(1);
          break;
        case 1:
          if (!this.m_anim.IsDone())
            break;
          this.m_stateMachine.SetNextState(2);
          break;
        case 2:
          if (!(this.GetLevel().m_playerSpawn != this.GetParent().m_position))
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

    public AABB GetArea() => new AABB(this.GetPos() + new Vector2(6f, 0.0f), 16f, 20f);

    public override void Draw(GameTime gameTime)
    {
      if (this.m_debugDraw)
      {
        Color blue = Color.Blue with { A = 100 };
        SpriteManager.DrawAABB(this.GetArea(), blue, this.GetCamera(), 1f);
      }
      base.Draw(gameTime);
    }

    private enum State
    {
      Idle,
      Activate,
      Active,
    }
  }
}
