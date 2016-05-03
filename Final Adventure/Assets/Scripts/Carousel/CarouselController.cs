using UnityEngine;
using System.Collections;
using Assets.Scripts.Model;
using UnityEngine.UI;

public class CarouselController : MonoBehaviour
{

    public GameObject CurrentCharacter;

	// Use this for initialization
	void Start ()
	{
	    
	}
	
	// Update is called once per frame
	void Update () {
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
	}
}
