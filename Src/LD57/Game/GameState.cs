
// Type: LD57.GameState
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Assembly location: C:\Users\Admin\Desktop\RE\DepthDelver\LD57.dll

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace LD57
{
  public abstract class GameState
  {
    public Game1 m_base;
    protected Entity m_GameStateRootEntity;
    protected List<Entity> m_entities;

    public GameState(Game1 b)
    {
      this.m_base = b;
      this.m_GameStateRootEntity = new Entity((Entity) null);
      this.m_entities = new List<Entity>();
      this.m_entities.Add(this.m_GameStateRootEntity);
    }

    public Entity GetRootEntity() => this.m_GameStateRootEntity;

    public virtual void Update(GameTime gameTime)
    {
      for (int index = 0; index < this.m_entities.Count; ++index)
        this.m_entities[index].Update(gameTime);
      for (int index = this.m_entities.Count<Entity>() - 1; index >= 0; --index)
      {
        if (this.m_entities[index].IsDestroyed())
          this.m_entities.RemoveAt(index);
      }
    }

    public virtual void Draw(GameTime gameTime)
    {
      for (int index = 0; index < this.m_entities.Count; ++index)
        this.m_entities[index].Draw(gameTime);
    }
  }
}
