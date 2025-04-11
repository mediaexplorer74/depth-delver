// Decompiled with JetBrains decompiler
// Type: LD57.Spawn.SpawnData
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Assembly location: C:\Users\Admin\Desktop\RE\DepthDelver\LD57.dll

using MonoGame.Extended.Tiled;

#nullable disable
namespace LD57.Spawn
{
  public class SpawnData
  {
    private TiledMapObject m_obj;

    public SpawnData(TiledMapObject obj) => this.m_obj = obj;

    public TiledMapObject GetObject() => this.m_obj;

    public string GetType()
    {
      return !this.m_obj.Type.Equals("") || !(this.m_obj.GetType() == typeof (TiledMapTileObject)) ? this.m_obj.Type : ((TiledMapTileObject) this.m_obj).Tile.Type;
    }

    public bool HasProperty(string prop)
    {
      if (this.m_obj.Properties.ContainsKey(prop))
        return true;
      return this.m_obj.GetType() == typeof (TiledMapTileObject) && ((TiledMapTileObject) this.m_obj).Tile != null && ((TiledMapTileObject) this.m_obj).Tile.Properties.ContainsKey(prop);
    }

    public string GetProperty(string prop, string defaultValue = "")
    {
      if (this.m_obj.Properties.ContainsKey(prop))
        return (string) this.m_obj.Properties[prop];
      return this.m_obj.GetType() == typeof (TiledMapTileObject) && ((TiledMapTileObject) this.m_obj).Tile != null && ((TiledMapTileObject) this.m_obj).Tile.Properties.ContainsKey(prop) ? (string) ((TiledMapTileObject) this.m_obj).Tile.Properties[prop] : defaultValue;
    }

    public float GetPropertyFloat(string prop, float defaultValue = 0.0f)
    {
      return this.HasProperty(prop) ? float.Parse(this.GetProperty(prop)) : defaultValue;
    }
  }
}
