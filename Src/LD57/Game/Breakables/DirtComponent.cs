
// Type: LD57.Breakables.DirtComponent
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Modded by [M]edia[E]xplorer

using LD57.Combat;
using LD57.Spawn;
using Microsoft.Xna.Framework;

#nullable disable
namespace LD57.Breakables
{
  public class DirtComponent : BreakableComponent
  {
    public DirtComponent(Entity parent, LevelState level, SpawnPoint spawnPoint, bool big)
      : base(parent, level, spawnPoint, big ? "DirtBig" : "Dirt")
    {
      if (!big)
        return;
      this.SetPos(this.GetPos() + new Vector2(8f, -8f));
      this.m_physics.SetExtents(new Vector2(16f, 16f));
      this.m_combat.m_defenseBoxes[0] = new AABB(Vector2.Zero, 16f, 16f);
    }

    public override void OnDealHit(DamageDesc damage)
    {
        this.m_combat.DealHit(damage);
    }

    public override bool ValidateHit(DamageDesc damage)
    {
        bool result = this.m_combat.ValidateHit(damage);
        return result;
    }
  }
}
