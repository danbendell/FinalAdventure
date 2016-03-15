using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Model;

public class Damage : MonoBehaviour
{

    private Character Attacker { get; set; }
    private Character Defender { get; set; }

    private Character Healer { get; set; }
    private Character Reciever { get; set; }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool Attack(Character attacker, Vector2 pointer)
    {
        Attacker = attacker;
        Defender = FindCharacter(pointer);
        if (Defender == null) return false;

        DamageUtil damageUtil = new DamageUtil();
        int damageAmount = damageUtil.CalculatePhysicalDamage(Defender, Attacker);

        if (damageAmount == 0) print("MISS!!");

        Defender.TakeDamage(damageAmount);

        return true;
    }

    public bool Heal(Character healer, Vector2 pointer)
    {
        Healer = healer;
        Reciever = FindCharacter(pointer);
        if (Reciever == null) return false;

        DamageUtil damageUtil = new DamageUtil();
        int healAmount = damageUtil.CalculateHealAmount(Healer);

        Healer.Heal(Reciever, healAmount);
        return true;
    }
    
    private Character FindCharacter(Vector2 pointer)
    {
        List<CharacterHolder> characterHolders =
            GameObject.Find("Characters").GetComponent<CharactersController>().CharacterHolders;
        Defender = null;

        foreach (CharacterHolder CH in characterHolders)
        {
            if (CH.Character.XyPosition() != pointer) continue;
            return CH.Character;
        }

        return null;
    }
}
