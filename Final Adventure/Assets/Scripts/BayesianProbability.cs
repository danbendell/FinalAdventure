using System;
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
    private List<CharacterProbabilities> _characterProbabilities = new List<CharacterProbabilities>();

    public List<CharacterProbabilities> CharacterProbabilities()
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
            CharacterProbabilities cp = new CharacterProbabilities(characterHolder.Job)
            {
                Attack = ProbabilityOfAttackNew(characterHolder),
                Move = ProbabilityOfMovement(characterHolder),
                Heal = ProbabilityOfHealNew(characterHolder)
            };
            characterHolder.Probabilities = cp;
            _characterProbabilities.Add(cp);
        }
    }

    private float ProbabilityOfAttackNew(CharacterHolder characterHolder)
    {
        float AINearCharacter = CalcNumberOfAIAround(characterHolder);
        float allyNearCharacter = CalcNumberOfAlliesAround(characterHolder);

        float AIPercentHP = CalcPercentHP(_AI);
        float TargetPercentHP = CalcPercentHP(characterHolder);

        float strength = characterHolder.Character.Strength;
        float defence = _AI.Character.Defence;

        float mod = 1.0f;
        if (characterHolder.Job == CharacterHolder.Jobs.Archer) mod = 1.2f;

        float allies = allyNearCharacter / AINearCharacter;
        float health = TargetPercentHP / AIPercentHP;
        float stats = strength / defence;

        float result = (allies * (health * stats)) * mod;
        result = Mathf.Round(result * 100f) / 100f;
        if (result > 1f) result = 1f;

        if (!InAttackRange(_AI, characterHolder)) result = 0f;
        return result;
    }

    private float ProbabilityOfAttack(CharacterHolder characterHolder)
    {
        float attackProbability = 0f;

        var distanceFromAI = CalcDistance(characterHolder, _AI);
        var attackRange = characterHolder.Character.AttackRange;
        var movementRange = characterHolder.Character.Speed;
        if ((attackRange + movementRange) > distanceFromAI)
        {
            attackProbability = 0.5f;
            //Possible for character to attack AI
            var characterPercentHP = CalcPercentHP(characterHolder);
            var AIPercentHP = CalcPercentHP(_AI);
            if (characterPercentHP > AIPercentHP)
            {
                attackProbability = 0.75f;
                //Greater chance of attack
                //The health percentage is in the favor of the attacker
                var simulatedDamage = CalcPotentialDamage(characterHolder);
                if (_AI.Character.Health < simulatedDamage)
                {
                    //Greatest chance of attack
                    //Should result in a kill
                    attackProbability = 0.9f;
                }
            }
            else
            {
                //Less chance of attack
                //The health percentage is in the favor of the defender
                //Sim AI HP (0.6) vs Char HP (0.2)
                //0.4
                var HPPercentDifference = AIPercentHP - characterPercentHP;
                //AP 0.5 -= 0.2
                attackProbability -= (HPPercentDifference / 2);
                //keep it to 2DP
                attackProbability = Mathf.Round(attackProbability * 100f) / 100f;
            }
        }

        //Can't ever be 100% sure, this accounts for poor dicision making
        //and other unknown stratagies 
        if (attackProbability > 0.9f) attackProbability = 0.9f;

        return attackProbability;
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
                movementProbability = 0.25f;
            }
            if (characterPercentHP < 0.4f)
            {
                movementProbability = 0.75f;
            }
        }
        else
        {
            movementProbability = 0.5f;
        }

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

    private float ProbabilityOfHeal(CharacterHolder characterHolder)
    {
        float healProbability = 0f;
        var characterPercentHP = CalcPercentHP(characterHolder);
        if (characterPercentHP < 0.75f)
        {
            //Set it at an 50 50 chance of heal
            healProbability = 0.5f;
            //Increase that heal potential based on how low the health is
            healProbability += 0.75f - characterPercentHP;
            //increase the chance beacuse the job is a wizard
            if (characterHolder.Job == CharacterHolder.Jobs.Wizard)
            {
                healProbability = increaseProbability(healProbability, 0.8f);
            }
        }

        //Healing allies
        List<CharacterHolder> allyNearCharacter = GetAlliesAround(characterHolder);
        foreach (var ally in allyNearCharacter)
        {
            var allyPercentHP = CalcPercentHP(ally);
            healProbability = increaseProbability(healProbability, 1 - allyPercentHP);

            if (characterHolder.Job == CharacterHolder.Jobs.Wizard)
            {
                healProbability = increaseProbability(healProbability, 0.8f);
            }
            
        }
        return healProbability;
    }

    private float ToTwoDecimalPlaces(float value)
    {
        return Mathf.Round(value * 100f) / 100f;
    }

    private bool InAttackRange(CharacterHolder defender, CharacterHolder attacker)
    {
        var distanceFromAI = CalcDistance(defender, attacker);
        var attackRange = attacker.Character.AttackRange;
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

        float mod = 1f;
        if (characterHolder.Job == CharacterHolder.Jobs.Wizard) mod = 1.5f;
        if (characterHolder.Job == CharacterHolder.Jobs.Archer) mod = 0.8f;

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
            var attackRange = ai.Character.AttackRange;
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
            var attackRange = allies.Character.AttackRange;
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
