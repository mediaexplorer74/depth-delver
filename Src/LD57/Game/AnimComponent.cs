
// Type: LD57.AnimComponent
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Modded by [M]edia[E]xplorer

using LD57.Camera;
using LD57.Pickups;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//using MonoGame.Extended;
using System;

#nullable disable
namespace LD57
{
  public class AnimComponent : Component
  {
    private AsepriteAnimData m_animData;
    private string m_filename;
    private int m_sequenceIdx;
    private int m_frameIdx;
    private float m_frameTime;
    private int m_loops;
    private float m_playrate;
    private bool m_paused;
    private SpriteEffects m_effects;
    public float m_depth;
    public bool m_useFacing;
    public bool m_registerCenter;
    public bool m_visible;
    public Vector2 m_localPosition;
    public ShakeRect m_shake;
    public Color m_color;
    private GameCamera m_camera;

    public AnimComponent(Entity parent, string filename, GameCamera camera)
      : base(parent)
    {
      this.m_animData = SpriteManager.GetAnimData(filename);
      this.m_filename = filename;
      this.m_sequenceIdx = -1;
      this.m_frameIdx = 0;
      this.m_frameTime = 0.0f;
      this.m_loops = -1;
      this.m_playrate = 1f;
      this.m_paused = false;
      this.m_effects = SpriteEffects.None;
      this.m_depth = 0.5f;
      this.m_useFacing = true;
      this.m_registerCenter = true;
      this.m_visible = true;
      this.m_localPosition = Vector2.Zero;
      this.m_shake = new ShakeRect();
      this.m_color = Color.White;
      this.m_camera = camera;
    }

    public override void Update(GameTime gameTime)
    {
      if (this.m_paused || this.m_loops == 0 || this.m_sequenceIdx < 0)
        return;
      this.m_frameTime += (float) gameTime.ElapsedGameTime.TotalSeconds * this.m_playrate;
      for (float frameDuration = this.GetFrameDuration(this.m_sequenceIdx, this.m_frameIdx); 
                (double) this.m_frameTime >= (double) frameDuration; 
                frameDuration = this.GetFrameDuration(this.m_sequenceIdx, this.m_frameIdx))
      {
        this.m_frameTime -= frameDuration;
        ++this.m_frameIdx;
        if (this.m_frameIdx >= this.GetCurSequenceFrameCount())
        {
          if (this.m_loops > 0)
          {
            --this.m_loops;
            if (this.m_loops == 0)
            {
              this.m_frameTime = frameDuration;
              --this.m_frameIdx;
              break;
            }
          }
          this.m_frameIdx = 0;
        }
      }
      this.m_shake.Update(gameTime.GetElapsedSeconds());
    }

    public void Play(string sequence, int loops = -1, float playrate = 1f, bool forceReset = false)
    {
      int sequenceIndex = this.m_animData.GetSequenceIndex(sequence);
      if (sequenceIndex < 0)
        throw new Exception("Sequence \"" + sequence + "\" does not exist");
      this.Play(sequenceIndex, loops, playrate, forceReset);
    }

    public void Play(int sequence, int loops = -1, float playrate = 1f, bool forceReset = false)
    {
      if (!(this.m_sequenceIdx != sequence | forceReset))
        return;
      this.m_sequenceIdx = sequence;
      this.m_frameTime = 0.0f;
      this.m_frameIdx = 0;
      this.m_loops = loops;
      this.m_playrate = playrate;
    }

    public void Swap(string sequence)
    {
      int sequenceIndex = this.m_animData.GetSequenceIndex(sequence);
      if (sequenceIndex < 0)
        throw new Exception("Sequence \"" + sequence + "\" does not exist");
      this.Swap(sequenceIndex);
    }

    public void Swap(int sequence) => this.m_sequenceIdx = sequence;

    public void SetLoopsRemaining(int loops) => this.m_loops = loops;

    public void SetPlayrate(float playrate) => this.m_playrate = playrate;

    public void SetPaused(bool paused) => this.m_paused = paused;

    public void AddSpriteEffect(SpriteEffects effect) => this.m_effects |= effect;

    public void ClearSpriteEffect(SpriteEffects effect) => this.m_effects &= ~effect;

    public void SetSpriteEffect(SpriteEffects effect, bool enabled)
    {
      if (enabled)
        this.AddSpriteEffect(effect);
      else
        this.ClearSpriteEffect(effect);
    }

    public bool IsDone() => this.m_loops == 0;

    public float GetCurFrameDuration()
    {
      return this.GetFrameDuration(this.m_sequenceIdx, this.m_frameIdx);
    }

    public float GetCurSequenceFrameDuration(int frame)
    {
      return this.GetFrameDuration(this.m_sequenceIdx, frame);
    }

