namespace Assets.Scripts.Model
{
    public class Archer : Character {

        public Archer()
        {
            Health = 80;
            Mana = 80;

            Strength = 10;
            Defence = 6;
            Magic = 5;
            Resist = 8;

            Speed = 4;
            Accuracy = 20;
            Evasion = 15;
            Luck = 5;
        }

        public Archer(int health, int mana, int strength, int defence, int magic, int resist, int speed, int accuracy,
            int evasion, int luck)
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
        }

        public new void WhoAmI()
        {
            print("I am an Archer");
        }
    }
}
