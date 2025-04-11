
// Type: LD57.ShakeRect
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Assembly location: C:\Users\Admin\Desktop\RE\DepthDelver\LD57.dll

using Microsoft.Xna.Framework;
using System;
//using System.Numerics;

#nullable disable
namespace LD57
{
  public struct ShakeRect
  {
    public float m_amount;
    public float m_time;
    public Vector2 m_offset;

    public ShakeRect()
    {
      this.m_amount = 0.0f;
      this.m_time = 0.0f;
      this.m_offset = Vector2.Zero;
    }

    public void Init(float amount, float time)
    {
      this.m_amount = amount;
      this.m_time = time;
    }

    public void Clear()
    {
      this.m_amount = 0.0f;
      this.m_time = 0.0f;
      this.m_offset = Vector2.Zero;
    }

    public bool IsDone() => (double) this.m_time == 0.0;

    public void Update(float time)
    {
      if ((double) this.m_time == 0.0)
        return;
      if ((double) this.m_time > 0.0)
        this.m_time = Math.Max(this.m_time - time, 0.0f);
      if ((double) this.m_time == 0.0)
        this.m_offset = Vector2.Zero;
      else
        this.m_offset = new Vector2(RandomUtil.RandF(-this.m_amount, this.m_amount), 
            RandomUtil.RandF(-this.m_amount, this.m_amount));
    }
  }
}
