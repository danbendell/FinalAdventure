using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine.UI;

public abstract class MenuBar : MonoBehaviour {

    public List<MenuBarItem> Items = new List<MenuBarItem>();
    public GameObject ParentScrollBar;

    protected float EnabledXPosition = -81f;
    protected float DisabledXPosition = 53f;
    protected float HiddenXPosition = 120f;

    protected float ScrollPosition = 1;
    protected float ScrollPercent = 1;

    private int _selectedPosition = 1;
    private int _upperBound = 1;
    private int _lowerBound = 4;

    public bool Animating = false;

    public States State;
    public enum States
    {
        Enabled,
        Disabled,
        Hidden
    }

    // Use this for initialization
    public void Init () {

        DisabledXPosition = ParentScrollBar.transform.GetComponent<RectTransform>().anchoredPosition3D.x +
                              (ParentScrollBar.transform.GetComponent<RectTransform>().sizeDelta.x * 0.65f);
        EnabledXPosition = ParentScrollBar.transform.GetComponent<RectTransform>().anchoredPosition3D.x;
        HiddenXPosition = ParentScrollBar.transform.GetComponent<RectTransform>().anchoredPosition3D.x +
                          ParentScrollBar.transform.GetComponent<RectTransform>().sizeDelta.x;
    }
	
	// Update is called once per frame
	public void UpdateMenu ()
	{

	    if (GameObject.Find("Canvas").GetComponent<GamePause>().isPaused) return;
	    if (GameObject.Find("Canvas").GetComponent<GamePause>().isEndGame) return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (State == States.Enabled)
            {
                ActionBarItemSelected();
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (State == States.Enabled) HighlightNextMenuItem();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (State == States.Enabled) HighlightPreviousMenuItem();
        }

	    Scroll();

        AnimateMenuBar();

    }

    public void Refresh()
    {
        ScrollPosition = 1;
        ScrollPercent = 1;

        _selectedPosition = 1;
        _upperBound = 1;
        _lowerBound = 4;
    }

    private void Scroll()
    {
        if (State != States.Enabled) return;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            for (int i = _selectedPosition; i < Items.Count; i++)
            {
                ScrollUp();
                if (Items[i].Active) break;
            }
            if(_selectedPosition == Items.Count) ScrollUp();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            for (int i = _selectedPosition; i < Items.Count; i++)
            {
                ScrollDown();
                if (Items[i].Active) break;
            }
        }

        ParentScrollBar.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, ScrollPosition);
    }

    private void ScrollUp()
    {
        _selectedPosition--;
        if (_selectedPosition < 1) _selectedPosition = 1;
        if (ScrollPosition < 1)
        {
            if (_selectedPosition == _upperBound)
            {
                _lowerBound--;
                _upperBound--;
                ScrollPosition += ScrollPercent;
            }
        }
    }

    private void ScrollDown()
    {
        _selectedPosition++;

        if (_selectedPosition > Items.Count) _selectedPosition = Items.Count;
        if (ScrollPosition > 0)
        {
            if (_selectedPosition == _lowerBound)
            {
                _lowerBound++;
                _upperBound++;
                ScrollPosition -= ScrollPercent;
            }
        }
    }

    private void AnimateMenuBar()
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

        AnimateItems();
    }

    private void AnimateItems()
    {
        foreach (MenuBarItem item in Items)
        {
            item.Update();
        }
    }

    private void Animate(float x)
    {
        Animating = false;
        if (Math.Abs(ParentScrollBar.GetComponent<RectTransform>().anchoredPosition3D.x - x) > 50f)
        {
            Animating = true;
        }
        ParentScrollBar.GetComponent<RectTransform>().anchoredPosition3D =
        Vector3.Lerp(ParentScrollBar.GetComponent<RectTransform>().anchoredPosition3D, new Vector3(x, ParentScrollBar.GetComponent<RectTransform>().anchoredPosition3D.y, ParentScrollBar.GetComponent<RectTransform>().anchoredPosition3D.z), 5 * Time.deltaTime);
    }

    public virtual void Show()
    {
        State = States.Enabled;
    }

    public void HighlightNextMenuItem()
    {
        for (var i = 0; i < Items.Count; i++)
        {
            if (!Items[i].Active) continue;
            if (i >= Items.Count - 1) continue;

            Items[i].Active = false;
            for (var x = i + 1; x < Items.Count; x++)
            {
                if (Items[x].Disabled()) continue;
                Items[x].Active = true;
                break;
            }
            return;
        }
    }

    public void HighlightPreviousMenuItem()
    {
        for (var i = 0; i < Items.Count; i++)
        {
            if (!Items[i].Active) continue;
            if (i <= 0) continue;

            Items[i].Active = false;
            for (var x = i - 1; x >= 0; x--)
            {
                if (Items[x].Disabled()) continue;
                Items[x].Active = true;
                break;
            }
            return;
        }
    }


    public void ActionBarItemSelected()
    {
        for (var i = 0; i < Items.Count; i++)
        {
            if (Items[i].Active)
            {
                Items[i].Selected();
                break;
            }
        }
    }
}
