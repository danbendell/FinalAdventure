using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Model;
using Assets.Scripts.Movement;

public class CharactersController : MonoBehaviour
{
    public List<CharacterHolder> CharacterHolders;


    private CharacterHolder _currentCharacterHolder;

    public CharacterHolder CurrentCharacterHolder
    {
        get { return _currentCharacterHolder; }
        set
        {
            _currentCharacterHolder = value;
            _currentCharacterHolder.Turn = new Turn();
            GameObject.Find("Floor").GetComponent<FloorHighlight>().SetPointerPosition(_currentCharacterHolder.Character.XyPosition());
        }
    }

    private UICharacterStats _uiCharacterStats;

    // Use this for initialization
    void Start ()
	{

        CharacterHolders = new List<CharacterHolder>();
        for (var i = 0; i < transform.childCount; i++)
        {
            CharacterHolders.Add(transform.GetChild(i).GetComponent<CharacterHolder>());
        }

        CurrentCharacterHolder = CharacterHolders[0];

        _uiCharacterStats = GameObject.Find("Canvas").GetComponent<UICharacterStats>();
    }
	
	// Update is called once per frame
	void Update () {
	    CheckTurnStatus();
	    CheckPointerPosition();        
    }

    private void CheckTurnStatus()
    {
        if (!CurrentCharacterHolder.Turn.Complete()) return;
        NextPlayer();
    }

    private void NextPlayer()
    {
        for (var i = 0; i < CharacterHolders.Count; i++)
        {
            if (CharacterHolders[i] != CurrentCharacterHolder) continue;
            CurrentCharacterHolder = i + 1 >= CharacterHolders.Count ? CharacterHolders[0] : CharacterHolders[i + 1];
            break;
        }
    }

    private void CheckPointerPosition()
    {
        for (var i = 0; i < CharacterHolders.Count; i++)
        {
            Vector2 characterPos = CharacterHolders[i].Character.XyPosition();
            if (GameObject.Find("Floor").GetComponent<FloorHighlight>().PointerPosition == characterPos)
            {
                _uiCharacterStats.UpdateCharacterStats(CharacterHolders[i].Character);
                GameObject.Find("Stats").GetComponent<StatsBar>().Show();
                break;
            }

            _uiCharacterStats.UpdateCharacterStats(CurrentCharacterHolder.Character);
            GameObject.Find("Stats").GetComponent<StatsBar>().Hide();
        }
    }

    public void HighlightCharacterMovement()
    {
        CurrentCharacterHolder.HighlightMovement();
    }

    public void HighlightCharacterAttackRange()
    {
        CurrentCharacterHolder.HighlightAttackRange();
    }

    public void CancelAction()
    {
        HideMovement();
        //CancelAttack();
        //CancelMagic();
    }

    private void HideMovement()
    {
        GameObject.Find("Floor").GetComponent<FloorHighlight>().ResetFloorHighlight();
    }
}
