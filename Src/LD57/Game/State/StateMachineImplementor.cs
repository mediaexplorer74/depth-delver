
// Type: LD57.State.StateMachineImplementor
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Assembly location: C:\Users\Admin\Desktop\RE\DepthDelver\LD57.dll

using Microsoft.Xna.Framework;

#nullable disable
namespace LD57.State
{
  public interface StateMachineImplementor
  {
    void InitState();

    void UpdateState(GameTime gameTime);

    void ExitState();
  }
}
