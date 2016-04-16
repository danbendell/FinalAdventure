using UnityEngine;
using System.Collections;
using System.Security.Policy;
using Assets.Scripts.Damage;
using Assets.Scripts.Model;
using Flare = Assets.Scripts.Damage.Flare;

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

        float hitChanceBase = GetAttackType(attacker); 
        damage *= CalculateHitChance(defender, attacker, hitChanceBase);

        return Mathf.RoundToInt(damage);
    }

    public int CalculateFocusDamage(Character defender, Character attacker)
    {
        /*
            D = (((2 * LV + 10) / 250) * (SV / DV) * B + 2) * Mod
            D = Damage
            LV = Level
            SV = Strength Value
            DV = Defenece Value
            B = Focus Base Value
            Mod = Modifiers
        */
        Focus focus = new Focus();
        float stageOne = (2f * 1f + 10f) / 250f;
        float stageTwo = (float)attacker.Strength / (float)defender.Defence;
        float stageThree = stageOne * stageTwo * focus.Power + 2f;
        float damage = stageThree * GetModifier(attacker);

        float hitChanceBase = GetAttackType(attacker);
        hitChanceBase *= focus.AccuracyMod;
        damage *= CalculateHitChance(defender, attacker, hitChanceBase);

        return Mathf.RoundToInt(damage);
    }

    public int CalculateHealAmount(Character healer)
    {
        /*
            H = (((2 * LV + 10) / 200) * (MV * MV) + 2) * Mod
            H = Heal Amount
            LV = Level
            MV = Magic Value
            Mod = Modifiers
        */
        Heal heal = new Heal();
        float stageOne = (2f * 1f + 10f) / 200f;
        float stageTwo = (float) healer.Magic * (float) heal.Power;
        float stageThree = stageOne * stageTwo + 2f;
        float healAmount = stageThree * GetModifier(healer);

        return Mathf.RoundToInt(healAmount);
    }

    public int CalculateFlareDamage(Character defender, Character attacker)
    {
        /*
           D = (((2 * LV + 10) / 200) * (MV / RV) * B + 2) * Mod
           D = Damage
           LV = Level
           MV = Magic Value
           RV = Resist Value
           B = Attack Base Value
           Mod = Modifiers
       */
        Flare flare = new Flare();
        float stageOne = (2f * 1f + 10f) / 200f;
        float stageTwo = (float)attacker.Magic / (float)defender.Resist;
        float stageThree = stageOne * stageTwo * flare.Power + 2f;
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

    private int CalculateHitChance(Character defender, Character attacker, float hitChanceBase)
    {
        /*
            Based on the attack type and its accuracy vs evasion.
            HC = HR * (AC / EV)
            HC = Hit Chance
            HR = Hit Rate
            AC = Accuracy
            EV = Evasion

            HC e.g. Melee 95% = 0.95
            Range 85% HR  = 0.85 - Varies on distance, abilities used
            Magic 100% HR = 1
        */
        //This is assuming the attack is Melee 0.95f
        var hitChance = hitChanceBase * ((float) attacker.Accuracy / (float) defender.Evasion);

        //Hit has over 100% chance to hit
        if (hitChance > 1) return 1;

        //e.g. Ran(92) > 80 (0.8 * 100) Miss
        //e.g. Ran(32) > 80 (0.8 * 100) Hit
        if (Random.Range(1, 100) > (hitChance*100)) return 0;

        return 1;
    }

    private float GetAttackType(Character attacker)
    {
        /*
            HC e.g. Melee 95% = 0.95
            Range 85% HR  = 0.85 - Varies on distance, abilities used
            Magic 100% HR = 1
        */
        if (attacker.Job() == CharacterHolder.Jobs.Archer) return 0.85f;
        return 0.95f;
    }
}
