// Decompiled with JetBrains decompiler
// Type: LD57.Component
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Assembly location: C:\Users\Admin\Desktop\RE\DepthDelver\LD57.dll

using Microsoft.Xna.Framework;

#nullable disable
namespace LD57
{
  public abstract class Component
  {
    private Entity m_parent;

    public Component(Entity parent)
    {
      this.m_parent = parent;
      parent.AddComponent(this);
    }

    public Entity GetParent() => this.m_parent;

    public abstract void Update(GameTime gameTime);

    public abstract void Draw(GameTime gameTime);
  }
}
