using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Damage;
using Assets.Scripts.Damage.Abilities;
using UnityEngine;

namespace Assets.Scripts.Model.Character_Types
{
    class Assassin : Character
    {

        public Assassin()
        {
            MaxHealth = 16;
            Health = 16;
            MaxMana = 20;
            Mana = 20;

            Strength = 15;
            Defence = 7;
            Magic = 5;
            Resist = 12;

            Speed = 4;
            Accuracy = 16;
            Evasion = 18;
            Luck = 5;
            CritChance = 12;

            AttackRange = new Vector2(0, 1);

            Spells = new List<Spell>();
            Spells.Add(new Heal());

            Abilities = new List<Ability>();
            Abilities.Add(new Assassinate());
            Abilities.Add(new BloodBlade());


        }

    public Assassin(int health, int mana, int strength, int defence, int magic, int resist, int speed, int accuracy,
            int evasion, int luck, int critChance, Vector2 attackRange, List<Ability> abilities, List<Spell> spells)
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

            Abilities = abilities;
            Spells = spells;

        }

        public override CharacterHolder.Jobs Job()
        {
            return CharacterHolder.Jobs.Assassin;
        }

        public override string Information()
        {
            return "Silent Slayer\n" +
                   "Average overall stats\n" +
                   "High damage but harmful abilities";
        }
    }
}
