// Decompiled with JetBrains decompiler
// Type: LD57.HUDComponent
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Assembly location: C:\Users\Admin\Desktop\RE\DepthDelver\LD57.dll

using LD57.Camera;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace LD57
{
  public class HUDComponent : Component
  {
    private AnimComponent[] m_hearts;
    private AnimComponent m_key;
    private AnimComponent m_coin;
    private AnimComponent[] m_numbers;
    private LevelState m_level;
    private int m_health;
    private bool m_hasKey;
    private int m_coins;

    public HUDComponent(Entity parent, LevelState level)
      : base(parent)
    {
      this.m_level = level;
      this.m_hearts = new AnimComponent[level.m_player.GetCombat().m_maxHealth];
      for (int index = 0; index < ((IEnumerable<AnimComponent>) this.m_hearts).Count<AnimComponent>(); ++index)
      {
        this.m_hearts[index] = new AnimComponent(parent, "HeartHUD", (GameCamera) null);
        this.m_hearts[index].m_localPosition = new Vector2((float) (8 + index * 16), 8f);
        this.m_hearts[index].m_depth = 0.9f;
      }
      this.m_key = new AnimComponent(parent, "KeyHUD", (GameCamera) null);
      this.m_key.m_localPosition = new Vector2(248f, 8f);
      this.m_key.m_depth = 0.9f;
      this.m_coin = new AnimComponent(parent, "CoinHUD", (GameCamera) null);
      this.m_coin.m_localPosition = new Vector2(232f, 8f);
      this.m_coin.m_depth = 0.9f;
      this.m_numbers = new AnimComponent[2];
      for (int index = 0; index < ((IEnumerable<AnimComponent>) this.m_numbers).Count<AnimComponent>(); ++index)
      {
        this.m_numbers[index] = new AnimComponent(parent, "Numbers", (GameCamera) null);
        this.m_numbers[index].m_localPosition = new Vector2((float) (256 - (40 + index * 16)), 8f);
        this.m_numbers[index].m_depth = 0.9f;
      }
      this.Snap();
    }

    public void Snap()
    {
      this.m_level.m_keyHUD = this.m_level.m_key;
      this.m_level.m_coinsHUD = this.m_level.m_coins;
      this.m_health = this.m_level.m_player.GetCombat().m_health;
      for (int index = 0; index < ((IEnumerable<AnimComponent>) this.m_hearts).Count<AnimComponent>(); ++index)
      {
        this.m_hearts[index].Play(this.m_health >= index + 1 ? "Idle" : "Empty");
        if (this.m_health > 0)
          this.m_hearts[index].m_shake.Clear();
        else
          this.m_hearts[index].m_shake.Init(2.5f, -1f);
      }
      this.m_hasKey = this.m_level.m_keyHUD;
      this.m_key.Play(this.m_hasKey ? "Idle" : "Empty");
      this.m_coins = this.m_level.m_coinsHUD;
      this.m_coin.Play("Idle");
      this.UpdateNumbers();
    }

    public void ApplyHeal()
    {
      int health = this.m_level.m_player.GetCombat().m_health;
      for (int index = 0; index < ((IEnumerable<AnimComponent>) this.m_hearts).Count<AnimComponent>(); ++index)
      {
        int num = index + 1;
        if (this.m_health < num && health >= num)
          this.m_hearts[index].Play("Collect", 1, forceReset: true);
      }
      this.m_health = health;
    }

    public void ForceHealth(int health)
    {
      if (this.m_health >= health)
        return;
      this.m_health = health;
    }

    private void UpdateNumbers()
    {
      string str = this.m_coins.ToString();
      for (int index1 = 0; index1 < ((IEnumerable<AnimComponent>) this.m_numbers).Count<AnimComponent>(); ++index1)
      {
        int index2 = str.Length - 1 - index1;
        if (this.m_coins <= 0 || index2 < 0)
          this.m_numbers[index1].Play("0");
        else
          this.m_numbers[index1].Play(str[index2].ToString() ?? "");
      }
    }

    public override void Update(GameTime gameTime)
    {
      int num1 = Math.Min(this.m_level.m_player.GetCombat().m_health, this.m_health);
      bool flag = false;
      for (int index = 0; index < ((IEnumerable<AnimComponent>) this.m_hearts).Count<AnimComponent>(); ++index)
      {
        int num2 = index + 1;
        if (num1 < num2 && this.m_health >= num2)
        {
          this.m_hearts[index].Play("Break", 1, forceReset: true);
          flag = true;
        }
        else if (this.m_hearts[index].IsDone())
          this.m_hearts[index].Play(num1 >= num2 ? "Idle" : "Empty");
        if (flag)
          this.m_hearts[index].m_shake.Init(2.5f, 0.166666672f);
        if (num1 > 0)
        {
          if ((double) this.m_hearts[index].m_shake.m_time < 0.0)
            this.m_hearts[index].m_shake.Clear();
        }
        else
          this.m_hearts[index].m_shake.Init(2.5f, -1f);
      }
      this.m_health = num1;
      bool keyHud = this.m_level.m_keyHUD;
      if (keyHud && !this.m_hasKey)
        this.m_key.Play("Collect", 1, forceReset: true);
      else if (!keyHud && this.m_hasKey)
        this.m_key.Play("Empty");
      else if (this.m_key.IsDone())
        this.m_key.Play(keyHud ? "Idle" : "Empty");
      this.m_hasKey = keyHud;
      int coinsHud = this.m_level.m_coinsHUD;
      if (this.m_coins != coinsHud)
      {
        this.m_coin.Play("Collect", 1, forceReset: true);
        this.m_coins = coinsHud;
      }
      this.UpdateNumbers();
    }

    public override void Draw(GameTime gameTime)
    {
    }
  }
}
