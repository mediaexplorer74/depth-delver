
// Type: LD57.Ease
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace LD57
{
  public class Ease
  {
    private float m_value;
    private float m_valueOld;
    private float m_time;
    private float m_timeTotal = 1f;
    private Ease.EaseType m_type;

    public void Init(Ease.EaseType type, float time)
    {
      this.m_type = type;
      this.m_value = 0.0f;
      this.m_valueOld = 0.0f;
      this.m_time = 0.0f;
      this.m_timeTotal = time;
    }

    public void Update(float time)
    {
      this.m_time = MathHelper.Clamp(this.m_time + time, 0.0f, this.m_timeTotal);
      this.m_valueOld = this.m_value;
      this.m_value = Ease.CalcValue(this.m_type, this.m_time / this.m_timeTotal);
    }

    public float GetValue() => this.m_value;

    public float GetValueOld() => this.m_valueOld;

    public bool IsDone() => (double) this.m_time >= (double) this.m_timeTotal;

    public static float CalcValue(Ease.EaseType type, float value)
    {
      switch (type)
      {
        case Ease.EaseType.SinIn:
          return 1f - Ease.CalcValue(Ease.EaseType.SinOut, 1f - value);
        case Ease.EaseType.SinOut:
          return (float) Math.Sin((double) value * Math.PI * 0.5);
        case Ease.EaseType.SinInOut:
          return (double) value < 0.5 ? Ease.CalcValue(Ease.EaseType.SinIn, value * 2f) * 0.5f : (float) ((double) Ease.CalcValue(Ease.EaseType.SinOut, (float) (((double) value - 0.5) * 2.0)) * 0.5 + 0.5);
        case Ease.EaseType.QuadIn:
          return value * value;
        case Ease.EaseType.QuadOut:
          return 1f - Ease.CalcValue(Ease.EaseType.QuadIn, 1f - value);
        case Ease.EaseType.QuadInOut:
          return (double) value < 0.5 ? Ease.CalcValue(Ease.EaseType.QuadIn, value * 2f) * 0.5f : (float) ((double) Ease.CalcValue(Ease.EaseType.QuadOut, (float) (((double) value - 0.5) * 2.0)) * 0.5 + 0.5);
        case Ease.EaseType.QuadOutIn:
          return 1f - Ease.CalcValue(Ease.EaseType.QuadIn, 1f - value);
        default:
          return value;
      }
    }

    public enum EaseType
    {
      Linear,
      SinIn,
      SinOut,
      SinInOut,
      QuadIn,
      QuadOut,
      QuadInOut,
      QuadOutIn,
    }
  }
}
