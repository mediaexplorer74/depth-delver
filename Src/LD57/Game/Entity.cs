
// Type: LD57.Entity
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Assembly location: C:\Users\Admin\Desktop\RE\DepthDelver\LD57.dll

using Microsoft.Xna.Framework;
using System.Collections.Generic;

#nullable disable
namespace LD57
{
  public class Entity
  {
    private Entity m_parent;
    private List<Component> m_components = new List<Component>();
    public Vector2 m_position = Vector2.Zero;
    public Vector2 m_facing = new Vector2(1f, -1f);
    private bool m_queueDestroy;

    public Entity(Entity parent) => this.m_parent = parent;

    public void AddComponent(Component component) => this.m_components.Add(component);

    public void Destroy() => this.m_queueDestroy = true;

    public bool IsDestroyed() => this.m_queueDestroy;

    public Vector2 GetFacingX() => new Vector2(this.m_facing.X, 0.0f);

    public Vector2 GetFacingY() => new Vector2(0.0f, this.m_facing.Y);

    public void Update(GameTime gameTime)
    {
      for (int index = 0; index < this.m_components.Count; ++index)
        this.m_components[index].Update(gameTime);
    }

    public void Draw(GameTime gameTime)
    {
      for (int index = 0; index < this.m_components.Count; ++index)
        this.m_components[index].Draw(gameTime);
    }
  }
}
