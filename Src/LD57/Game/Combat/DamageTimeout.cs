
// Type: LD57.Combat.DamageTimeout
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Modded by [M]edia[E]xplorer

#nullable disable
namespace LD57.Combat
{
  internal struct DamageTimeout(float time, int guid)
  {
    private float m_time = time;
    private int m_guid = guid;

    public void Update(float time) => this.m_time -= time;

    public float GetTime() => this.m_time;

    public int GetGuid() => this.m_guid;
  }
}
