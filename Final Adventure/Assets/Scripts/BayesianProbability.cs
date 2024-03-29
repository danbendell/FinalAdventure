﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

public class BayesianProbability
{

    private CharacterHolder _AI;
    private List<CharacterHolder> _opposition = new List<CharacterHolder>();
    private List<CharacterHolder> _aiCharacterHolders = new List<CharacterHolder>();
    private List<Probabilities> _characterProbabilities = new List<Probabilities>();

    public List<Probabilities> CharacterProbabilities()
    {
        return _characterProbabilities;
    } 

    public BayesianProbability(CharacterHolder AI)
    {
        _AI = AI;
        CreateProbabilitys();
    }

    private void CreateProbabilitys()
    {
        _opposition = OppositionList();
        _aiCharacterHolders = AiList();

        foreach (var characterHolder in _opposition)
        {
            Probabilities cp = new Probabilities(characterHolder.Job)
            {
                Attack = ProbabilityOfAttackGivenMovementNew(characterHolder),
                Move = ProbabilityOfMovement(characterHolder),
                Heal = ProbabilityOfHealNew(characterHolder)
            };
            characterHolder.Probabilities = cp;
            _characterProbabilities.Add(cp);
        }
    }

    private float ProbabilityOfAttackGivenMovementNew(CharacterHolder characterHolder)
    {
        float AINearCharacter = CalcNumberOfAIAround(characterHolder);
        float allyNearCharacter = CalcNumberOfAlliesAround(characterHolder);

        float AIPercentHP = CalcPercentHP(_AI);
        float TargetPercentHP = CalcPercentHP(characterHolder);

        float strength = characterHolder.Character.Strength;
        float defence = _AI.Character.Defence;

        float mod = characterHolder.Character.AttackProbabilityModifier;

        float allies = allyNearCharacter / AINearCharacter;
        float health = TargetPercentHP / AIPercentHP;
        float stats = strength / defence;

        float result = (allies * (health * stats)) * mod;
        result = Mathf.Round(result * 100f) / 100f;
        if (result > 1f) result = 1f;

        if (!InAttackRange(_AI, characterHolder)) result = 0f;
        return result;
    }

    private float ProbabilityOfAttackGivenNoMovement(CharacterHolder characterHolder)
    {
        float AINearCharacter = CalcNumberOfAIAround(characterHolder);
        float allyNearCharacter = CalcNumberOfAlliesAround(characterHolder);

        float AIPercentHP = CalcPercentHP(_AI);
        float TargetPercentHP = CalcPercentHP(characterHolder);

        float strength = characterHolder.Character.Strength;
        float defence = _AI.Character.Defence;

        float mod = characterHolder.Character.AttackProbabilityModifier;

        float allies = allyNearCharacter / AINearCharacter;
        float health = TargetPercentHP / AIPercentHP;
        float stats = strength / defence;

        float result = (allies * (health * stats)) * mod;
        result = Mathf.Round(result * 100f) / 100f;
        if (result > 1f) result = 1f;

        var distanceFromAI = CalcDistance(_AI, characterHolder);
        var attackRange = characterHolder.Character.AttackRange.y;
        if (attackRange > distanceFromAI) result = 0f;
        return result;
    }

    private float ProbabilityOfMovement(CharacterHolder characterHolder)
    {
        float movementProbability = 0.5f;
        float AINearCharacter = CalcNumberOfAIAround(characterHolder);
        var characterPercentHP = CalcPercentHP(characterHolder);

        //More that one AI are in range of attacking
        if (AINearCharacter > 1f)
        {
            //Current health is high, no need retreat
            if (characterPercentHP > 0.75f)
            {
                //Using a higher grade than one to allow for a 10percetile random choice
                // 1.1 - 0.8 = 0.3 % 
                movementProbability = 1.1f - characterPercentHP;
            }
            if (characterPercentHP < 0.4f)
            {
                // 1.1 - 0.3 = 0.8 % 
                movementProbability = 1.1f - characterPercentHP;
            }
        }
        else
        {
            movementProbability = 0.5f;
        }
        if (movementProbability > 1) movementProbability = 0.9f;

        return movementProbability;
    }

    private float ProbabilityOfHealNew(CharacterHolder characterHolder)
    {
        float healProbability = 0f;
        List<float> surroundingAIHealth = new List<float>();
        List<float> surroundingAllyHealth = new List<float>();
        List<float> damageTakenPercent = new List<float>();

        foreach (var allies in _opposition)
        {
            //Ally is too far away to be considered
            if (!InAttackRange(characterHolder, allies)) continue;

            //The amount of damage taken 0f - 1f
            float damageTaken = 1f - CalcPercentHP(allies);
            damageTakenPercent.Add(ToTwoDecimalPlaces(damageTaken));

            //The amount of health remaining
            float HPRemaining = CalcPercentHP(allies);
            surroundingAllyHealth.Add(ToTwoDecimalPlaces(HPRemaining));
        }

        //Loops through the damage taken for each ally and calculates the chance of the Ai healing them
        healProbability = CalcHealChanceBasedOnDamageTaken(characterHolder, damageTakenPercent);
        
        //Get the health of all the surrounding AI
        foreach (var ai in _aiCharacterHolders)
        {
            if (!InAttackRange(characterHolder, ai)) continue;

            float damageTaken = CalcPercentHP(ai);
            surroundingAIHealth.Add(ToTwoDecimalPlaces(damageTaken));
        }
        //Divide the AI over Ally
        //This will determine which team has the current advantage
        float outNumberMod = surroundingAIHealth.Sum() / surroundingAllyHealth.Sum();

        //Only want to apply the modifier if the allies are at a disadvantage
        if (outNumberMod > 1) healProbability *= outNumberMod;
        healProbability = ToTwoDecimalPlaces(healProbability);
        return healProbability;
    }

