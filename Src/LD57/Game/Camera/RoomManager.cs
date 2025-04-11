
// Type: LD57.Camera.RoomManager
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Modded by [M]edia[E]xplorer

using LD57.Tiled;
using Microsoft.Xna.Framework;
//using MonoGame.Extended.Tiled;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace LD57.Camera
{
  public class RoomManager
  {
    private List<Room> m_rooms;
    private LevelState m_level;
    private int m_curRoom;

    public RoomManager(LevelState level)
    {
      this.m_rooms = new List<Room>();
      this.m_level = level;
      this.m_curRoom = -1;
    }

    public void Update()
    {
      if (this.m_level.m_player.IsDead())
        return;
      if (this.m_curRoom < 0 || !this.IsPlayerInRoom(this.m_curRoom))
      {
        for (int index = 0; index < this.m_rooms.Count<Room>(); ++index)
        {
          if (index != this.m_curRoom && this.m_rooms[index].GetArea().Contains(this.m_level.m_player.GetPos()))
          {
            this.m_curRoom = index;
            this.m_level.OnRoomTransition();
            break;
          }
        }
      }
      if (this.m_curRoom >= 0)
        return;
      this.m_curRoom = 0;
    }

    private bool IsPlayerInRoom(int idx)
    {
      return this.m_rooms[idx].GetArea().Contains(this.m_level.m_player.GetPos());
    }

    public Room GetCurRoom() => this.m_rooms[this.m_curRoom];

    public Room GetRoomByPos(Vector2 pos)
    {
      for (int index = 0; index < this.m_rooms.Count<Room>(); ++index)
      {
        Room room = this.m_rooms[index];
        if (room.GetArea().Contains(pos))
          return room;
      }
      return (Room) null;
    }

    public void Reset()
    {
      this.m_curRoom = -1;
      this.Update();
    }

    public void LoadRooms(TiledMap map)
    {
      for (int index1 = 0; index1 < map.ObjectLayers.Count<TiledMapObjectLayer>(); ++index1)
      {
        TiledMapObjectLayer objectLayer = map.ObjectLayers[index1];
        if (objectLayer.Name.Equals("CAMERA"))
        {
          for (int index2 = 0; index2 < ((IEnumerable<TiledMapObject>) objectLayer.Objects).Count<TiledMapObject>(); ++index2)
          {
            TiledMapObject tiledMapObject = objectLayer.Objects[index2];

            if (tiledMapObject.Type.Equals("Room"))
              this.m_rooms.Add(new Room(new AABB(tiledMapObject.Position, 
                  tiledMapObject.Position + 
                  new Vector2(
                      (float)tiledMapObject.Size.Width, 
                      (float)tiledMapObject.Size.Height)
                  )));
          }
        }
      }
    }
  }
}
