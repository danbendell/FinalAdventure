using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AbilityBarItem : MenuBarItem
{

    private string _name;

    private Vector3 _scale = new Vector3(1f, 1f, 1f);
    private Vector2 _anchorMin = new Vector2(0f, 0f);
    private Vector2 _anchorMax = new Vector2(1f, 1f);

    public AbilityBarItem(GameObject item, string name, float bottom, float top)
    {
        Item = item;
        _name = name;

        item.transform.GetComponent<RectTransform>().offsetMin = new Vector2(0, bottom);
        item.transform.GetComponent<RectTransform>().offsetMax = new Vector2(15f, top);
        item.transform.GetComponent<RectTransform>().localScale = _scale;
        item.transform.GetComponent<RectTransform>().anchorMin = _anchorMin;
        item.transform.GetComponent<RectTransform>().anchorMax = _anchorMax;
        item.transform.GetChild(0).GetComponent<Text>().text = _name;
    }

    public override void Selected()
    {
        base.Selected();

        GameObject.Find("AbilityBar").GetComponent<MenuBar>().State = MenuBar.States.Disabled;
    }
}
