using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Assets.Scripts.Damage;
using UnityEngine.UI;
using Flare = Assets.Scripts.Damage.Flare;

public class AbilityBar : SubMenuBar
{

    public AbilityBarItem.Abilities Ability = AbilityBarItem.Abilities.None;
    private List<Ability> _abilities = new List<Ability>();
    
    // Use this for initialization
    void Start ()
    {
        Init(_abilities);

        Create();

        if (Items.Count > 0) Items[0].Active = true;

        ScrollPercent = 1 / (ItemCount - 4);

        ParentScrollBar.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, ScrollPosition);
    }
	
	// Update is called once per frame
	void Update () {
        CharactersController CC = GameObject.Find("Characters").GetComponent<CharactersController>();
        if (CC.CurrentCharacterHolder.IsAi) return;

        UpdateSubMenu();

	    KeyboardInput();

	}

    private void Create()
    {
        CharactersController CC = GameObject.Find("Characters").GetComponent<CharactersController>();
        CharacterHolder CH = CC.CurrentCharacterHolder;

        float parentHeight = transform.GetComponent<RectTransform>().sizeDelta.y;

        for (var i = 0; i < _abilities.Count; i++)
        {
            GameObject abilityBarItem = (GameObject) Instantiate(Resources.Load("UIBarItem"));
            abilityBarItem.transform.SetParent(transform);
            abilityBarItem.name = _abilities[i].Name;

            float bottom = parentHeight - ((ItemHeight * (i + 1)) + (ItemGap * i));
            float top = -(ItemHeight * i) - (ItemGap * i);
            AbilityBarItem ABI = new AbilityBarItem(abilityBarItem, _abilities[i].Name, _abilities[i].Cost.ToString(), bottom, top);
            Items.Add(ABI);
        }

        if (Items.Count > 0) Items[0].Active = true;

        ScrollPercent = 1 / (float)(_abilities.Count - 4);
    }

    public void Refresh(List<Ability> abilities)
    {
        base.Refresh();
        ClearMenuBar();
        _abilities = abilities;
        Create();
    }

    public void DisableAction()
    {
        foreach (AbilityBarItem item in Items)
        {
            if (item.Ability == Ability) item.DisableItem();
        }
    }

    private void KeyboardInput()
    {
        
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            if (State == States.Disabled)
            {
                State = States.Enabled;
                Ability = AbilityBarItem.Abilities.None;
                if (State == States.Disabled) State = States.Enabled;
            }
            else if (State == States.Enabled)
            {
                State = States.Hidden;
                Ability = AbilityBarItem.Abilities.None;
                GameObject.Find("ActionBar").GetComponent<MenuBar>().State = MenuBar.States.Enabled;
            }

        }

    }
}