    private float ToTwoDecimalPlaces(float value)
    {
        return Mathf.Round(value * 100f) / 100f;
    }

    private bool InAttackRange(CharacterHolder defender, CharacterHolder attacker)
    {
        var distanceFromAI = CalcDistance(defender, attacker);
        var attackRange = attacker.Character.AttackRange.y;
        var movementRange = attacker.Character.Speed;
        return (attackRange + movementRange) > distanceFromAI;
    }

    private float CalcHealChanceBasedOnDamageTaken(CharacterHolder characterHolder, List<float> damageTakenPercent)
    {
        //N(N*J) for each ally in range of a potential heal
        //(N(N*J), ..., ...) * (OH / AH);
        //N = Damage Taken
        //J = Job Modiifier
        //OH = Opposition Total Health
        //AH = Ally Total Health

        float mod = characterHolder.Character.HealProbabilityModifier;

        List<float> calcIndividualDamageProb = new List<float>();
        foreach (var damage in damageTakenPercent)
        {
            float newDamage = (mod * damage) * damage;
            newDamage = Mathf.Round(newDamage * 100f) / 100f;
            calcIndividualDamageProb.Add(newDamage);
        }

        float value = calcIndividualDamageProb.Sum();
        if (value > 1f) value = 1f;

        return value;
    }

    private float increaseProbability(float currentValue, float newValue)
    {
        if (newValue > currentValue) currentValue = newValue;
        return currentValue;
    }

    private float CalcNumberOfAIAround(CharacterHolder characterHolder)
    {
        float aiInAttackRange = 0f;
        foreach (var ai in _aiCharacterHolders)
        {
            var distanceFromAI = CalcDistance(characterHolder, ai);
            var attackRange = ai.Character.AttackRange.y;
            var movementRange = ai.Character.Speed;
            if ((attackRange + movementRange) > distanceFromAI)
            {
                aiInAttackRange ++;
            }
        }
        return aiInAttackRange;
    }

    private float CalcNumberOfAlliesAround(CharacterHolder characterHolder)
    {

        float alliesInRange = 0f;
        foreach (var allies in _opposition)
        {
            var distanceFromAI = CalcDistance(characterHolder, allies);
            var attackRange = allies.Character.AttackRange.y;
            var movementRange = allies.Character.Speed;
            if ((attackRange + movementRange) > distanceFromAI)
            {
                alliesInRange++;
            }
        }
        return alliesInRange;
    }

    private List<CharacterHolder> GetAlliesAround(CharacterHolder characterHolder)
    {
        List<CharacterHolder> alliesInReach = new List<CharacterHolder>();
        foreach (var character in _opposition)
        {
            if (character == characterHolder) continue;
            var distanceFromCharacter = CalcDistance(characterHolder, character);
            //Setting this to one, as you have to be near to heal
            var healRange = 1f;
            var movementRange = character.Character.Speed;
            if ((healRange + movementRange) > distanceFromCharacter)
            {
                alliesInReach.Add(character);
            }
        }
        return alliesInReach;
    }

    private List<CharacterHolder> OppositionList()
    {
        List<CharacterHolder> allCharacters =
            GameObject.Find("Characters").GetComponent<CharactersController>().CharacterHolders;

        for (var i = 0; i < allCharacters.Count; i++)
        {
            if (allCharacters[i].IsAi) continue;
            _opposition.Add(allCharacters[i]);
        }

        return _opposition;
    }

    private List<CharacterHolder> AiList()
    {
        List<CharacterHolder> allCharacters =
           GameObject.Find("Characters").GetComponent<CharactersController>().CharacterHolders;

        for (var i = 0; i < allCharacters.Count; i++)
        {
            if (!allCharacters[i].IsAi) continue;
            _aiCharacterHolders.Add(allCharacters[i]);
        }

        return _aiCharacterHolders;
    }

    private int CalcPotentialDamage(CharacterHolder characterHolder)
    {
        //Take an average of three attacks, this will account for misses, crits
        int loops = 3; 
        DamageUtil damageUtil = new DamageUtil();
        float damageAmount = 0;
        for (var i = 0; i < loops; i++)
        {
           damageAmount += damageUtil.CalculatePhysicalDamage(_AI.Character, characterHolder.Character);
        }

        damageAmount /= loops;

        return Convert.ToInt32(damageAmount);
    }

    private float CalcPercentHP(CharacterHolder characterHolder)
    {
        float onePercent = 1f / characterHolder.Character.MaxHealth;
        float currentPercent = onePercent * characterHolder.Character.Health;
        return currentPercent;
    }

    private float CalcDistance(CharacterHolder characterHolder, CharacterHolder ai)
    {
        var characterPos = characterHolder.Character.XyPosition();
        var _AIPos = ai.Character.XyPosition();

        float differenceInX = CalculatePositiveDifference(_AIPos.x, characterPos.x);
        float differnceInY = CalculatePositiveDifference(_AIPos.y, characterPos.y);
        return differenceInX + differnceInY;
    }

    private float CalculatePositiveDifference(float valueOne, float valueTwo)
    {
        if (valueOne > valueTwo)
        {
            return valueOne - valueTwo;
        }
        return valueTwo - valueOne;
    }


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
