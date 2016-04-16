using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SubMenuBarItem : MenuBarItem
{

    public virtual void Create(GameObject item, string name, float bottom, float top)
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
}
