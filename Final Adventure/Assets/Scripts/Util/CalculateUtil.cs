using UnityEngine;
using System.Collections;
using Assets.Scripts.Model;

public static class CalculateUtil {

    public static float CalcDistance(CharacterHolder characterHolder, CharacterHolder targetCharacterHolder)
    {
        var characterPos = characterHolder.Character.XyPosition();
        var targetPos = targetCharacterHolder.Character.XyPosition();

        float differenceInX = CalculatePositiveDifference(targetPos.x, characterPos.x);
        float differnceInY = CalculatePositiveDifference(targetPos.y, characterPos.y);
        return differenceInX + differnceInY;
    }

    public static float CalcDistance(Character character, Character targetCharacter)
    {
        var characterPos = character.XyPosition();
        var targetPos = targetCharacter.XyPosition();

        float differenceInX = CalculatePositiveDifference(targetPos.x, characterPos.x);
        float differnceInY = CalculatePositiveDifference(targetPos.y, characterPos.y);
        return differenceInX + differnceInY;
    }

    public static float CalculatePositiveDifference(float valueOne, float valueTwo)
    {
        if (valueOne > valueTwo)
        {
            return valueOne - valueTwo;
        }
        else
        {
            return valueTwo - valueOne;
        }
    }

    public static float CalcPercentHP(CharacterHolder characterHolder)
    {
        float onePercent = 1f / characterHolder.Character.MaxHealth;
        float currentPercent = onePercent * characterHolder.Character.Health;
        return currentPercent;
    }

    public static float CalcPercentMP(CharacterHolder characterHolder)
    {
        float onePercent = 1f / characterHolder.Character.MaxMana;
        float currentPercent = onePercent * characterHolder.Character.Mana;
        return currentPercent;
    }

    public static bool InAttackRange(CharacterHolder defender, CharacterHolder attacker)
    {
        int distanceFromAI = (int) CalcDistance(defender, attacker);
        int attackRange = (int) attacker.Character.AttackRange.y;
        int movementRange = attacker.Character.Speed;
        return (attackRange + movementRange) >= distanceFromAI;
    }
}
