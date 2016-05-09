using Assets.Scripts.Model;

namespace Assets.Scripts.Damage
{
    public class Flare : Spell
    {

        public Flare()
        {
            Name = "Flare";
            Power = 25f;
            Cost = 10f;
            Base = 200f;
        }

        public override bool Cast(Character attacker, Character defender, int damage)
        {

            if (attacker.Mana - Cost < 0) return false;

            if (defender.Health > 0)
            {
                attacker.Mana -= (int) Cost;
                defender.Health -= damage;
            }

            if (defender.Health < 0) defender.Health = 0;

            return true;
        }
    }
}
