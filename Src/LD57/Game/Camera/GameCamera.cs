
// Type: LD57.Camera.GameCamera
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Modded by [M]edia[E]xplorer

using LD57.Objects;
using LD57.Pickups;
using Microsoft.Xna.Framework;
//using MonoGame.Extended;
using System;

#nullable disable
namespace LD57.Camera
{
  public class GameCamera
  {
    private Vector2 m_position = Vector2.Zero;
    private Vector2 m_target = Vector2.Zero;
    private GameObjectComponent m_tracking;
    private float m_lerpFactor = 4f;
    private float m_minVel = 300f;
    private AABB m_roomBound;
    private ShakeRect m_shake = new ShakeRect();

    public void Update(GameTime gameTime)
    {
      this.UpdateTarget();
      Vector2 clampedTargetPos = this.GetClampedTargetPos();
      if (clampedTargetPos != this.m_position)
      {
        Vector2 vector2_1 = clampedTargetPos - this.m_position;
        float y = this.m_tracking == null ? this.m_lerpFactor : MathHelper.Lerp(this.m_lerpFactor, this.m_lerpFactor * 5f, 
            MathHelper.Clamp((float) (((double) this.m_tracking.GetPhysics().m_velocity.Y - 300.0) / 300.0), 0.0f, 1f));
        Vector2 vector2_2 = vector2_1 * new Vector2(this.m_lerpFactor, y);
        if ((double) vector2_2.LengthSquared() < (double) this.m_minVel * (double) this.m_minVel)
        {
          vector2_2.Normalize();
          vector2_2 *= this.m_minVel;
        }
        Vector2 vector2_3 = vector2_2 * gameTime.GetElapsedSeconds();
        if ((double) vector2_3.LengthSquared() >= (double) vector2_1.LengthSquared())
          this.m_position = clampedTargetPos;
        else
          this.m_position += vector2_3;
      }
      this.m_shake.Update(gameTime.GetElapsedSeconds());
    }

    private void UpdateTarget()
    {
      if (this.m_tracking == null)
        return;
      this.m_target = this.m_tracking.GetPos();
    }

    public void SetTracking(GameObjectComponent tracking) => this.m_tracking = tracking;

    public void SnapToTarget()
    {
      this.UpdateTarget();
      this.m_position = this.GetClampedTargetPos();
    }

    public Vector2 GetPosition() => this.m_position + (Vector2) this.m_shake.m_offset;

    private Vector2 GetClampedTargetPos()
    {
      AABB roomBound = this.m_roomBound;
      roomBound.m_extents.X = Math.Max(roomBound.m_extents.X - 128f, 0.0f);
      roomBound.m_extents.Y = Math.Max(roomBound.m_extents.Y - 72f, 0.0f);
      return this.PositionFromTarget(roomBound.GetClosestPoint(this.m_target));
    }

    private Vector2 PositionFromTarget(Vector2 target) => target - new Vector2(128f, 72f);

    public void SetRoomBound(AABB aabb) => this.m_roomBound = aabb;

    public AABB GetViewBound()
    {
      return new AABB(this.m_position, this.m_position + new Vector2(256f, 144f));
    }

    public void StartShake(float amount, float time) => this.m_shake.Init(amount, time);
  }
}
