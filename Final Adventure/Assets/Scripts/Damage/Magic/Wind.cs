using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Model;

namespace Assets.Scripts.Damage.Magic
{
    class Wind : Spell
    {
        public Wind()
        {
            Name = "Wind";
            Power = 25f;
            Cost = 10f;
            Base = 200f;
            Desc = "Powerful air Spell";
        }

        public override bool Cast(Character attacker, Character defender, int damage)
        {

            if (attacker.Mana - Cost < 0) return false;

            if (defender.Health > 0)
            {
                attacker.Mana -= (int)Cost;
                defender.Health -= damage;
            }

            if (defender.Health < 0) defender.Health = 0;

            return true;
        }
    }
}
