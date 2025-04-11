// Decompiled with JetBrains decompiler
// Type: LD57.SpriteManager
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Assembly location: C:\Users\Admin\Desktop\RE\DepthDelver\LD57.dll

using LD57.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

#nullable disable
namespace LD57
{
  internal static class SpriteManager
  {
    public static ContentManager s_content;
    public static SpriteBatch s_spriteBatch;
    private static Dictionary<string, Texture2D> s_textures = new Dictionary<string, Texture2D>();
    private static Dictionary<string, AsepriteAnimData> s_animData = new Dictionary<string, AsepriteAnimData>();
    public static GameCamera s_camera = (GameCamera) null;

    public static void SetContentManager(ContentManager content)
    {
      SpriteManager.s_content = content;
    }

    public static void SetSpriteBatch(SpriteBatch spriteBatch)
    {
      SpriteManager.s_spriteBatch = spriteBatch;
    }

    public static void LoadTexture(string name, string path = "sprites/")
    {
      SpriteManager.AddTexture(name, SpriteManager.s_content.Load<Texture2D>(path + name));
    }

    public static void AddTexture(string name, Texture2D texture)
    {
      SpriteManager.s_textures.Add(name, texture);
    }

    public static Texture2D GetTexture(string name) => SpriteManager.s_textures[name];

    public static void DrawTexture(
      string name,
      Vector2 position,
      Rectangle? srcRect = null,
      Color? color = null,
      float rotation = 0.0f,
      Vector2? origin = null,
      Vector2? scale = null,
      SpriteEffects effects = SpriteEffects.None,
      float layerDepth = 0.0f)
    {
      position = new Vector2((float) (int) Math.Round((double) position.X, 1), (float) (int) Math.Round((double) position.Y, 1));
      SpriteBatch spriteBatch = SpriteManager.s_spriteBatch;
      Texture2D texture = SpriteManager.s_textures[name];
      Vector2 position1 = position;
      Rectangle? sourceRectangle = srcRect;
      Color color1 = color ?? Color.White;
      double rotation1 = (double) rotation;
      Vector2? nullable = origin;
      Vector2 origin1 = nullable ?? Vector2.Zero;
      nullable = scale;
      Vector2 scale1 = nullable ?? Vector2.One;
      int effects1 = (int) effects;
      double layerDepth1 = (double) layerDepth;
      spriteBatch.Draw(texture, position1, sourceRectangle, color1, (float) rotation1, origin1, scale1, (SpriteEffects) effects1, (float) layerDepth1);
    }

    public static void DrawAABB(AABB aabb, Color color, GameCamera camera, float layerDepth = 0.0f)
    {
      if (camera != null)
        aabb.m_center -= camera.GetPosition();
      Vector2 min = aabb.GetMin();
      Color? nullable1 = new Color?(color);
      Vector2? nullable2 = new Vector2?(aabb.m_extents * 2f);
      float num = layerDepth;
      Rectangle? srcRect = new Rectangle?();
      Color? color1 = nullable1;
      Vector2? origin = new Vector2?();
      Vector2? scale = nullable2;
      double layerDepth1 = (double) num;
      SpriteManager.DrawTexture("pixel", min, srcRect, color1, origin: origin, scale: scale, layerDepth: (float) layerDepth1);
    }

    public static Color GetTexturePixelColor(string name, int x, int y)
    {
      Texture2D texture = SpriteManager.s_textures[name];
      Color[] data = new Color[texture.Width * texture.Height];
      texture.GetData<Color>(0, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), data, 0, texture.Width * texture.Height);
      return data[x + y * texture.Width];
    }

    public static void LoadAnim(string name, string path = "sprites/")
    {
      SpriteManager.LoadTexture(name, path);
      SpriteManager.AddAnimData(name, SpriteManager.s_content.Load<AsepriteAnimData>(path + name + "_data"));
    }

    public static void AddAnimData(string name, AsepriteAnimData data)
    {
      SpriteManager.s_animData.Add(name, data);
    }

    public static AsepriteAnimData GetAnimData(string name) => SpriteManager.s_animData[name];
  }
}
