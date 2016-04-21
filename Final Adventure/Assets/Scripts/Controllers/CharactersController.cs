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
            SetUpAPIData();
            GameObject.Find("Menus").GetComponent<MenuController>().Refresh(_currentCharacterHolder);
            GameObject.Find("Floor").GetComponent<FloorHighlight>().SetPosition(_currentCharacterHolder.Character.XyPosition());
            if(_currentCharacterHolder.IsAi) _currentCharacterHolder.transform.GetComponent<AI>().BeginTurn();
        }
    }

    private UICharacterStats _uiCharacterStats;

    // Use this for initialization
    void Start ()
	{

        CharacterHolders = new List<CharacterHolder>();
        for (var i = 0; i < transform.childCount; i++)
        {
            CharacterHolder characterHolder = transform.GetChild(i).GetComponent<CharacterHolder>();
            characterHolder.Load();
            CharacterHolders.Add(characterHolder);
        }

        CurrentCharacterHolder = CharacterHolders[0];

        _uiCharacterStats = GameObject.Find("Canvas").GetComponent<UICharacterStats>();
    }
	
	// Update is called once per frame
	void Update () {
	    CheckTurnStatus();
	    CheckPointerPosition();
	    CheckDeadStatus();
	}

    private void CheckTurnStatus()
    {
        if (!CurrentCharacterHolder.Turn.Complete()) return;
        //RotateCharacter();
        if (!_currentCharacterHolder.IsAi) SendApiData();
        NextPlayer();
    }

    private void SendApiData()
    {
        GameObject.Find("Util").GetComponent<APIController>().SendData(_currentCharacterHolder.Turn.Moved);
    }

    private void RotateCharacter()
    {
        RotationNodes rotationNodes = GameObject.Find("Rotation").GetComponent<RotationNodes>();
        rotationNodes.Active = true;
    }

    private void CheckDeadStatus()
    {
        foreach (var characterHolder in CharacterHolders)
        {
            if (characterHolder.IsDead)
            {
                if(characterHolder == CurrentCharacterHolder) NextPlayer();
                CharacterHolders.Remove(characterHolder);
                break;
            }
        }
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
                bool isSelf = (CurrentCharacterHolder == CharacterHolders[i]);
                _uiCharacterStats.UpdateCharacterStats(CharacterHolders[i], isSelf);
                GameObject.Find("Stats").GetComponent<StatsBar>().Show();
                break;
            }

            _uiCharacterStats.UpdateCharacterStats(CurrentCharacterHolder, true);
            GameObject.Find("Stats").GetComponent<StatsBar>().Hide();
        }
    }

    private void SetUpAPIData()
    {
        int surroundingAllyCount = GetSurroundingCharacterCount(true);
        int surroundingOppostionCount = GetSurroundingCharacterCount(false);
        int totalAllyCount = GetTotalCharacterCount(true);
        int totalOppositionCount = GetTotalCharacterCount(false);
        string job = _currentCharacterHolder.Job.ToString();
        float healthPercent = CalculateUtil.CalcPercentHP(_currentCharacterHolder);
        float manaPercent = CalculateUtil.CalcPercentMP(_currentCharacterHolder);

        GameObject.Find("Util").GetComponent<APIController>().SetData(surroundingAllyCount, surroundingOppostionCount, totalAllyCount, totalOppositionCount, job, healthPercent, manaPercent);
    }

    private int GetSurroundingCharacterCount(bool isAllyCount)
    {
        int allyCount = 0;
        int oppositionCount = 0;

        foreach (var characterHolder in CharacterHolders)
        {
            if(characterHolder == _currentCharacterHolder) continue;
            if(CalculateUtil.InAttackRange(characterHolder, _currentCharacterHolder) == false) continue;

            if (characterHolder.IsAi) oppositionCount++;
            else allyCount++;
        }
        return isAllyCount ? allyCount : oppositionCount;
    }

    private int GetTotalCharacterCount(bool isAllyCount)
    {
        int allyCount = 0;
        int oppositionCount = 0;

        foreach (var characterHolder in CharacterHolders)
        {
            if (characterHolder == _currentCharacterHolder) continue;

            if (characterHolder.IsAi) oppositionCount++;
            else allyCount++;
        }
        return isAllyCount ? allyCount : oppositionCount;
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
