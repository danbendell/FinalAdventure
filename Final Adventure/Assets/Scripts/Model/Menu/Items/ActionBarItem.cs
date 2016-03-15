using UnityEngine;
using System.Collections;
using System.ComponentModel;
using System.Runtime.InteropServices;
using UnityEngine.UI;

public class ActionBarItem : MenuBarItem
{

    public Actions Action;

    public enum Actions
    {
        Attack,
        Move,
        Magic,
        Wait,
        None
    }

    public ActionBarItem(GameObject item, Actions action, float bottom, float top)
    {
        Item = item;
        Name = action.ToString();
        Action = action;
        Active = false;

        item.transform.GetComponent<RectTransform>().offsetMin = new Vector2(0, bottom);
        item.transform.GetComponent<RectTransform>().offsetMax = new Vector2(15f, top);
        item.transform.GetComponent<RectTransform>().localScale = Scale;
        item.transform.GetComponent<RectTransform>().anchorMin = AnchorMin;
        item.transform.GetComponent<RectTransform>().anchorMax = AnchorMax;
        item.transform.GetChild(0).GetComponent<Text>().text = Name;


    }

    public override void Update()
    {
        base.Update();
        Turn turn = GameObject.Find("Characters").GetComponent<CharactersController>().CurrentCharacterHolder.Turn;
        switch (Action)
        {
            case Actions.Attack:
                if(turn.CompletedAction) DisableItem();
                else EnableItem();
                break;
            case Actions.Move:
                if (turn.Moved) DisableItem();
                else EnableItem();
                break;
            case Actions.Magic:
                if (turn.CompletedAction) DisableItem();
                else EnableItem();
                break;
            case Actions.Wait:
                if(turn.CompletedAction && turn.Moved) DisableItem();
                else EnableItem();
                break;
        }
    }

    public override void Selected()
    {
        base.Selected();

        Item.transform.parent.GetComponent<ActionBar>().Action = Action;
        if(Disabled()) return;
        if (GameObject.Find("ActionBar").GetComponent<ActionBar>().State != MenuBar.States.Enabled) return;

        switch (Action)
        {
            case Actions.Attack:
                GameObject.Find("Characters").GetComponent<CharactersController>().HighlightCharacterAttackRange();
                GameObject.Find("ActionBar").GetComponent<ActionBar>().State = MenuBar.States.Disabled;
                break;
            case Actions.Move:
                GameObject.Find("Characters").GetComponent<CharactersController>().HighlightCharacterMovement();
                GameObject.Find("ActionBar").GetComponent<ActionBar>().State = MenuBar.States.Disabled;
                break;
            case Actions.Magic:
                GameObject.Find("ActionBar").GetComponent<ActionBar>().State = MenuBar.States.Hidden;
                GameObject.Find("AbilityBar").GetComponent<AbilityBar>().Action = AbilityBarItem.Actions.None;
                GameObject.Find("AbilityBar").GetComponent<AbilityBar>().State = MenuBar.States.Enabled;
                break;
            case Actions.Wait:
                GameObject.Find("Characters").GetComponent<CharactersController>().CurrentCharacterHolder.Turn.Skip();
                break;
        }
    }

}
