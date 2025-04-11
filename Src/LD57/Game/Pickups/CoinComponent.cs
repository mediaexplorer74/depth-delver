// Decompiled with JetBrains decompiler
// Type: LD57.Pickups.CoinComponent
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Modded by [M]edia[E]xplorer

using LD57.Spawn;

#nullable disable
namespace LD57.Pickups
{
  public class CoinComponent(Entity parent, LevelState level, SpawnPoint spawnPoint) : 
    PickupComponent(parent, level, spawnPoint, "Coin")
  {
    protected override void OnCollect() => ++this.GetLevel().m_coins;

    protected override void OnCollectEnd() => ++this.GetLevel().m_coinsHUD;
  }
}
