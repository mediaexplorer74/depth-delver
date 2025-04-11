
// Type: LD57.RandomUtil
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Assembly location: C:\Users\Admin\Desktop\RE\DepthDelver\LD57.dll

using System;

#nullable disable
namespace LD57
{
  internal static class RandomUtil
  {
    private static Random s_random = new Random();

    public static float RandF() => (float) RandomUtil.s_random.NextDouble();

    public static float RandF(float min, float max) => RandomUtil.RandF() * (max - min) + min;

    public static int RandI(int min, int max) => RandomUtil.s_random.Next(min, max);

    public static bool RandB() => (double) RandomUtil.RandF() < 0.5;
  }
}
