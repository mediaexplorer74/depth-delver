
// Type: LD57.Combat.DamageDesc
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;

#nullable disable
namespace LD57.Combat
{
  public struct DamageDesc
  {
    public int m_damage;
    public Vector2 m_dir;
    public CombatComponent m_owner;
    public CombatComponent m_origOwner;
    public CombatComponent m_defender;
    public float m_timeout;
    public int m_guid;
    private static int s_nextGuid;

    public DamageDesc()
    {
      this.m_guid = 0;
      this.m_damage = 1;
      this.m_dir = Vector2.Zero;
      this.m_owner = (CombatComponent) null;
      this.m_origOwner = (CombatComponent) null;
      this.m_defender = (CombatComponent) null;
      this.m_timeout = 0.0f;
      this.CycleGUID();
    }

    public void CycleGUID()
    {
      this.m_guid = DamageDesc.s_nextGuid;
      ++DamageDesc.s_nextGuid;
    }
  }
}
