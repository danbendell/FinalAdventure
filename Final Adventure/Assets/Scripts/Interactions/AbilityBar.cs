using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine.UI;

public class AbilityBar : MenuBar
{

    private List<string> _abilities = new List<string> { "Heal", "Flare", "Wind", "Aqua", "Earth" };
    private float _itemHeight = 45.5f;
    private float _itemGap = 2f;

    private float _scrollPosition = 1;
    private float _scrollPercent;

    private int _selectedPosition = 1;


    // Use this for initialization
    void Start ()
    {
        State = States.Hidden;

        DisabledXPosition = transform.parent.GetComponent<RectTransform>().anchoredPosition3D.x +
                          (transform.parent.GetComponent<RectTransform>().sizeDelta.x * 0.65f);
        EnabledXPosition = transform.parent.GetComponent<RectTransform>().anchoredPosition3D.x;
        HiddenXPosition = transform.parent.GetComponent<RectTransform>().anchoredPosition3D.x +
                          transform.parent.GetComponent<RectTransform>().sizeDelta.x;

	    AdjustListHeight();

        float parentHeight = transform.GetComponent<RectTransform>().sizeDelta.y;

	    for (var i = 0; i < _abilities.Count; i++)
	    {
            GameObject abilityBarItem = (GameObject)Instantiate(Resources.Load("UIBarItem"));
            abilityBarItem.transform.SetParent(transform);
            abilityBarItem.name = _abilities[i];
            float bottom = parentHeight - ((_itemHeight * (i + 1)) + (_itemGap * i));
            float top = -(_itemHeight * i) - (_itemGap * i);
            AbilityBarItem ABI = new AbilityBarItem(abilityBarItem, _abilities[i], bottom, top);
            Items.Add(ABI);
        }

        if (Items.Count > 0) Items[0].Active = true;

        _scrollPercent = 1 / (float) (_abilities.Count - 4);

        transform.parent.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, _scrollPosition);

    }
	
	// Update is called once per frame
	void Update () {

        UpdateMenu();
	    AnimateScrollView();

	    KeyboardInput();
       
        transform.parent.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, _scrollPosition);
    }

    private void KeyboardInput()
    {
        
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            if(State == States.Disabled) State = States.Enabled;
            else if (State == States.Enabled)
            {
                State = States.Hidden;
                GameObject.Find("ActionBar").GetComponent<MenuBar>().State = MenuBar.States.Enabled;
            }

        }

        if (State != States.Enabled) return;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (_scrollPosition < 1)
            {
                if (_selectedPosition == 1) _scrollPosition += _scrollPercent;
                if (_selectedPosition != 1) _selectedPosition--;
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (_scrollPosition > 0)
            {
                if (_selectedPosition == 3) _scrollPosition -= _scrollPercent;
                if (_selectedPosition != 3) _selectedPosition++;
            }
        }
    }

    private void AnimateScrollView()
    {
        switch (State)
        {
            case States.Enabled:
                Animate(EnabledXPosition);
                break;
            case States.Disabled:
                Animate(DisabledXPosition);
                break;
            case States.Hidden:
                Animate(HiddenXPosition);
                break;
        }
    }

    private void Animate(float x)
    {
        transform.parent.GetComponent<RectTransform>().anchoredPosition3D =
                Vector3.Lerp(transform.parent.GetComponent<RectTransform>().anchoredPosition3D, new Vector3(x, transform.parent.GetComponent<RectTransform>().anchoredPosition3D.y, transform.parent.GetComponent<RectTransform>().anchoredPosition3D.z), 5 * Time.deltaTime);
    }

    private void AdjustListHeight()
    {
        float combinedItemHeight = _abilities.Count*(_itemHeight + _itemGap);

        if (combinedItemHeight < transform.GetComponent<RectTransform>().sizeDelta.y) return;

        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(transform.GetComponent<RectTransform>().sizeDelta.x, combinedItemHeight);
    }


}
