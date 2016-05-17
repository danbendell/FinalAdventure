using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MoreInfo : MonoBehaviour
{
    public GameObject MoreInfoGameObject;

    public Text JobText;

    public Text AbilityTextOne;
    public Text AbilityTextTwo;
    public Text AbilityTextThree;

    public Text SpellTextOne;
    public Text SpellTextTwo;
    public Text SpellTextThree;

    private List<Text> _abilityList = new List<Text>(); 
    private List<Text> _spellList = new List<Text>(); 

	// Use this for initialization
	void Start () {
        _abilityList.Add(AbilityTextOne);
        _abilityList.Add(AbilityTextTwo);
        _abilityList.Add(AbilityTextThree);

        _spellList.Add(SpellTextOne);
        _spellList.Add(SpellTextTwo);
        _spellList.Add(SpellTextThree);

    }
	
	// Update is called once per frame
	void Update ()
	{

	    CarouselController CC = GameObject.Find("CharacterCarousel").GetComponent<CarouselController>();
        CarouselCharacterHolder CCH = CC.CurrentCharacter.GetComponent<CarouselCharacterHolder>();

	    JobText.text = CCH.Job.ToString();

	    foreach (var abilityText in _abilityList)
	    {
	        abilityText.text = "";
	    }

        foreach (var spellText in _spellList)
        {
            spellText.text = "";
        }

        for (var i = 0; i < CCH.Character.Abilities.Count; i++)
	    {
	        _abilityList[i].text = CCH.Character.Abilities[i].Name +  " - " + CCH.Character.Abilities[i].Desc;
	    }

        for (var i = 0; i < CCH.Character.Spells.Count; i++)
        {
            _spellList[i].text = CCH.Character.Spells[i].Name + " - " + CCH.Character.Spells[i].Desc;
        }

    }
}
