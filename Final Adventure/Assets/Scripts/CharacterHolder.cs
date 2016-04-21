using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Model;
using Assets.Scripts.Movement;
using Assets.Scripts.Util;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;

public class CharacterHolder : MonoBehaviour
{

    public Character Character;
    public CharacterProbabilities Probabilities;
    public float PriorityLevel;
    public Turn Turn;
    public bool IsAi;

    public bool IsDead;

    public Jobs Job;
    public enum Jobs
    {
        Wizard,
        Archer,
        Worrior
    }

    // Use this for initialization
    public void Load () {
        switch (Job)
        {
            case Jobs.Wizard:
                Character = new Wizard();
                break;
            case Jobs.Archer:
                Character = new Archer();
                break;
            case Jobs.Worrior:
                Character = new Warrior();
                break;
        }
        Character.Position = transform.position;

        transform.GetComponent<Movement>().SetCharacter(Character);
    }
	
	// Update is called once per frame
	void Update () {
        KeyboardInput();
	    CheckStatus();
	}

    private void KeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (GameObject.Find("Characters").GetComponent<CharactersController>().CurrentCharacterHolder.Character !=
               Character) return;

            MenuController menuController = GameObject.Find("Menus").GetComponent<MenuController>();

            var floor = GameObject.Find("Floor").GetComponent<FloorHighlight>().FloorArray;
            var pointer = GameObject.Find("Floor").GetComponent<FloorHighlight>().PointerPosition;
            Tile tile = floor[(int)pointer.x, (int)pointer.y];

            ActionBarItem.Actions action = menuController.ActionBar.Action;
            if (action != ActionBarItem.Actions.None)
            {
                CompleteAction(action, tile, pointer);
            }
        }
    }

    private void CheckStatus()
    {
        if (Character.Health <= 0)
        {
            RemoveCharacter();
        }

        if (transform.localScale.x <= 0.1f)
        {
            if (IsDead) return;
            ParticleController particleController = GameObject.Find("Dead").GetComponent<ParticleController>();
            particleController.Play(Character.XyPosition());
            IsDead = true;
        }
    }

    private void RemoveCharacter()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0, 0, 0), 5f * Time.deltaTime);
    }

    private void CompleteAction(ActionBarItem.Actions action, Tile tile, Vector2 pointer)
    {
        MenuController menuController = GameObject.Find("Menus").GetComponent<MenuController>();
        if (menuController.Animating()) return;

        Abilities abilities = new Abilities(Character, pointer);

        if (action == ActionBarItem.Actions.Attack)
        {
            if (tile.GetState() != Tile.State.Attackable) return;
            abilities.Attack();
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
            MagicBarItem.Spells magic = menuController.MagicBar.Spell;
            if (magic == MagicBarItem.Spells.Heal)
            {
                if (tile.GetState() != Tile.State.Attackable) return;
                abilities.Heal();
            }

            if (magic == MagicBarItem.Spells.Flare)
            {
                if (tile.GetState() != Tile.State.Attackable) return;
                abilities.Flare();
            }

            if (magic == MagicBarItem.Spells.Wind)
            {
                //if (tile.GetState() != Tile.State.Attackable) return;
                abilities.Wind();
            }
        }

        if (action == ActionBarItem.Actions.Ability)
        {
            AbilityBarItem.Abilities ability = menuController.AbilityBar.Ability;
            if (ability == AbilityBarItem.Abilities.Focus)
            {
                if (tile.GetState() != Tile.State.Attackable) return;
                abilities.Focus();
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
        GameObject.Find("Floor").GetComponent<FloorHighlight>().SetAttackRange(Character.XyPosition(), Character.AttackRange);
    }

}
