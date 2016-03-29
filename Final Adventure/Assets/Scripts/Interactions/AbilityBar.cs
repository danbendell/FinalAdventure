using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine.UI;

public class AbilityBar : MenuBar
{

    public AbilityBarItem.Actions Action = AbilityBarItem.Actions.None;

    private List<string> _abilities = new List<string> { "Heal", "Flare", "Wind", "Aqua", "Earth" };
    private float _itemHeight = 45.5f;
    private float _itemGap = 2f;


    // Use this for initialization
    void Start ()
    {

        Init();

        State = States.Hidden;

	    AdjustListHeight();

        float parentHeight = transform.GetComponent<RectTransform>().sizeDelta.y;

	    for (var i = 0; i < _abilities.Count; i++)
	    {
            GameObject abilityBarItem = (GameObject)Instantiate(Resources.Load("UIBarItem"));
            abilityBarItem.transform.SetParent(transform);
            abilityBarItem.name = _abilities[i];

            float bottom = parentHeight - ((_itemHeight * (i + 1)) + (_itemGap * i));
            float top = -(_itemHeight * i) - (_itemGap * i);

            AbilityBarItem ABI = new AbilityBarItem(abilityBarItem, _abilities[i], (AbilityBarItem.Actions) i, bottom, top);
            Items.Add(ABI);
        }

        if (Items.Count > 0) Items[0].Active = true;

        ScrollPercent = 1 / (float) (_abilities.Count - 4);

        ParentScrollBar.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, ScrollPosition);

    }
	
	// Update is called once per frame
	void Update () {

        UpdateMenu();

	    KeyboardInput();
       
        
    }

    public void DisableAction()
    {
        foreach (AbilityBarItem item in Items)
        {
            if (item.Action == Action) item.DisableItem();
        }
    }

    private void KeyboardInput()
    {
        
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            if (State == States.Disabled)
            {
                State = States.Enabled;
                Action = AbilityBarItem.Actions.None;
                if (State == States.Disabled) State = States.Enabled;
            }
            else if (State == States.Enabled)
            {
                State = States.Hidden;
                Action = AbilityBarItem.Actions.None;
                GameObject.Find("ActionBar").GetComponent<MenuBar>().State = MenuBar.States.Enabled;
            }

        }

    }


    private void AdjustListHeight()
    {
        float combinedItemHeight = _abilities.Count*(_itemHeight + _itemGap);

        if (combinedItemHeight < transform.GetComponent<RectTransform>().sizeDelta.y) return;

        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(transform.GetComponent<RectTransform>().sizeDelta.x, combinedItemHeight);
    }


}
