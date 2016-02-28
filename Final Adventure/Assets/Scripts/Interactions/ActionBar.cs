using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

public class ActionBar : MonoBehaviour
{

    private List<ActionBarItem> _actionBarMenu;
    public bool Enabled = true;
     
	// Use this for initialization
	void Start () {
        _actionBarMenu = new List<ActionBarItem>();

        for (var i = 0; i < transform.childCount; i++)
	    {
            ActionBarItem item = new ActionBarItem(transform.GetChild(i).gameObject, (ActionBarItem.Actions) i);
            _actionBarMenu.Add(item);
        }

	    if(_actionBarMenu.Count > 0) _actionBarMenu[0].Active = true;
	}
	
	// Update is called once per frame
	void Update () {

	    if (Input.GetKeyDown(KeyCode.Return))
	    {
	        if (Enabled)
	        {
	            ActionBarItemSelected();
	            Enabled = false;
	        }
	    }

	    if (Input.GetKeyDown(KeyCode.RightShift))
	    {
	        if (!Enabled)
	        {
	            GameObject.Find("Characters").GetComponent<CharactersController>().CancelAction();
	            Enabled = true;
	        }
	    }

	    if (Input.GetKeyDown(KeyCode.DownArrow))
	    {
	        if(Enabled) HighlightNextMenuItem();
	    }

	    if (Input.GetKeyDown(KeyCode.UpArrow))
	    {
            if (Enabled) HighlightPreviousMenuItem();
        }

	    AnimateActionBar();

	    for (var i = 0; i < _actionBarMenu.Count; i++)
	    {
	        _actionBarMenu[i].Update();
	    }
	}

    private void AnimateActionBar()
    {
        if (Enabled == false) Animate(-138f, -130f);
        else Animate(-8f, -20f);
    }

    private void Animate(float left, float right)
    {
        transform.GetComponent<RectTransform>().offsetMax =
                Vector2.Lerp(transform.GetComponent<RectTransform>().offsetMax,
                    new Vector2(right, transform.GetComponent<RectTransform>().offsetMax.y), 5 * Time.deltaTime);
        transform.GetComponent<RectTransform>().offsetMin =
            Vector2.Lerp(transform.GetComponent<RectTransform>().offsetMin,
                new Vector2(left, transform.GetComponent<RectTransform>().offsetMin.y), 5 * Time.deltaTime);
    }

    private void HighlightNextMenuItem()
    {
        for (var i = 0; i < _actionBarMenu.Count; i++)
        {
            if (_actionBarMenu[i].Active)
            {
                if (i < _actionBarMenu.Count - 1)
                {
                    _actionBarMenu[i].Active = false;
                    _actionBarMenu[i + 1].Active = true;
                    return;
                }
            }
        }

    }

    private void HighlightPreviousMenuItem()
    {
        for (var i = 0; i < _actionBarMenu.Count; i++)
        {
            if (_actionBarMenu[i].Active)
            {
                if (i > 0)
                {
                    _actionBarMenu[i].Active = false;
                    _actionBarMenu[i - 1].Active = true;
                    return;
                }
            }
        }
    }

    private void ActionBarItemSelected()
    {
        for (var i = 0; i < _actionBarMenu.Count; i++)
        {
            if (_actionBarMenu[i].Active)
            {
                _actionBarMenu[i].Selected();
                break;
            }
        }
    }
}
