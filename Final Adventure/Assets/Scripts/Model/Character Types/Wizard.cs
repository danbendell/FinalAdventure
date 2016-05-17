using System.Collections.Generic;
using System.Linq.Expressions;
using Assets.Scripts.Damage;
using Assets.Scripts.Damage.Magic;
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

            Strength = 8;
            Defence = 5;
            Magic = 18;
            Resist = 12;

            Speed = 3;
            Accuracy = 11;
            Evasion = 12;
            Luck = 5;
            CritChance = 5;

            AttackRange = new Vector2(0, 1);

            AttackProbabilityModifier = 1f;
            HealProbabilityModifier = 1.5f;
            
            Abilities = new List<Ability>();
            //Possible things for the future like increase magic or magic resist

            Spells = new List<Spell>();
            Spells.Add(new Heal());
            Spells.Add(new Damage.Flare());
            Spells.Add(new Wind());
            //{ "Heal", "Flare", "Wind", "Aqua", "Earth" };
        }

        public Wizard(int health, int mana, int strength, int defence, int magic, int resist, int speed, int accuracy,
            int evasion, int luck, int critChance, Vector2 attackRange, List<Ability> abilities, List<Spell> spells)
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

            Abilities = abilities;
            Spells = spells;
        }

        public override CharacterHolder.Jobs Job()
        {
            return CharacterHolder.Jobs.Wizard;
        }

        public override string Information()
        {
            return "Master of Spells\n" +
                   "Strong offensive and defensive casts\n" +
                   "Weak physical attacks";
        }
    }
}
