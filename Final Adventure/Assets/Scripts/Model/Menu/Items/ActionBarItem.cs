using UnityEngine;
using System.Collections;
using System.ComponentModel;
using UnityEngine.UI;

public class ActionBarItem : MenuBarItem
{
    public Actions Action;
    
    public enum Actions
    {
        Attack,
        Move,
        Magic,
        None
    }

    public ActionBarItem(GameObject item, Actions actions)
    {
        Item = item;
        Action = actions;
        Active = false;
    }

    public override void Selected()
    {
        base.Selected();

        Item.transform.parent.GetComponent<ActionBar>().Action = Action;
        switch (Action)
        {
            case Actions.Attack:
                GameObject.Find("Characters").GetComponent<CharactersController>().HighlightCharacterAttackRange();
                GameObject.Find("ActionBar").GetComponent<MenuBar>().State = MenuBar.States.Disabled;
                break;
            case Actions.Move:
                GameObject.Find("Characters").GetComponent<CharactersController>().HighlightCharacterMovement();
                GameObject.Find("ActionBar").GetComponent<MenuBar>().State = MenuBar.States.Disabled;
                break;
            case Actions.Magic:
                GameObject.Find("ActionBar").GetComponent<MenuBar>().State = MenuBar.States.Hidden;
                GameObject.Find("AbilityBar").GetComponent<MenuBar>().State = MenuBar.States.Enabled;
                break;
        }
    }

}
