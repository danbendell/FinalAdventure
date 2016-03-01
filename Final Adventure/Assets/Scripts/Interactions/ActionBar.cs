using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

public class ActionBar : MenuBar
{

    public ActionBarItem.Actions Action = ActionBarItem.Actions.None;
     
	// Use this for initialization
	void Start () {
        State = States.Enabled;

        DisabledXPosition = transform.GetComponent<RectTransform>().anchoredPosition3D.x +
                         (transform.GetComponent<RectTransform>().sizeDelta.x * 0.81f);
        EnabledXPosition = transform.GetComponent<RectTransform>().anchoredPosition3D.x;
        HiddenXPosition = transform.GetComponent<RectTransform>().anchoredPosition3D.x +
                          (transform.GetComponent<RectTransform>().sizeDelta.x * 1.2f);

        for (var i = 0; i < transform.childCount; i++)
	    {
            ActionBarItem item = new ActionBarItem(transform.GetChild(i).gameObject, (ActionBarItem.Actions) i);
            Items.Add(item);
        }

	    if(Items.Count > 0) Items[0].Active = true;
	}
	
	// Update is called once per frame
    void Update()
    {
        UpdateMenu();

        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            if (State == States.Enabled) return;

            GameObject.Find("Characters").GetComponent<CharactersController>().CancelAction();
            Action = ActionBarItem.Actions.None;
            if(State == States.Disabled) State = States.Enabled;
            
        }
    }
}
