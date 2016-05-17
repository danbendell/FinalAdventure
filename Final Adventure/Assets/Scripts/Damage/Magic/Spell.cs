using UnityEngine;
using System.Collections;
using Assets.Scripts.Model;

public class Spell  {


    public string Name { get; set; }
    public float Power { get; set; }
    public float Cost { get; set; }
    public float Base { get; set; }
    public string Desc { get; set; }

    public virtual bool Cast(Character caster, Character defender, int amount)
    {
        return true;
    }
}
