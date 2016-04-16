using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuController : MonoBehaviour {

    public List<GameObject> Menus = new List<GameObject>();

    public ActionBar ActionBar;
    public MagicBar MagicBar;
    public AbilityBar AbilityBar;

	// Use this for initialization
	void Start ()
	{
	    ActionBar = GameObject.Find("ActionBar").GetComponent<ActionBar>();
	    AbilityBar = GameObject.Find("AbilityBar").GetComponent<AbilityBar>();
	    MagicBar = GameObject.Find("MagicBar").GetComponent<MagicBar>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Refresh(CharacterHolder characterHolder)
    {
        List<string> actions = new List<string> { "Attack", "Move", "Magic", "Ability", "Wait" };
        List<Ability> abilitieses = characterHolder.Character.Abilities;
        List<Spell> spells = characterHolder.Character.Spells;

        if (abilitieses.Count == 0) actions.Remove("Ability");
        if (spells.Count == 0) actions.Remove("Magic");

        ActionBar.Refresh(actions);
        AbilityBar.Refresh(abilitieses);
        MagicBar.Refresh(spells);
    }


    public bool Animating()
    {
        return ActionBar.Animating || AbilityBar.Animating || MagicBar.Animating;
    }
}
