using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts.Damage
{
    public class Heal : Spell
    {
        public Heal()
        {
            Name = "Heal";
            Power = 15f;
            Cost = 8f;
        }

        public override bool Cast(Character caster, Character reciever, int amount)
        {
            if (caster.Mana - Cost < 0) return false;
            if (reciever.Health == reciever.MaxHealth) return false;

            if (reciever.Health < reciever.MaxHealth)
            {
                caster.Mana -= (int) Cost;
                reciever.Health += amount;
            }

            if (reciever.Health > reciever.MaxHealth) reciever.Health = reciever.MaxHealth;

            return true;
        }

    }
}
