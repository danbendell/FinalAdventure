using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Model;
using Assets.Scripts.Model.Menu;
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
        if (GameObject.Find("CarouselBar").GetComponent<CarouselBar>().State == MenuBar.States.Enabled) return;
        if (IsPositionTaken()) return;
        if (GameObject.Find("Main Camera").GetComponent<MoveCamera>().IsAtCarousel()) return;
        if (NewCharacter == null) PlaceCharacter();
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

    public void SaveCharacters()
    {
        ApplicationModel.CharacterHolders = new List<CharacterHolder>();
        GameObject characterController = Instantiate((GameObject)Resources.Load("CharacterController"));
        characterController.name = "Characters";
        foreach (var character in ChosenCharacters)
        {
            ApplicationModel.CharacterHolders.Add(character.GetComponent<CharacterHolder>());
            character.transform.SetParent(characterController.transform);
        }

        LoadAI();
        
        DontDestroyOnLoad(characterController);
    }

    private void LoadAI()
    {
        
        //Load one of each characters
        //Wizard 
        CreateAI("WizardAI", new Vector3(10, -9f, 3));

        //Archer
        CreateAI("ArcherAI", new Vector3(10, -9f, 5));

        //Warrior
        CreateAI("WarriorAI", new Vector3(9, -9f, 4));

        //Assassin
        CreateAI("AssassinAI", new Vector3(9, -9f, 6));

        //Priest
        CreateAI("PriestAI", new Vector3(10, -9f, 7));
    }

    private void CreateAI(string type, Vector3 position)
    {
        GameObject characterController = GameObject.Find("Characters");
        NewCharacter = Instantiate((GameObject)Resources.Load(type));
        NewCharacter.GetComponent<CharacterHolder>().enabled = false;
        NewCharacter.GetComponent<Movement>().enabled = false;
        NewCharacter.transform.localPosition = position;
        NewCharacter.transform.localEulerAngles = new Vector3(NewCharacter.transform.localEulerAngles.x, NewCharacter.transform.localEulerAngles.y + 180, NewCharacter.transform.localEulerAngles.z);
        NewCharacter.transform.SetParent(characterController.transform);
        ChosenCharacters.Add(NewCharacter);
    }


    public void PlaceCharacter()
    {
        if (IsPositionTaken())
        {
            MovePointer();
        }
        NewCharacter = Instantiate((GameObject) Resources.Load(CurrentCharacter.GetComponent<CarouselCharacterHolder>().Job.ToString()));
        NewCharacter.GetComponent<CharacterHolder>().enabled = false;
        NewCharacter.GetComponent<Movement>().enabled = false;

        Vector2 pointer = GameObject.Find("Floor").GetComponent<FloorHighlight>().PointerPosition;
        NewCharacter.transform.localPosition = new Vector3(pointer.x, -9f, pointer.y);
    }

    private void MovePointer()
    {
        for (var i = 0; i < 2; i++)
        {
            for (var x = 0; x < 10; x++)
            {
                GameObject.Find("Floor").GetComponent<FloorHighlight>().SetPointerPosition(new Vector2(i, x));
                if (IsPositionTaken() == false) return;
            }
        }
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
