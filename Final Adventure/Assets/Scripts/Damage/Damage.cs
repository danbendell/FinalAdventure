using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Model;

public class Damage : MonoBehaviour
{

    private Character Attacker { get; set; }
    private Character Defender { get; set; }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Attack(Character attacker, Vector2 pointer)
    {
        Attacker = attacker;
        FindDefender(pointer);
        if (Defender == null) return;

        DamageUtil damageUtil = new DamageUtil();
        int damageAmount = damageUtil.CalculatePhysicalDamage(Defender, Attacker);

        if (damageAmount == 0) print("MISS!!");

        Defender.TakeDamage(damageAmount);
    }
    
    private void FindDefender(Vector2 pointer)
    {
        List<CharacterHolder> characterHolders =
            GameObject.Find("Characters").GetComponent<CharactersController>().CharacterHolders;
        Defender = null;

        foreach (CharacterHolder CH in characterHolders)
        {
            if (CH.Character.XyPosition() != pointer) continue;
            Defender = CH.Character;
        }
    }
}
