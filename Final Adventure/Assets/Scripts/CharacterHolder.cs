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
    public Turn Turn;
    

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


     //   transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.Lerp(transform.GetChild(0).GetComponent<MeshRenderer>().material.GetColor("_EmissionColor"), emissionColor, Time.deltaTime * 40f));
	    //if (transform.GetChild(0).GetComponent<MeshRenderer>().material.GetColor("_EmissionColor") ==
	    //    new Color(0.25f, 0f, 0f))
	    //{
     //       emissionColor = new Color(0.0f, 0, 0);
     //   }
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

            ActionBarItem.Actions action = GameObject.Find("ActionBar").GetComponent<ActionBar>().Action;
            if (action != ActionBarItem.Actions.None)
            {
                CompleteAction(action, tile, pointer);
            }
        }
    }

    private void CompleteAction(ActionBarItem.Actions action, Tile tile, Vector2 pointer)
    {
        if (GameObject.Find("AbilityBar").GetComponent<AbilityBar>().Animating) return;
        if (GameObject.Find("ActionBar").GetComponent<ActionBar>().Animating) return;

        if (action == ActionBarItem.Actions.Attack)
        {
            if (tile.GetState() != Tile.State.Attackable) return;
            transform.GetComponent<Damage>().Attack(Character, pointer);
            Turn.CompletedAction = true;
            GameObject.Find("ActionBar").GetComponent<ActionBar>().DisableAction();
            GameObject.Find("Floor").GetComponent<FloorHighlight>().ResetFloorHighlight();
            GameObject.Find("ActionBar").GetComponent<ActionBar>().Show();
        }

        if (action == ActionBarItem.Actions.Move)
        {
            if (tile.GetState() != Tile.State.Walkable) return;
            transform.GetComponent<Movement>().SetPosition(tile, pointer);
            Turn.Moved = true;
            GameObject.Find("Floor").GetComponent<FloorHighlight>().ResetFloorHighlight();
        }

        if (action == ActionBarItem.Actions.Magic)
        {
            if (GameObject.Find("AbilityBar").GetComponent<AbilityBar>().Animating) return;
            AbilityBarItem.Actions magic = GameObject.Find("AbilityBar").GetComponent<AbilityBar>().Action;
            if (magic == AbilityBarItem.Actions.Heal)
            {
                if (tile.GetState() != Tile.State.Attackable) return;
                transform.GetComponent<Damage>().Heal(Character, pointer);
                Turn.CompletedAction = true;
                GameObject.Find("ActionBar").GetComponent<ActionBar>().DisableAction();
                GameObject.Find("Floor").GetComponent<FloorHighlight>().ResetFloorHighlight();
                GameObject.Find("ActionBar").GetComponent<ActionBar>().Show();
            }

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
