// Decompiled with JetBrains decompiler
// Type: LD57.Pickups.HeartComponent
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Modded by [M]edia[E]xplorer

using LD57.Spawn;

#nullable disable
namespace LD57.Pickups
{
  public class HeartComponent(Entity parent, LevelState level, SpawnPoint spawnPoint) : 
    PickupComponent(parent, level, spawnPoint, "Heart")
  {
    protected override void OnCollect() => this.GetLevel().m_player.GetCombat().AddHealth(1);

    protected override void OnCollectEnd() => this.GetLevel().GetHUD().ApplyHeal();
  }
}
