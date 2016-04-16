using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Damage;
using Assets.Scripts.Model;

public class Worrior : Character {

    public Worrior()
    {
        MaxHealth = 25;
        Health = 25;
        MaxMana = 15;
        Mana = 15;

        Strength = 20;
        Defence = 12;
        Magic = 5;
        Resist = 9;

        Speed = 4;
        Accuracy = 15;
        Evasion = 10;
        Luck = 5;
        CritChance = 5;

        AttackRange = new Vector2(0, 1);

        Abilities = new List<Ability>();
        //Possible things for the future like increase magic or magic resist

        Spells = new List<Spell>();
        //Spells.Add(new Heal());
        //Spells.Add(new Damage.Flare());
        //{ "Heal", "Flare", "Wind", "Aqua", "Earth" };
    }

    public Worrior(int health, int mana, int strength, int defence, int magic, int resist, int speed, int accuracy,
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
        return CharacterHolder.Jobs.Worrior;
    }
}
