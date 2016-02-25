using UnityEngine;

namespace Assets.Scripts.Model
{
    public class Wizard : Character
    {
        public Wizard()
        {
            Health = 80;
            Mana = 150;

            Strength = 4;
            Defence = 5;
            Magic = 15;
            Resist = 12;

            Speed = 3;
            Accuracy = 10;
            Evasion = 10;
            Luck = 5;
        }

        public Wizard(int health, int mana, int strength, int defence, int magic, int resist, int speed, int accuracy,
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

        public override void WhoAmI()
        {
            print("I am a Wizard");
        }
    }
}
