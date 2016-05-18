﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Damage;
using Assets.Scripts.Damage.Magic;
using UnityEngine;

namespace Assets.Scripts.Model.Character_Types
{
    class Priest : Character
    {
        public Priest()
        {
            MaxHealth = 18;
            Health = 1;
            MaxMana = 50;
            Mana = 50;

            Strength = 5;
            Defence = 9;
            Magic = 18;
            Resist = 12;

            Speed = 4;
            Accuracy = 10;
            Evasion = 10;
            Luck = 5;
            CritChance = 5;

            AttackRange = new Vector2(0, 1);


            AttackProbabilityModifier = 0.8f;
            HealProbabilityModifier = 1.6f;

            Spells = new List<Spell>();
            Spells.Add(new Heal());
            Spells.Add(new Wind());

            Abilities = new List<Ability>();

        }

        public Priest(int health, int mana, int strength, int defence, int magic, int resist, int speed, int accuracy,
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
            return CharacterHolder.Jobs.Priest;
        }

        public override string Information()
        {
            return "Faithful Servant\n" +
                   "Defensive caster & Stat booster\n" +
                   "High Mana with low movability";
        }

    }
}
