using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Model;
using Assets.Scripts.Movement;
using UnityEngine.UI;

public class CarouselController : MonoBehaviour
{

    public GameObject CurrentCharacter;
    public GameObject NewCharacter;

    public List<GameObject> ChosenCharacters;

    // Use this for initialization
    void Start ()
	{
        //NewCharacter = Instantiate(CurrentCharacter);
        ChosenCharacters = new List<GameObject>();
    }
	
	// Update is called once per frame
	void Update ()
	{
	    //ChangeCharacter();
	    MoveCharacter();
	    KeyboardInput();

        transform.GetComponent<RotateCharacter>().CurrentCharacter = CurrentCharacter;

	    Character character = CurrentCharacter.GetComponent<CarouselCharacterHolder>().Character;

	    GameObject.Find("Job").GetComponent<Text>().text = character.Job().ToString();
	    GameObject.Find("HP").GetComponent<Text>().text = character.MaxHealth.ToString();
	    GameObject.Find("MP").GetComponent<Text>().text = character.MaxMana.ToString();
	    GameObject.Find("STR").GetComponent<Text>().text = character.Strength.ToString();
	    GameObject.Find("DEF").GetComponent<Text>().text = character.Defence.ToString();
	    GameObject.Find("MAG").GetComponent<Text>().text = character.Magic.ToString();
	    GameObject.Find("RES").GetComponent<Text>().text = character.Resist.ToString();
	    GameObject.Find("SPE").GetComponent<Text>().text = character.Speed.ToString();
	    GameObject.Find("ACC").GetComponent<Text>().text = character.Accuracy.ToString();
	    GameObject.Find("EVA").GetComponent<Text>().text = character.Evasion.ToString();

        if (GameObject.Find("CarouselBar").GetComponent<CarouselBar>().State != MenuBar.States.Enabled) return;
        GameObject.Find("Info").GetComponent<Text>().text = character.Information();
	}

    private void MoveCharacter()
    {
        if (NewCharacter == null) return;
        if (GameObject.Find("Main Camera").GetComponent<MoveCamera>().IsAtCarousel()) return;
        Vector2 pointer = GameObject.Find("Floor").GetComponent<FloorHighlight>().PointerPosition;
        NewCharacter.transform.localPosition = new Vector3(pointer.x, -9f, pointer.y);
    }

    private void KeyboardInput()
    {
        
    }

    public bool IsPositionTaken()
    {
        Vector2 pointer = GameObject.Find("Floor").GetComponent<FloorHighlight>().PointerPosition;
        foreach (var characterGO in ChosenCharacters)
        {
            Character character = characterGO.GetComponent<CarouselCharacterHolder>().Character;
            if (pointer == character.XyPosition())
            {
                return true;
            }
        }
        return false;
    }

    public void SavePlayer()
    {
        if (IsPositionTaken()) return;
        Vector2 pointer = GameObject.Find("Floor").GetComponent<FloorHighlight>().PointerPosition;
        NewCharacter.GetComponent<CarouselCharacterHolder>().Character.Position = new Vector3(pointer.x, 0, pointer.y);
        ChosenCharacters.Add(NewCharacter);
        GameObject.Find("Main Camera").GetComponent<MoveCamera>().MoveToCarousel();  
    }

    public void PlaceCharacter()
    {
        NewCharacter = Instantiate(CurrentCharacter);
        Vector2 pointer = GameObject.Find("Floor").GetComponent<FloorHighlight>().PointerPosition;
        NewCharacter.transform.localPosition = new Vector3(pointer.x, -9f, pointer.y);
    }

    public void RemoveCharacter()
    {
        Destroy(NewCharacter);
    }

    public void RemoveTargetCharacter()
    {
        Vector2 pointer = GameObject.Find("Floor").GetComponent<FloorHighlight>().PointerPosition;
        foreach (var characterGO in ChosenCharacters)
        {
            Character character = characterGO.GetComponent<CarouselCharacterHolder>().Character;
            if (pointer == character.XyPosition())
            {
                ChosenCharacters.Remove(characterGO);
                Destroy(characterGO);
                return;
            }
        }
    }
}
