using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Model;
using Assets.Scripts.Movement;

public class CharactersController : MonoBehaviour
{
    public List<CharacterHolder> CharacterHolders; 

    public CharacterHolder CurrentCharacterHolder { get; set; }

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
	    if (Input.GetKeyDown(KeyCode.K))
	    {
	        if (CurrentCharacterHolder == transform.GetChild(0).GetComponent<CharacterHolder>())
	            CurrentCharacterHolder = transform.GetChild(1).GetComponent<CharacterHolder>();
            else CurrentCharacterHolder = transform.GetChild(0).GetComponent<CharacterHolder>();
        }

	    CheckPointerPosition();
        
    }

    private void CheckPointerPosition()
    {
        for (var i = 0; i < CharacterHolders.Count; i++)
        {
            Vector2 characterPos = new Vector2(CharacterHolders[i].Character.Position.x, CharacterHolders[i].Character.Position.z);
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
