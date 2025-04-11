// Decompiled with JetBrains decompiler
// Type: LD57.Spawn.SpawnPoint
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Assembly location: C:\Users\Admin\Desktop\RE\DepthDelver\LD57.dll

using LD57.Camera;
using Microsoft.Xna.Framework;

#nullable disable
namespace LD57.Spawn
{
  public class SpawnPoint
  {
    private SpawnData m_data;
    private Room m_room;
    private bool m_dead;
    private LevelState m_level;
    private Entity m_entity;

    public SpawnPoint(SpawnData data, LevelState level)
    {
      this.m_data = data;
      this.m_room = level.GetRoomManager().GetRoomByPos(data.GetObject().Position + new Vector2(8f, -8f));
      this.m_dead = false;
      this.m_level = level;
      this.m_entity = (Entity) null;
    }

    public void Update()
    {
      if (this.m_entity == null || !this.m_entity.IsDestroyed())
        return;
      this.m_entity = (Entity) null;
    }

    public void CheckSpawn()
    {
      if (this.m_dead || this.m_entity != null && !this.m_entity.IsDestroyed() && !this.m_data.HasProperty("ForceReset") || this.m_level.GetRoomManager().GetCurRoom() != this.m_room)
        return;
      if (this.m_entity != null && this.m_entity.IsDestroyed())
        this.m_entity.Destroy();
      this.m_entity = this.m_level.SpawnObject(this.m_data.GetObject(), this);
    }

    public void SetDead() => this.m_dead = true;

    public Room GetRoom() => this.m_room;

    public SpawnData GetData() => this.m_data;
  }
}
