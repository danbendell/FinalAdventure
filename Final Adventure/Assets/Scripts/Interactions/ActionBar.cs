using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

public class ActionBar : MonoBehaviour
{

    private List<ActionBarItem> ActionBarMenu;

    enum Direction
    {
        Up,
        Down
    }
     
	// Use this for initialization
	void Start () {
        ActionBarMenu = new List<ActionBarItem>();

        for (var i = 0; i < transform.childCount; i++)
	    {
            ActionBarItem item = new ActionBarItem(transform.GetChild(i).gameObject);
            ActionBarMenu.Add(item);
        }

	    if(ActionBarMenu.Count > 0) ActionBarMenu[0].Active = true;
	}
	
	// Update is called once per frame
	void Update () {

	    if (Input.GetKeyDown(KeyCode.DownArrow))
	    {
	        HighlightNextMenuItem(Direction.Down);
	    }

	    if (Input.GetKeyDown(KeyCode.UpArrow))
	    {
            HighlightNextMenuItem(Direction.Up);
        }
	}

    private void HighlightNextMenuItem(Direction direction)
    {
        if (direction == Direction.Down)
        {
            HighlightNextMenuItem();
        }
        else if (direction == Direction.Up)
        {
            HighlightPreviousMenuItem();
        }
    }

    private void HighlightNextMenuItem()
    {
        for (var i = 0; i < ActionBarMenu.Count; i++)
        {
            if (ActionBarMenu[i].Active)
            {
                if (i < ActionBarMenu.Count - 1)
                {
                    ActionBarMenu[i].Active = false;
                    ActionBarMenu[i + 1].Active = true;
                    return;
                }
            }
        }

    }

    private void HighlightPreviousMenuItem()
    {
        for (var i = 0; i < ActionBarMenu.Count; i++)
        {
            if (ActionBarMenu[i].Active)
            {
                if (i > 0)
                {
                    ActionBarMenu[i].Active = false;
                    ActionBarMenu[i - 1].Active = true;
                    return;
                }
            }
        }
    }
}
