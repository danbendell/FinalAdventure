using System.Collections.Generic;
using Assets.Scripts.Damage;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public class Archer : Character {

        public Archer()
        {
            MaxHealth = 22;
            Health = 2;
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

            AttackRange = new Vector2(2, 5);

            Spells = new List<Spell>();
            Spells.Add(new Heal());

            Abilities = new List<Ability>();
            Abilities.Add(new Focus());

        }

        public Archer(int health, int mana, int strength, int defence, int magic, int resist, int speed, int accuracy,
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
            return CharacterHolder.Jobs.Archer;
        }

    }
}
