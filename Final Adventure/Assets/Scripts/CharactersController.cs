using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Model;
using Assets.Scripts.Movement;

public class CharactersController : MonoBehaviour
{
    public List<CharacterHolder> CharacterHolders;
    private int _turnNumber = 0;

    private CharacterHolder _currentCharacterHolder;

    public CharacterHolder CurrentCharacterHolder
    {
        get { return _currentCharacterHolder; }
        set
        {
            _currentCharacterHolder = value;
            _currentCharacterHolder.Turn = new Turn();
            GameObject.Find("Floor").GetComponent<FloorHighlight>().SetPosition(_currentCharacterHolder.Character.XyPosition());
            if(_currentCharacterHolder.IsAi) transform.GetChild(_turnNumber).GetComponent<AI>().BeginTurn();
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
            _turnNumber = i + 1 >= CharacterHolders.Count ? 0 : i + 1;
            CurrentCharacterHolder = CharacterHolders[_turnNumber];
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

    public void FillMap(bool[,] map)
    {
        foreach (var holder in CharacterHolders)
        {
            if(holder == CurrentCharacterHolder) continue;
            
            var position = holder.Character.XyPosition();
            map[(int)position.x, (int)position.y] = false;
        }
    }
}
