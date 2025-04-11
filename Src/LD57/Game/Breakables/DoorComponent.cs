
// Type: LD57.Breakables.DoorComponent
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Modded by [M]edia[E]xplorer

using LD57.Combat;
using LD57.Spawn;
using Microsoft.Xna.Framework;

#nullable disable
namespace LD57.Breakables
{
  public class DoorComponent : BreakableComponent
  {
    public DoorComponent(Entity parent, LevelState level, SpawnPoint spawnPoint)
      : base(parent, level, spawnPoint, "Door")
    {
      this.SetPos(this.GetPos() + new Vector2(0.0f, -8f));
      this.m_physics.SetExtents(new Vector2(8f, 16f));
      this.m_combat.m_defenseBoxes[0] = new AABB(Vector2.Zero, 8f, 16f);
    }

        public override void OnDealHit(DamageDesc damage)
        {
            throw new System.NotImplementedException();
        }

        public override bool ValidateHit(DamageDesc damage)
        {
            throw new System.NotImplementedException();
        }

        protected override bool CanBreak() => this.GetLevel().m_key;

    protected override void OnBreak()
    {
      this.GetLevel().m_key = false;
      this.GetLevel().m_keyHUD = false;
      this.m_spawnPoint.SetDead();
    }
  }
}
