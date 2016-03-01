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

        transform.GetComponent<Movement>().SetCharacter(Character);
        
    }
	
	// Update is called once per frame
	void Update () {
        KeyboardInput();
    }

    private void KeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Character.WhoAmI();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (GameObject.Find("Characters").GetComponent<CharactersController>().CurrentCharacterHolder.Character !=
               Character) return;

            var floor = GameObject.Find("Floor").GetComponent<FloorHighlight>().FloorArray;
            var pointer = GameObject.Find("Floor").GetComponent<FloorHighlight>().PointerPosition;
            Tile tile = floor[(int)pointer.x, (int)pointer.y];

            if (GameObject.Find("ActionBar").GetComponent<ActionBar>().Action == ActionBarItem.Actions.Attack)
            {
                if (tile.GetState() != Tile.State.Attackable) return;
                print("Attack");
                transform.GetComponent<Damage>().Attack(Character, pointer);
            }

            if (GameObject.Find("ActionBar").GetComponent<ActionBar>().Action == ActionBarItem.Actions.Move)
            {
                if (tile.GetState() != Tile.State.Walkable) return;
                print("Move");
                transform.GetComponent<Movement>().SetPosition(tile, pointer);
            }
                
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            DamageUtil damageUtil = new DamageUtil();
            int healAmount = damageUtil.CalculateHealAmount(Character);

            Character.Heal(healAmount);
        }
    }

    public void HighlightMovement()
    {
        GameObject.Find("Floor").GetComponent<FloorHighlight>().SetMovement(Character.XyPosition(), Character.Speed);
    }

    public void HighlightAttackRange()
    {
        //needs to have some sort of attack range instead of one
        GameObject.Find("Floor").GetComponent<FloorHighlight>().SetAttackRange(Character.XyPosition(), 1);
    }

}
