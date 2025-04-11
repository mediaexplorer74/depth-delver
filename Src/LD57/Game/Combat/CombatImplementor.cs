
// Type: LD57.Combat.CombatImplementor
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Modded by [M]edia[E]xplorer

#nullable disable
namespace LD57.Combat
{
  public interface CombatImplementor
  {
        bool ValidateHit(DamageDesc damage);// => true;

        void OnTakeDamage(DamageDesc damage); //{}

        void OnDealHit(DamageDesc damage);//{}

        void OnDeath(DamageDesc damage); //{}    
  }
}
