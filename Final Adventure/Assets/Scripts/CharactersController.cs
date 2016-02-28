using UnityEngine;
using System.Collections;
using Assets.Scripts.Model;
using Assets.Scripts.Movement;

public class CharactersController : MonoBehaviour
{

    public CharacterHolder CurrentCharacterHolder { get; set; }

	// Use this for initialization
	void Start ()
	{
        //This is temp, need to have a list of characters in the game.
        //Only using one character at the moment
        CurrentCharacterHolder = transform.GetChild(0).GetComponent<CharacterHolder>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.K))
	    {
	        print("SWTICH!");
	        if (CurrentCharacterHolder == transform.GetChild(0).GetComponent<CharacterHolder>())
	            CurrentCharacterHolder = transform.GetChild(1).GetComponent<CharacterHolder>();
            else CurrentCharacterHolder = transform.GetChild(0).GetComponent<CharacterHolder>();

        }
	}

    public void HighlightCharacterMovement()
    {
        CurrentCharacterHolder.HighlightMovement();
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
