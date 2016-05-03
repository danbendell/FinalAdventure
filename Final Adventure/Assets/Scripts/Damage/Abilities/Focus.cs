using UnityEngine;
using System.Collections;
using Assets.Scripts.Model;

public class Focus : Ability
{

    public float AccuracyMod { get; private set; }

    public Focus()
    {
        Name = "Focus";
        Power = 35f;
        Cost = 5f;
        AccuracyMod = 1.15f;
    }

    public override bool Cast(Character attacker, Character defender, int amount)
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