    public float GetFrameDuration(string sequence, int frame)
    {
      return this.m_animData.GetFrameDuration(this.m_animData.GetSequenceIndex(sequence), frame);
    }

    public float GetFrameDuration(int sequence, int frame)
    {
      return this.m_animData.GetFrameDuration(sequence, frame);
    }

    public int GetCurSequenceFrameCount() => this.GetSequenceFrameCount(this.m_sequenceIdx);

    public int GetSequenceFrameCount(string sequence)
    {
      return this.m_animData.GetSequenceFrameCount(this.m_animData.GetSequenceIndex(sequence));
    }

    public int GetSequenceFrameCount(int sequence)
    {
      return this.m_animData.GetSequenceFrameCount(sequence);
    }

    public int GetFrameIndex() => this.m_frameIdx;

    public float GetCurSequenceDuration() => this.GetSequenceDuration(this.m_sequenceIdx);

    public float GetSequenceDuration(string sequence)
    {
      return this.GetSequenceDuration(this.m_animData.GetSequenceIndex(sequence));
    }

    public float GetSequenceDuration(int sequence)
    {
      float sequenceDuration = 0.0f;
      for (int frame = 0; frame < this.GetSequenceFrameCount(sequence); ++frame)
        sequenceDuration += this.GetFrameDuration(sequence, frame);
      return sequenceDuration;
    }

    public float GetSequenceTime()
    {
      float sequenceTime = 0.0f;
      for (int frame = 0; frame < this.GetSequenceFrameCount(this.m_sequenceIdx); ++frame)
      {
        if (frame == this.m_frameIdx)
          return sequenceTime + this.m_frameTime;
        sequenceTime += this.GetFrameDuration(this.m_sequenceIdx, frame);
      }
      return sequenceTime;
    }

    public AABB GetBound()
    {
      Rectangle frameSrcRect = this.m_animData.GetFrameSrcRect(this.m_sequenceIdx, this.m_frameIdx);
      Vector2 center = this.GetParent().m_position + (Vector2) this.m_shake.m_offset + this.m_localPosition;
      if (!this.m_registerCenter)
        center += new Vector2((float) frameSrcRect.Width, (float) frameSrcRect.Height) * 0.5f;
      return new AABB(center, (float) frameSrcRect.Width / 2f, (float) frameSrcRect.Height / 2f);
    }

    public int GetWidth()
    {
      return this.m_animData.GetFrameSrcRect(this.m_sequenceIdx, this.m_frameIdx).Width;
    }

    public int GetHeight()
    {
      return this.m_animData.GetFrameSrcRect(this.m_sequenceIdx, this.m_frameIdx).Height;
    }

    public Color GetFramePixelColor(int x, int y)
    {
      Rectangle frameSrcRect = this.m_animData.GetFrameSrcRect(this.m_sequenceIdx, this.m_frameIdx);
      return x < 0 || y < 0 || x >= frameSrcRect.Width || y >= frameSrcRect.Height ? new Color(0, 0, 0, 0) : SpriteManager.GetTexturePixelColor(this.m_filename, frameSrcRect.X + x, frameSrcRect.Y + y);
    }

    public override void Draw(GameTime gameTime)
    {
      if (!this.m_visible || this.m_sequenceIdx < 0)
        return;
      Rectangle frameSrcRect = this.m_animData.GetFrameSrcRect(this.m_sequenceIdx, this.m_frameIdx);
      SpriteEffects effects1 = this.m_effects;
      if (this.m_useFacing)
      {
        if ((double) this.GetParent().m_facing.X < 0.0)
          effects1 |= SpriteEffects.FlipHorizontally;
        if ((double) this.GetParent().m_facing.Y > 0.0)
          effects1 |= SpriteEffects.FlipVertically;
      }
      Vector2 vector2 = this.GetParent().m_position + (Vector2) this.m_shake.m_offset + this.m_localPosition;
      if (this.m_registerCenter)
        vector2 -= new Vector2((float) frameSrcRect.Width, (float) frameSrcRect.Height) * 0.5f;
      if (this.m_camera != null)
        vector2 -= this.m_camera.GetPosition();
      string filename = this.m_filename;
      Vector2 position = vector2;
      Rectangle? srcRect = new Rectangle?(frameSrcRect);
      Color? color = new Color?(this.m_color);
      SpriteEffects spriteEffects = effects1;
      float depth = this.m_depth;
      Vector2? origin = new Vector2?();
      Vector2? scale = new Vector2?();
      int effects2 = (int) spriteEffects;
      double layerDepth = (double) depth;
      SpriteManager.DrawTexture(filename, position, srcRect, color, origin: origin, scale: scale, effects: (SpriteEffects) effects2, layerDepth: (float) layerDepth);
    }
  }
}
