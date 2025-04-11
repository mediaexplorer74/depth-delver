
// Type: LD57.Combat.CombatManager
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace LD57.Combat
{
  public class CombatManager
  {
    private List<CombatComponent> m_combatComponents;
    private bool m_debugDraw;

    public CombatManager() => this.m_combatComponents = new List<CombatComponent>();

    public void AddCombat(CombatComponent combat) => this.m_combatComponents.Add(combat);

    public void Update(GameTime gameTime)
    {
      double totalSeconds = gameTime.ElapsedGameTime.TotalSeconds;
      for (int index1 = 0; index1 < this.m_combatComponents.Count; ++index1)
      {
        if (this.m_combatComponents[index1].GetParent().IsDestroyed())
        {
          this.m_combatComponents.RemoveAt(index1);
          --index1;
        }
        else
        {
          CombatComponent combatComponent1 = this.m_combatComponents[index1];
          combatComponent1.Update(gameTime);
          if (combatComponent1.m_defenseEnabled)
          {
            foreach (CombatComponent combatComponent2 in this.m_combatComponents)
            {
              if 
              ( combatComponent2 != combatComponent1 && !combatComponent1.GetParent().IsDestroyed() 
                && combatComponent2.m_attackEnabled && (combatComponent2.m_attackMask
                & combatComponent1.m_defenseMask) != (CombatComponent.CombatMask) 0
                && !combatComponent1.IsTimeout(combatComponent2.m_damage.m_guid) 
              )
              {
                bool flag = false;
                for (int index2 = 0; index2 < combatComponent2.m_attackBoxes.Count<AABB>(); ++index2)
                {
                  AABB attackBox = combatComponent2.m_attackBoxes[index2];
                  attackBox.m_center += combatComponent2.GetParent().m_position;
                  for (int index3 = 0; index3 < combatComponent1.m_defenseBoxes.Count<AABB>(); ++index3)
                  {
                    AABB defenseBox = combatComponent1.m_defenseBoxes[index3];
                    defenseBox.m_center += combatComponent1.GetParent().m_position;
                    if (attackBox.Intersects(defenseBox))
                    {
                      flag = true;
                      break;
                    }
                  }
                  if (flag)
                    break;
                }
                if (flag)
                {
                  DamageDesc damage = combatComponent2.m_damage with
                  {
                    m_defender = combatComponent1
                  };
                  if (combatComponent1.ValidateHit(damage))
                  {
                    combatComponent1.TakeDamage(damage);
                    combatComponent2.DealHit(damage);
                  }
                }
              }
            }
          }
        }
      }
    }

    public void Draw()
    {
      int num = this.m_debugDraw ? 1 : 0;
    }
  }
}
