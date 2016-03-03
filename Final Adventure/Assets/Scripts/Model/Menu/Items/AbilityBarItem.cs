using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AbilityBarItem : MenuBarItem
{
    public Actions Action;

    public enum Actions
    {
        Heal,
        Flare,
        Wind,
        Aqua,
        Earth,
        None
    }

    public AbilityBarItem(GameObject item, string name, float bottom, float top)
    {
        Item = item;
        Name = name;

        item.transform.GetComponent<RectTransform>().offsetMin = new Vector2(0, bottom);
        item.transform.GetComponent<RectTransform>().offsetMax = new Vector2(15f, top);
        item.transform.GetComponent<RectTransform>().localScale = Scale;
        item.transform.GetComponent<RectTransform>().anchorMin = AnchorMin;
        item.transform.GetComponent<RectTransform>().anchorMax = AnchorMax;
        item.transform.GetChild(0).GetComponent<Text>().text = Name;
    }

    public override void Selected()
    {
        base.Selected();

        Item.transform.parent.GetComponent<AbilityBar>().Action = Action;
        if (Disabled()) return;
        if (GameObject.Find("AbilityBar").GetComponent<AbilityBar>().State != MenuBar.States.Enabled) return;

        switch (Action)
        {
            case Actions.Heal:
                GameObject.Find("Characters").GetComponent<CharactersController>().HighlightCharacterAttackRange();
                GameObject.Find("AbilityBar").GetComponent<AbilityBar>().State = MenuBar.States.Disabled;
                break;
            case Actions.Flare:
                break;
            case Actions.Wind:
                break;
            case Actions.Aqua:
                break;
            case Actions.Earth:
                break;
        }
    }
}
