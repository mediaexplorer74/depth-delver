
// Type: LD57.Pickups.KeyComponent
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Assembly location: C:\Users\Admin\Desktop\RE\DepthDelver\LD57.dll

using LD57.Spawn;

#nullable disable
namespace LD57.Pickups
{
  public class KeyComponent : PickupComponent
  {
    public KeyComponent(Entity parent, LevelState level, SpawnPoint spawnPoint)
      : base(parent, level, spawnPoint, "Key")
    {
      this.m_sound = "Checkpoint";
    }

    protected override bool CanCollect() => !this.GetLevel().m_key;

    protected override void OnCollect() => this.GetLevel().m_key = true;

    protected override void OnCollectEnd() => this.GetLevel().m_keyHUD = true;
  }
}
