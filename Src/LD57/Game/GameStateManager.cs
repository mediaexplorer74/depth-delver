// Decompiled with JetBrains decompiler
// Type: LD57.GameStateManager
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Assembly location: C:\Users\Admin\Desktop\RE\DepthDelver\LD57.dll

using Microsoft.Xna.Framework;
using System.Collections.Generic;

#nullable disable
namespace LD57
{
  public class GameStateManager
  {
    private Game1 m_base;
    private List<GameState> m_gameStates;
    private int m_curState;

    public GameStateManager(Game1 b)
    {
      this.m_base = b;
      this.m_curState = 0;
      this.m_gameStates = new List<GameState>();
      this.m_gameStates.Add((GameState) new LevelState(this.m_base));
    }

    public void Update(GameTime gameTime) => this.m_gameStates[this.m_curState].Update(gameTime);

    public void Draw(GameTime gameTime) => this.m_gameStates[this.m_curState].Draw(gameTime);
  }
}
