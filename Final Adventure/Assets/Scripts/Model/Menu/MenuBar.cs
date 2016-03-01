using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class MenuBar : MonoBehaviour {

    public List<MenuBarItem> Items = new List<MenuBarItem>();

    protected float EnabledXPosition = -81f;
    protected float DisabledXPosition = 53f;
    protected float HiddenXPosition = 120f;

    public States State;
    public enum States
    {
        Enabled,
        Disabled,
        Hidden
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	public void UpdateMenu () {

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

        AnimateMenuBar();

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
        transform.GetComponent<RectTransform>().anchoredPosition3D =
                Vector3.Lerp(transform.GetComponent<RectTransform>().anchoredPosition3D, new Vector3(x, transform.GetComponent<RectTransform>().anchoredPosition3D.y, transform.GetComponent<RectTransform>().anchoredPosition3D.z), 5 * Time.deltaTime);
    }

    public void HighlightNextMenuItem()
    {
        for (var i = 0; i < Items.Count; i++)
        {
            if (Items[i].Active)
            {
                if (i < Items.Count - 1)
                {
                    Items[i].Active = false;
                    Items[i + 1].Active = true;
                    return;
                }
            }
        }

    }

    public void HighlightPreviousMenuItem()
    {
        for (var i = 0; i < Items.Count; i++)
        {
            if (Items[i].Active)
            {
                if (i > 0)
                {
                    Items[i].Active = false;
                    Items[i - 1].Active = true;
                    return;
                }
            }
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
