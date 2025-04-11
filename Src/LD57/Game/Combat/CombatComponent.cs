
// Type: LD57.Combat.CombatComponent
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Modded by [M]edia[E]xplorer

using LD57.Breakables;
using LD57.Enemies;
using LD57.Objects;
using LD57.Pickups; // =)
using Microsoft.Xna.Framework;
//using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace LD57.Combat
{
  public class CombatComponent : Component
  {
    private CombatManager m_manager;
    public bool m_debugDraw;
    public bool m_attackEnabled = true;
    public bool m_defenseEnabled = true;
    public List<AABB> m_attackBoxes;
    public List<AABB> m_defenseBoxes;
    private List<DamageTimeout> m_timeouts;
    private CombatImplementor m_implementor;
    public CombatComponent.CombatMask m_attackMask;
    public CombatComponent.CombatMask m_defenseMask;
    public DamageDesc m_damage = new DamageDesc();
    public int m_health = 1;
    public int m_maxHealth = 1;
        private CombatManager combatManager;
        private SnakeEnemyComponent snakeEnemyComponent;
        private PlayerComponent playerComponent;
        private BatEnemyComponent batEnemyComponent;
        private BreakableComponent breakableComponent;

        public CombatComponent(Entity parent, CombatManager manager, CombatImplementor implementor = null)
      : base(parent)
    {
      this.m_manager = manager;
      this.m_manager.AddCombat(this);
      this.m_implementor = implementor;
      this.m_damage.m_owner = this;
      this.m_damage.m_origOwner = this;
      this.m_attackBoxes = new List<AABB>();
      this.m_defenseBoxes = new List<AABB>();
      this.m_timeouts = new List<DamageTimeout>();
    }

        public CombatComponent(Entity parent, CombatManager combatManager, SnakeEnemyComponent snakeEnemyComponent) : base(parent)
        {
            this.combatManager = combatManager;
            this.snakeEnemyComponent = snakeEnemyComponent;
        }

        public CombatComponent(Entity parent, CombatManager combatManager, PlayerComponent playerComponent) : base(parent)
        {
            this.combatManager = combatManager;
            this.playerComponent = playerComponent;
        }

        public CombatComponent(Entity parent, CombatManager combatManager, BatEnemyComponent batEnemyComponent) : base(parent)
        {
            this.combatManager = combatManager;
            this.batEnemyComponent = batEnemyComponent;
        }

        public CombatComponent(Entity parent, CombatManager combatManager, BreakableComponent breakableComponent) : base(parent)
        {
            this.combatManager = combatManager;
            this.breakableComponent = breakableComponent;
        }

        public CombatImplementor GetImplementor()
        {
            return this.m_implementor;
        }

        public bool ValidateHit(DamageDesc damage)
    {
      return this.m_implementor == null || this.m_implementor.ValidateHit(damage);
    }

    public void TakeDamage(DamageDesc damage)
    {
      this.m_timeouts.Add(new DamageTimeout(damage.m_timeout, damage.m_guid));
      this.m_health -= damage.m_damage;
      if (this.m_implementor == null)
        return;
      if (this.m_health <= 0)
        this.m_implementor.OnDeath(damage);
      else
        this.m_implementor.OnTakeDamage(damage);
    }

    public void DealHit(DamageDesc damage)
    {
      if (this.m_implementor == null)
        return;
      this.m_implementor.OnDealHit(damage);
    }

    public override void Update(GameTime gameTime)
    {
      for (int index = 0; index < this.m_timeouts.Count; ++index)
      {
        if ((double) this.m_timeouts[index].GetTime() <= 0.0)
        {
          this.m_timeouts.RemoveAt(index);
          --index;
        }
        else
          this.m_timeouts[index].Update(gameTime.GetElapsedSeconds());
      }
    }

    public bool IsTimeout(int guid)
    {
      foreach (DamageTimeout timeout in this.m_timeouts)
      {
        if (timeout.GetGuid() == guid)
          return true;
      }
      return false;
    }

    public void AddHealth(int health)
    {
      this.m_health = Math.Min(this.m_health + health, this.m_maxHealth);
    }

    public override void Draw(GameTime gameTime)
    {
      if (!this.m_debugDraw)
        return;
      Color red = Color.Red;
      Color blue = Color.Blue;
      red.A = (byte) 100;
      blue.A = (byte) 100;
      if (this.m_attackEnabled)
      {
        for (int index = 0; index < this.m_attackBoxes.Count<AABB>(); ++index)
        {
          AABB attackBox = this.m_attackBoxes[index];
          attackBox.m_center += this.GetParent().m_position;
          SpriteManager.DrawAABB(attackBox, red, SpriteManager.s_camera, 1f);
        }
      }
      if (!this.m_defenseEnabled)
        return;
      for (int index = 0; index < this.m_defenseBoxes.Count<AABB>(); ++index)
      {
        AABB defenseBox = this.m_defenseBoxes[index];
        defenseBox.m_center += this.GetParent().m_position;
        SpriteManager.DrawAABB(defenseBox, blue, SpriteManager.s_camera, 1f);
      }
    }

    public enum CombatMask
    {
      Player = 1,
      Enemy = 4,
      Object = 8,
    }
  }
}
