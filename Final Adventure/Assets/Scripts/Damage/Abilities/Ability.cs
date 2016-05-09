using UnityEngine;
using System.Collections;
using Assets.Scripts.Model;

public abstract class Ability
{

    public string Name { get; set; }
    public float Power { get; set; }
    public float Cost { get; set; }
    public float Base { get; set; }

    public virtual bool Cast(Character attacker, Character defender, int amount)
    {
        if (attacker.Mana - Cost < 0) return false;

        if (defender.Health > 0f)
        {
            attacker.Mana -= (int)Cost;
            defender.Health -= amount;
        }

        return true;
    }
    
}
