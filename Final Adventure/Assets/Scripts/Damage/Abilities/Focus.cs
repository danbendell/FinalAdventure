using UnityEngine;
using System.Collections;
using Assets.Scripts.Model;

public class Focus : Ability
{

    public float AccuracyMod { get; private set; }

    public Focus()
    {
        Name = "Focus";
        Power = 20f;
        Cost = 5f;
        Base = 250f;
        AccuracyMod = 1.15f;
        Desc = "Powerful ranged attack that increases accuracy";
    }

}
