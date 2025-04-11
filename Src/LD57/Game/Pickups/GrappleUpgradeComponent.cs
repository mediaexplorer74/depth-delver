// Decompiled with JetBrains decompiler
// Type: LD57.Pickups.GrappleUpgradeComponent
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Modded by [M]edia[E]xplorer

using LD57.Spawn;
using Microsoft.Xna.Framework;

#nullable disable
namespace LD57.Pickups
{
  public class GrappleUpgradeComponent : PickupComponent
  {
    private Vector2 m_startPos;

    public GrappleUpgradeComponent(Entity parent, LevelState level, SpawnPoint spawnPoint)
      : base(parent, level, spawnPoint, "Whip", true)
    {
      this.m_anim.Play("Gold");
      this.m_startPos = this.GetPos();
      this.m_sound = "Checkpoint";
    }

    protected override void OnCollect()
    {
      this.GetLevel().m_canGrapple = true;
      this.GetLevel().m_playerSpawn = this.m_startPos + new Vector2(0.0f, 14f);
    }

    protected override void OnCollectEnd()
    {
      this.GetLevel().m_tutorialTimer = -1f;
      AudioManager.PlaySFX("Upgrade");
    }
  }
}
