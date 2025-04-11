// Decompiled with JetBrains decompiler
// Type: LD57.Camera.Room
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Modded by [M]edia[E]xplorer

#nullable disable
namespace LD57.Camera
{
  public class Room
  {
    private AABB m_area;

    public Room(AABB area) => this.m_area = area;

    public AABB GetArea() => this.m_area;
  }
}
