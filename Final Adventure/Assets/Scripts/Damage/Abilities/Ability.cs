using UnityEngine;
using System.Collections;
using Assets.Scripts.Model;

public abstract class Ability
{

    public string Name { get; set; }
    public float Power { get; set; }
    public float Cost { get; set; }

    public virtual bool Cast(Character attacker, Character defender, int amount)
    {
        return true;
    }
    
}
