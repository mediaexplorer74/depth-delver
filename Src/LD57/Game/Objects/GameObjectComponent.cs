
// Type: LD57.Objects.GameObjectComponent
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Modded by [M]edia[E]xplorer

using LD57.Breakables;
using LD57.Camera;
using LD57.Combat;
using LD57.Physics;
using LD57.Spawn;
using LD57.State;
using Microsoft.Xna.Framework;

#nullable disable
namespace LD57.Objects
{
  public abstract class GameObjectComponent : Component, StateMachineImplementor
  {
    protected AnimComponent m_anim;
    protected PhysicsComponent m_physics;
    protected CombatComponent m_combat;
    private LevelState m_level;
    protected StateMachine m_stateMachine;
    protected SpawnPoint m_spawnPoint;
    protected bool m_cullable = true;

    public GameObjectComponent(Entity parent, LevelState level, SpawnPoint spawnPoint)
      : base(parent)
    {
      this.m_level = level;
      this.m_stateMachine = new StateMachine((StateMachineImplementor) this);
      this.m_spawnPoint = spawnPoint;
    }

    public AnimComponent GetAnim() => this.m_anim;

    public PhysicsComponent GetPhysics() => this.m_physics;

    public CombatComponent GetCombat() => this.m_combat;

    public LevelState GetLevel() => this.m_level;

    public Vector2 GetPos() => this.GetParent().m_position;

    public void SetPos(Vector2 pos) => this.GetParent().m_position = pos;

    public bool IsDead() => this.m_combat != null && this.m_combat.m_health <= 0;

    public GameCamera GetCamera() => this.m_level.GetCamera();

    public bool IsOnScreen()
    {
      return this.m_anim != null && this.m_anim.GetBound().Intersects(this.m_level.GetCamera().GetViewBound());
    }

    public void Gib(float dir = 0.0f, float scale = 1f)
    {
      for (int x = 0; x < this.m_anim.GetWidth() * 2; ++x)
      {
        for (int y = 0; y < this.m_anim.GetWidth() * 2; ++y)
        {
          Color framePixelColor = this.m_anim.GetFramePixelColor(x, y);
          if (framePixelColor.A > (byte) 0)
          {
            Entity entity = new Entity(this.GetLevel().GetRootEntity());
            entity.m_position = this.GetPos() - new Vector2((float) (this.m_anim.GetWidth() / 2), (float) (this.m_anim.GetHeight() / 2)) + new Vector2((float) x + 0.5f, (float) y + 0.5f);
            GibComponent gibComponent = new GibComponent(entity, this.GetLevel(), framePixelColor);
            gibComponent.GetPhysics().m_velocity.Y = -RandomUtil.RandF(100f, 250f);
            if ((double) dir == 0.0)
              gibComponent.GetPhysics().m_velocity.X = RandomUtil.RandF(-100f, 100f);
            else
              gibComponent.GetPhysics().m_velocity.X = dir * RandomUtil.RandF(100f, 250f);
            gibComponent.GetPhysics().m_velocity *= scale;
            this.GetLevel().AddEntity(entity);
          }
        }
      }
      this.GetParent().Destroy();
    }

    public override void Update(GameTime gameTime)
    {
      this.m_stateMachine.Update(gameTime);
      if (this.m_spawnPoint == null || !this.m_cullable 
                || this.m_level.GetRoomManager().GetCurRoom() == this.m_spawnPoint.GetRoom() || this.IsOnScreen())
        return;
      this.GetParent().Destroy();
    }

    public override void Draw(GameTime gameTime)
    {
    }

    public abstract void InitState();

    public abstract void UpdateState(GameTime gameTime);

    public abstract void ExitState();
  }
}
