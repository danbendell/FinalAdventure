using UnityEngine;
using System.Collections;
using Assets.Scripts.Model;

public class DamageUtil {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public int CalculatePhysicalDamage(Character defender, Character attacker)
    {
        /*
            D = (((2 * LV + 10) / 250) * (SV / DV) * B + 2) * Mod
            D = Damage
            LV = Level
            SV = Strength Value
            DV = Defenece Value
            B = Attack Base Value
            Mod = Modifiers
        */
        float stageOne = (2f * 1f + 10f) / 250f;
        float stageTwo = (float) attacker.Strength / (float) defender.Defence;
        float stageThree = stageOne * stageTwo * attacker.Strength + 2f;
        float damage = stageThree * GetModifier(attacker);

        return Mathf.RoundToInt(damage);
    }

    private float GetModifier(Character attacker)
    {
        //Here we can add weaknesses, critical chance, STAB and other statical changes.
        //STAB is 1.5 where e.g. mage uses magic, archer uses a range attack, worrier uses melee attack. Attack is in their role
        //STAB is 1 where the attack is not in their favor e.g. mage uses melee, worrier uses magic
        float min = 0.85f;
        float max = 1f;

        /*
            Applying a crit chance (x/100)
            R = Random.Range(0, 100);
            if(R <= CritChance) 2
            e.g. 3/100 <= CritChance(5) = 2
            e.g. 67/100 <= CritChance(5) = 1
        */
        float critValue = 1f;
        if (Random.Range(0f, 100f) <= attacker.CritChance) critValue = 2;
        min *= critValue;
        max *= critValue;

        /*
            Applying Stab
            ... Unsure at the moment
            1.5 or 1
        */
        float stab = 1f;
        min *= stab;
        max *= stab;

        return Random.Range(min, max);
    }
}
