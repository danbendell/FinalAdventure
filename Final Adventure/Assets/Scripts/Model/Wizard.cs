using UnityEngine;

namespace Assets.Scripts.Model
{
    public class Wizard : Character
    {
        public Wizard()
        {
            MaxHealth = 20;
            Health = 20;
            MaxMana = 50;
            Mana = 50;

            Strength = 4;
            Defence = 5;
            Magic = 15;
            Resist = 12;

            Speed = 3;
            Accuracy = 10;
            Evasion = 10;
            Luck = 5;
            CritChance = 5;

            AttackRange = 1;
        }

        public Wizard(int health, int mana, int strength, int defence, int magic, int resist, int speed, int accuracy,
            int evasion, int luck, int critChance, int attackRange)
        {
            MaxHealth = health;
            Health = health;
            MaxMana = mana;
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

        public override void WhoAmI()
        {
            //print("I am a Wizard");
        }
    }
}
