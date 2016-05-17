using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Model;

namespace Assets.Scripts.Damage.Abilities
{
    class BloodBlade : Ability
    {
        public BloodBlade()
        {
            Name = "BloodBlade";
            Power = 30f;
            Cost = 8f;
            Base = 230f;
            Desc = "Powerful physical attack that harms the user";
        }

        public override bool Cast(Character attacker, Character defender, int amount)
        {
            if (attacker.Mana - Cost < 0) return false;

            if (defender.Health > 0f)
            {
                attacker.Mana -= (int)Cost;
                defender.Health -= amount;

                float bloodDamage = ((float)amount/100f)*33f;
                attacker.Health -= (int) Math.Round(bloodDamage);
            }

            return true;
        }

    }
}
