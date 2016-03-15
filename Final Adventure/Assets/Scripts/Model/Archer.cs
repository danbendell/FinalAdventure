namespace Assets.Scripts.Model
{
    public class Archer : Character {

        public Archer()
        {
            MaxHealth = 22;
            Health = 22;
            MaxMana = 20;
            Mana = 20;

            Strength = 10;
            Defence = 6;
            Magic = 5;
            Resist = 8;

            Speed = 4;
            Accuracy = 20;
            Evasion = 15;
            Luck = 5;
            CritChance = 8;

            AttackRange = 5;
        }

        public Archer(int health, int mana, int strength, int defence, int magic, int resist, int speed, int accuracy,
            int evasion, int luck, int critChance, int attackRange)
        {
            Health = health;
            Mana = mana;

            Strength = strength;
            Defence = defence;
            Magic = magic;
            Resist = resist;

            Speed = speed;
            Accuracy = accuracy;
            Evasion = evasion;
            Luck = luck;
            CritChance = critChance;
            AttackRange = attackRange;
        }

        public new void WhoAmI()
        {
            //print("I am an Archer");
        }
    }
}
