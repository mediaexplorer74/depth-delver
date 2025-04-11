
// Type: LD57.State.StateMachine
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Modded by [M]edia[E]xplorer

using LD57.Pickups;
using Microsoft.Xna.Framework;
//using MonoGame.Extended;

#nullable disable
namespace LD57.State
{
  public class StateMachine
  {
    private StateMachineImplementor m_implementor;
    private int m_state;
    private int m_lastState;
    private int m_nextState;
    private float m_stateTime;
    private float m_stateTimeOld;
    private int m_stateFrameCount;

    public StateMachine(StateMachineImplementor implementor)
    {
      this.m_implementor = implementor;
      this.m_state = this.m_lastState = this.m_nextState = -1;
    }

    public void Update(GameTime gameTime)
    {
      this.InitStateLoop();
      this.m_implementor.UpdateState(gameTime);
      this.m_stateTimeOld = this.m_stateTime;
      this.m_stateTime += gameTime.GetElapsedSeconds();
      ++this.m_stateFrameCount;
      this.InitStateLoop();
    }

    private void InitStateLoop()
    {
      while (this.IsNextStateValid())
      {
        this.m_stateTime = 0.0f;
        this.m_stateTimeOld = 0.0f;
        this.m_stateFrameCount = 0;
        this.m_lastState = this.m_state;
        this.m_state = this.m_nextState;
        this.m_nextState = -1;
        this.m_implementor.ExitState();
        this.m_implementor.InitState();
      }
    }

    public bool IsNextStateValid() => this.m_nextState >= 0;

    public void SetNextStateNow(int state, bool forceReset = false)
    {
      this.SetNextState(state, forceReset);
      this.InitStateLoop();
    }

    public void SetNextState(int state, bool forceReset = false)
    {
      if (!(state != this.m_state | forceReset))
        return;
      this.m_nextState = state;
    }

    public float GetStateTime() => this.m_stateTime;

    public bool CrossedTime(float time)
    {
      return (double) this.m_stateTime >= (double) time && (double) this.m_stateTimeOld < (double) time;
    }

    public bool CrossedRepeatTime(float time, float offset)
    {
      return (int) (((double) this.m_stateTime + (double) offset) / (double) time) != (int) (((double) this.m_stateTimeOld + (double) offset) / (double) time);
    }

    public int GetStateFrameCount() => this.m_stateFrameCount;

    public int GetState() => this.m_state;

    public int GetLastState() => this.m_lastState;

    public int GetNextState() => this.m_nextState;
  }
}
