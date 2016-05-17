using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Damage;
using Assets.Scripts.Damage.Abilities;
using Assets.Scripts.Damage.Magic;
using Assets.Scripts.Model;
using Flare = Assets.Scripts.Damage.Flare;

public class Damage : MonoBehaviour
{

    private Character Attacker { get; set; }
    private Character Defender { get; set; }

    private Character Healer { get; set; }
    private Character Reciever { get; set; }

    private DamageText DamageText;

	// Use this for initialization
	void Start ()
	{
	    DamageText = GameObject.Find("DamageTextHolder").GetComponent<DamageText>();
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

        if (damageAmount == 0) DamageText.AnimateMessage("Miss");
        DamageText.AnimateDamage("Attack " + damageAmount);

        Attacker.Attack(Defender, damageAmount);

        return true;
    }

    public bool Focus(Character attacker, Vector2 pointer)
    {
        Attacker = attacker;
        Defender = FindCharacter(pointer);
        if (Defender == null) return false;

        DamageUtil damageUtil = new DamageUtil();
        int damageAmount = damageUtil.CalculateFocusDamage(Defender, Attacker);
        Focus focus = new Focus();

        DamageText.AnimateDamage(focus.Name + " " + damageAmount);
       
        return focus.Cast(Attacker, Defender, damageAmount);
    }

    public bool Slash(Character attacker, Vector2 pointer)
    {
        Attacker = attacker;
        Defender = FindCharacter(pointer);
        if (Defender == null) return false;

        DamageUtil damageUtil = new DamageUtil();
        int damageAmount = damageUtil.CalculateSlashDamage(Defender, Attacker);

        Slash slash = new Slash();
        DamageText.AnimateDamage(slash.Name + " " + damageAmount);
        return slash.Cast(Attacker, Defender, damageAmount);
    }

    public bool Assassinate(Character attacker, Vector2 pointer)
    {
        Attacker = attacker;
        Defender = FindCharacter(pointer);
        if (Defender == null) return false;

        DamageUtil damageUtil = new DamageUtil();
        int damageAmount = damageUtil.CalculateAssassinateDamage(Defender, Attacker);

        Assassinate assassinate = new Assassinate();

        DamageText.AnimateDamage(assassinate.Name + " " + damageAmount);
        return assassinate.Cast(Attacker, Defender, damageAmount);
    }

    public bool BloodBlade(Character attacker, Vector2 pointer)
    {
        Attacker = attacker;
        Defender = FindCharacter(pointer);
        if (Defender == null) return false;

        DamageUtil damageUtil = new DamageUtil();
        int damageAmount = damageUtil.CalculateBloodBladeDamage(Defender, Attacker);

        BloodBlade bloodBlade = new BloodBlade();
        DamageText.AnimateDamage(bloodBlade.Name + " " + damageAmount);
        return bloodBlade.Cast(Attacker, Defender, damageAmount);
    }

    public bool Heal(Character healer, Vector2 pointer)
    {
        Healer = healer;
        Reciever = FindCharacter(pointer);
        if (Reciever == null) return false;
        
        DamageUtil damageUtil = new DamageUtil();
        int healAmount = damageUtil.CalculateHealAmount(Healer);

        Heal heal = new Heal();
        DamageText.AnimateDamage(heal.Name + " " + healAmount);
        return heal.Cast(Healer, Reciever, healAmount);
    }

    public bool Flare(Character attacker, Vector2 pointer)
    {
        Attacker = attacker;
        Defender = FindCharacter(pointer);
        if (Defender == null) return false;

        DamageUtil damageUtil = new DamageUtil();
        int damageAmount = damageUtil.CalculateFlareDamage(Defender, Attacker);

        Flare flare = new Flare();
        DamageText.AnimateDamage(flare.Name + " " + damageAmount);
        return flare.Cast(Attacker, Defender, damageAmount);
    }

    public bool Wind(Character attacker, Vector2 pointer)
    {
        Attacker = attacker;
        Defender = FindCharacter(pointer);
        if (Defender == null) return false;

        DamageUtil damageUtil = new DamageUtil();
        int damageAmount = damageUtil.CalculateWindDamage(Defender, Attacker);

        Wind wind = new Wind();
        DamageText.AnimateDamage(wind.Name + " " + damageAmount);
        return wind.Cast(Attacker, Defender, damageAmount);
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
