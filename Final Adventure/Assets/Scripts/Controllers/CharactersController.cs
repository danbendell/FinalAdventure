using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Model;
using Assets.Scripts.Model.Menu;
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
            
        }
    }

    private UICharacterStats _uiCharacterStats;

    // Use this for initialization
    void Start ()
    {
    
        for (var i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<CharacterHolder>() != null) transform.GetChild(i).GetComponent<CharacterHolder>().enabled = true;
            if (transform.GetChild(i).GetComponent<Movement>() != null) transform.GetChild(i).GetComponent<Movement>().enabled = true;
            if(transform.GetChild(i).GetComponent<CarouselCharacterHolder>() != null) transform.GetChild(i).GetComponent<CarouselCharacterHolder>().enabled = false;
            if (transform.GetChild(i).GetComponent<AI>() != null)
            {
                transform.GetChild(i).GetComponent<CharacterHolder>().IsAi = true;
                transform.GetChild(i).GetComponent<AI>().enabled = true;
            }

            CharacterHolder characterHolder = transform.GetChild(i).GetComponent<CharacterHolder>();
            CharacterHolders.Add(characterHolder);
        }

        _uiCharacterStats = GameObject.Find("Canvas").GetComponent<UICharacterStats>();

        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(1.0f);
        foreach (var characterHolder in CharacterHolders)
        {
            characterHolder.Load();
        }
        SetNextCharacterHolder(CharacterHolders[0]);
    }
	
	// Update is called once per frame
	void Update ()
	{
	    if (GameObject.Find("Canvas").GetComponent<GamePause>().isEndGame) return;
	    if (CurrentCharacterHolder == null) return;

	    if (Input.GetKeyDown(KeyCode.Tab))
	    {
	        CurrentCharacterHolder.Turn.Skip();
	    }

	    CheckTurnStatus();
	    CheckPointerPosition();
	    CheckDeadStatus();
	    CheckEndGame();
	}

    private void CheckTurnStatus()
    {
        if (CurrentCharacterHolder == null) return;
        if (!CurrentCharacterHolder.Turn.Complete()) return;
        //RotateCharacter();
        if (!_currentCharacterHolder.IsAi) SendApiData();
        NextPlayer();
    }

    private void CheckEndGame()
    {
        for (int i = 0; i < CharacterHolders.Count; i++)
        {
            if (CharacterHolders[i].IsAi) break;
            if (i == CharacterHolders.Count - 1)
            {
                StartCoroutine(GameObject.Find("Canvas").GetComponent<EndGameController>().ShowVictory());
            }
        }

        for (int i = 0; i < CharacterHolders.Count; i++)
        {
            if (!CharacterHolders[i].IsAi) break;
            if (i == CharacterHolders.Count - 1)
            {
                StartCoroutine(GameObject.Find("Canvas").GetComponent<EndGameController>().ShowDefeat());
            }
        }
    }

    private void SendApiData()
    {
        APIController.SendData(_currentCharacterHolder.Turn.Moved);
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
            if (characterHolder.Character.Health <= 0)
            {
                ParticleController particleController = GameObject.Find("Dead").GetComponent<ParticleController>();
                particleController.Play(characterHolder.Character.XyPosition());
                SoundUtil sounds = GameObject.Find("Sounds").GetComponent<SoundUtil>();
                sounds.PlaySound(SoundUtil.Sounds.Dead);
                if (characterHolder == CurrentCharacterHolder) NextPlayer();
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
            SetNextCharacterHolder(CharacterHolders[_turnNumber]);
            break;
        }
    }

    private void SetNextCharacterHolder(CharacterHolder characterHolder)
    {
        CurrentCharacterHolder = characterHolder;
        CurrentCharacterHolder.Turn = new Turn();
        SetUpAPIData(!CurrentCharacterHolder.IsAi);
        GetAPIData();
        GameObject.Find("CurrentCharacterParticles").GetComponent<FollowTarget>().BeginFollow();
        GameObject.Find("Menus").GetComponent<MenuController>().Refresh(CurrentCharacterHolder);
        GameObject.Find("Floor").GetComponent<FloorHighlight>().SetPosition(CurrentCharacterHolder.Character.XyPosition());
        if (CurrentCharacterHolder.IsAi)
        {
            AI ai = CurrentCharacterHolder.transform.GetComponent<AI>();
            StartCoroutine(ai.BeginTurn());
        }
    }

    private void CheckPointerPosition()
    {
        _uiCharacterStats = GameObject.Find("Canvas").GetComponent<UICharacterStats>();
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
            GameObject.Find("Stats").GetComponent<StatsBar>().Close();
        }
    }

    private void SetUpAPIData(bool isHuman)
    {
        int surroundingAllyCount = GetSurroundingCharacterCount(isHuman);
        int surroundingOppostionCount = GetSurroundingCharacterCount(!isHuman);
        int totalAllyCount = GetTotalCharacterCount(isHuman);
        int totalOppositionCount = GetTotalCharacterCount(!isHuman);
        string job = _currentCharacterHolder.Job.ToString();
        float healthPercent = CalculateUtil.CalcPercentHP(_currentCharacterHolder) * 100;
        float manaPercent = CalculateUtil.CalcPercentMP(_currentCharacterHolder) * 100;

        APIController.SetData(surroundingAllyCount, surroundingOppostionCount, totalAllyCount, totalOppositionCount, job, healthPercent, manaPercent);
    }

    private void GetAPIData()
    {
        APIController.GetData();
    }

    private int GetSurroundingCharacterCount(bool isAllyCount)
    {
        int allyCount = 0;
        int oppositionCount = 0;

        foreach (var characterHolder in CharacterHolders)
        {
            if (characterHolder == _currentCharacterHolder) continue;
            if (characterHolder.Character.Health <= 0) continue;
            if (CalculateUtil.InAttackRange(_currentCharacterHolder, characterHolder) == false) continue;


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
            if (characterHolder.Character.Health <= 0) continue;
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
