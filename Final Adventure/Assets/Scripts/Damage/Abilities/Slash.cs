using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Model;

namespace Assets.Scripts.Damage.Abilities
{
    class Slash : Ability
    {
        public float AccuracyMod { get; private set; }

        public Slash()
        {
            Name = "Slash";
            Power = 38f;
            Cost = 8;
            AccuracyMod = 0.7f;
        }

        public override bool Cast(Character attacker, Character defender, int amount)
        {
            if (attacker.Mana - Cost < 0) return false;

            if (defender.Health > 0f)
            {
                attacker.Mana -= (int)Cost;
                defender.Health -= amount;
            }

            return true;
        }
    }
}
