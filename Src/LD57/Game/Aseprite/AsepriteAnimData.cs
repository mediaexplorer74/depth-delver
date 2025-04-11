// Decompiled with JetBrains decompiler
// Type: LD57.AsepriteAnimData
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace LD57
{
  public class AsepriteAnimData
  {
    public List<AsepriteAnimFrame> frames { get; set; }

    public AsepriteAnimMetadata meta { get; set; }

    public AsepriteAnimTag GetSequence(int sequence) => this.meta.frameTags[sequence];

    public AsepriteAnimFrame GetSequenceFrame(int sequence, int frame)
    {
      return this.frames[this.GetSequence(sequence).from + frame];
    }

    public int GetSequenceFrameCount(int sequence)
    {
      AsepriteAnimTag sequence1 = this.GetSequence(sequence);
      return sequence1.to - sequence1.from + 1;
    }

    public Rectangle GetFrameSrcRect(int sequence, int frame)
    {
      AsepriteAnimFrame sequenceFrame = this.GetSequenceFrame(sequence, frame);
      return new Rectangle(sequenceFrame.frame.x, sequenceFrame.frame.y, sequenceFrame.frame.w, sequenceFrame.frame.h);
    }

    public float GetFrameDuration(int sequence, int frame)
    {
      return this.GetSequenceFrame(sequence, frame).duration / 1000f;
    }

    public int GetSequenceCount() => this.meta.frameTags.Count<AsepriteAnimTag>();

    public int GetSequenceIndex(string sequence)
    {
      for (int sequence1 = 0; sequence1 < this.GetSequenceCount(); ++sequence1)
      {
        if (this.GetSequence(sequence1).name.Equals(sequence))
          return sequence1;
      }
      return -1;
    }
  }
}
