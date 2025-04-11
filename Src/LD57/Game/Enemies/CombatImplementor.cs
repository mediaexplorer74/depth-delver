
// Type: LD57.Enemies.SnakeEnemyComponent
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Modded by [M]edia[E]xplorer

using LD57.Combat;
using System;

namespace LD57.Enemies
{
    public class CombatImplementor
    {
        internal void OnDealHit(DamageDesc damage)
        {
            throw new NotImplementedException();
        }

        internal void OnDeath(DamageDesc damage)
        {
            throw new NotImplementedException();
        }

        internal void OnTakeDamage(DamageDesc damage)
        {
            throw new NotImplementedException();
        }

        internal bool ValidateHit(DamageDesc damage)
        {
            throw new NotImplementedException();
        }

        public static explicit operator CombatImplementor(SnakeEnemyComponent v)
        {
            throw new NotImplementedException();
        }
    }
}