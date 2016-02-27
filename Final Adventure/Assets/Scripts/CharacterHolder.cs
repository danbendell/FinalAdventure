using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Model;
using Assets.Scripts.Movement;
using Assets.Scripts.Util;
using UnityEngine.UI;

public class CharacterHolder : MonoBehaviour
{

    public Character Character;
    private UICharacterStats _uiCharacterStats;
    private float newHp;
    private float newMp;

    public Jobs Job;
    public enum Jobs
    {
        Wizard,
        Archer
    }

    // Use this for initialization
    void Start () {
        switch (Job)
        {
            case Jobs.Wizard:
                Character = new Wizard();
                break;
            case Jobs.Archer:
                Character = new Archer();
                break;
        }
        Character.Position = transform.position;

        _uiCharacterStats = GameObject.Find("Canvas").GetComponent<UICharacterStats>();
        _uiCharacterStats.UpdateCharacterStats(Character);
        
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.L))
        {
            Character.WhoAmI();
        }

	    if (Input.GetKeyDown(KeyCode.M))
	    {
            Vector2 floorPosition = new Vector2(Character.Position.x, Character.Position.z);
            GameObject.Find("Floor").GetComponent<FloorHighlight>().SetMovement(floorPosition, Character.Speed);
        }

        if (Input.GetKeyDown(KeyCode.O))
	    {
            Character tempCharacter = new Archer();
            DamageUtil damageUtil = new DamageUtil();
            int damageAmount = damageUtil.CalculatePhysicalDamage(Character, tempCharacter);

            Character.TakeDamage(damageAmount);
	        _uiCharacterStats.UpdateCharacterStats(Character);
           
	    }

        if (Input.GetKeyDown(KeyCode.P))
	    {
	        Character.Heal(10);
            _uiCharacterStats.UpdateCharacterStats(Character);
        }

    }
}
