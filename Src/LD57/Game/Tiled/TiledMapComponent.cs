
// Type: LD57.Tiled.TiledMapComponent
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Modded by [M]edia[E]xplorer

using LD57.Camera;
using LD57.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace LD57.Tiled
{
  public class TiledMapComponent : Component
  {
    public static readonly string[] kCollisionTypeNames = new string[3]
    {
      "None",
      "Solid",
      "SemiSolid"
    };
    private TiledMap m_tilemap;
    private List<float> m_depth;
    private List<float> m_alpha;
    private List<List<PhysicsComponent.CollideMask>> m_collision;
    private GameCamera m_camera;

    public TiledMapComponent(Entity parent, string mapname, string path = "maps/")
      : base(parent)
    {
      this.m_tilemap = SpriteManager.s_content.Load<TiledMap>(path + mapname);
      this.m_depth = new List<float>();
      this.m_alpha = new List<float>();
      this.m_collision = new List<List<PhysicsComponent.CollideMask>>();
      for (int index1 = 0; index1 < this.m_tilemap.TileLayers.Count; ++index1)
      {
        TiledMapTileLayer tileLayer = this.m_tilemap.TileLayers[index1];
        this.m_depth.Add(0.1f);
        if (tileLayer.Properties.ContainsKey("depth"))
          this.m_depth[index1] = MathHelper.Clamp(float.Parse(tileLayer.Properties["depth"].Value), 0.0f, 1f);
        this.m_alpha.Add(1f);
        this.m_collision.Add(new List<PhysicsComponent.CollideMask>());
        for (int index2 = 0; index2 < ((IEnumerable<TiledMapTile>) tileLayer.Tiles).Count<TiledMapTile>(); ++index2)
        {
          this.m_collision[index1].Add(PhysicsComponent.CollideMask.None);
          TiledMapTile tile = tileLayer.Tiles[index2];
          TiledMapTileset tilesetByIdentifyer = this.GetTilesetByIdentifyer(tile.GlobalIdentifier);
          int index3 = tile.GlobalIdentifier - this.m_tilemap.GetTilesetFirstGlobalIdentifier(tilesetByIdentifyer);
          if (index3 >= 0 && index3 < tilesetByIdentifyer.Tiles.Count)
          {
            string type = tilesetByIdentifyer.Tiles[index3].Type;
            for (int index4 = 0; index4 < TiledMapComponent.kCollisionTypeNames.Length; ++index4)
            {
              if (type.Equals(TiledMapComponent.kCollisionTypeNames[index4]))
                this.m_collision[index1][index2] = (PhysicsComponent.CollideMask) index4;
            }
          }
        }
      }
      this.m_camera = (GameCamera) null;
    }

    public void SetCamera(GameCamera camera) => this.m_camera = camera;

    public override void Update(GameTime gameTime)
    {
    }

    public TiledMap GetTiledMap() => this.m_tilemap;

    private TiledMapTileset GetTilesetByIdentifyer(int identifyer)
    {
      for (int index = this.m_tilemap.Tilesets.Count - 1; index > 0; --index)
      {
        if (identifyer >= this.m_tilemap.GetTilesetFirstGlobalIdentifier(this.m_tilemap.Tilesets[index]))
          return this.m_tilemap.Tilesets[index];
      }
      return this.m_tilemap.Tilesets[0];
    }

    public PhysicsComponent.CollideMask GetCollisionType(int layerIndex, int x, int y)
    {
      return x < 0 || x >= this.m_tilemap.Width || y < 0 || y >= this.m_tilemap.Height ? PhysicsComponent.CollideMask.None : this.m_collision[layerIndex][x + y * this.m_tilemap.Width];
    }

    public float RoundX(float x) => (float) (this.GetXIdx(x) * this.m_tilemap.TileWidth);

    public float RoundY(float y) => (float) (this.GetYIdx(y) * this.m_tilemap.TileHeight);

    public int GetXIdx(float x) => (int) ((double) x / (double) this.m_tilemap.TileWidth);

    public int GetYIdx(float y) => (int) ((double) y / (double) this.m_tilemap.TileHeight);

    public bool CheckCollision(
      AABB m_aabb,
      PhysicsComponent.CollideMask mask,
      List<AABB> hit = null,
      AABB? exclude = null)
    {
      Vector2 min = m_aabb.GetMin();
      Vector2 max = m_aabb.GetMax();
      for (int xidx = this.GetXIdx(min.X); (double) (xidx * this.m_tilemap.TileWidth) < (double) max.X; ++xidx)
      {
        if (xidx >= 0)
        {
          if (xidx < this.m_tilemap.Width)
          {
            for (int yidx = this.GetYIdx(min.Y); (double) (yidx * this.m_tilemap.TileHeight) < (double) max.Y; ++yidx)
            {
              if (yidx >= 0)
              {
                if (yidx < this.m_tilemap.Height)
                {
                  for (int index = 0; index < this.m_tilemap.TileLayers.Count; ++index)
                  {
                    if ((this.m_collision[index][xidx + yidx * this.m_tilemap.Width] & mask) != PhysicsComponent.CollideMask.None)
                    {
                      if (hit == null && !exclude.HasValue)
                        return true;
                      AABB aabb = new AABB(new Vector2(((float) xidx + 0.5f) * (float) this.m_tilemap.TileWidth, ((float) yidx + 0.5f) * (float) this.m_tilemap.TileHeight), (float) this.m_tilemap.TileWidth * 0.5f, (float) this.m_tilemap.TileHeight * 0.5f);
                      if (!exclude.HasValue || !aabb.Intersects(exclude.Value))
                        hit?.Add(aabb);
                      else if (hit == null)
                        return true;
                    }
                  }
                }
                else
                  break;
              }
            }
          }
          else
            break;
        }
      }
      return hit != null && hit.Count != 0;
    }

    public void SetLayerAlpha(int index, float alpha) => this.m_alpha[index] = alpha;

    public override void Draw(GameTime gameTime)
    {
      int tileWidth = this.m_tilemap.TileWidth;
      int tileHeight = this.m_tilemap.TileHeight;
      Vector2 vector2 = this.m_camera != null ? -this.m_camera.GetPosition() : Vector2.Zero;
      int num1 = Math.Max(0, (int) (-(double) vector2.X / (double) this.m_tilemap.TileWidth));
      int num2 = Math.Min(this.m_tilemap.Width, (int) ((256.0 - (double) vector2.X) / (double) this.m_tilemap.TileWidth) + 1);
      int num3 = Math.Max(0, (int) (-(double) vector2.Y / (double) this.m_tilemap.TileHeight));
      int num4 = Math.Min(this.m_tilemap.Height, (int) ((144.0 - (double) vector2.Y) / (double) this.m_tilemap.TileHeight) + 1);
      for (int index = 0; index < this.m_tilemap.TileLayers.Count; ++index)
      {
        if ((double) this.m_alpha[index] > 0.0)
        {
          TiledMapTileLayer tileLayer = this.m_tilemap.TileLayers[index];
          for (int x1 = num1; x1 < num2; ++x1)
          {
            for (int y1 = num3; y1 < num4; ++y1)
            {
              TiledMapTile tile = tileLayer.GetTile((ushort) x1, (ushort) y1);
              TiledMapTileset tilesetByIdentifyer = this.GetTilesetByIdentifyer(tile.GlobalIdentifier);
              int num5 = tile.GlobalIdentifier - this.m_tilemap.GetTilesetFirstGlobalIdentifier(tilesetByIdentifyer);
              if (num5 >= 0)
              {
                int x2 = num5 % tilesetByIdentifyer.Columns * this.m_tilemap.TileWidth;
                int y2 = num5 / tilesetByIdentifyer.Columns * this.m_tilemap.TileHeight;
                Vector2 position = vector2 + this.m_tilemap.TileLayers[index].Offset + new Vector2((float) (x1 * tileWidth), (float) (y1 * tileHeight)) + this.GetParent().m_position;
                position = new Vector2((float) (int) Math.Round((double) position.X, 1), (float) (int) Math.Round((double) position.Y, 1));
                Color color = new Color(this.m_alpha[index], this.m_alpha[index], this.m_alpha[index], this.m_alpha[index]);
                SpriteManager.s_spriteBatch.Draw(tilesetByIdentifyer.Texture, position, new Rectangle?(new Rectangle(x2, y2, tileWidth, tileHeight)), color, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, this.m_depth[index]);
              }
            }
          }
        }
      }
    }
  }
}
