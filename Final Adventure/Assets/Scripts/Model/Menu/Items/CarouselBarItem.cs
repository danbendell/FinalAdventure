using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CarouselBarItem : MenuBarItem
{


    public Options Option;

    public enum Options
    {
        Select,
        Info,
        None
    }

    public CarouselBarItem(GameObject item, string name, float bottom, float top)
    {
        Item = item;
        Name = name;
        Option = (Options)Enum.Parse(typeof(Options), name);
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
        switch (Option)
        {
            //case Options.Attack:
            //    if (turn.CompletedAction) DisableItem();
            //    else EnableItem();
            //    break;
            //case Options.Move:
            //    if (turn.Moved) DisableItem();
            //    else EnableItem();
            //    break;
            //case Options.Magic:
            //    if (turn.CompletedAction) DisableItem();
            //    else EnableItem();
            //    break;
            //case Options.Wait:
            //    if (turn.CompletedAction && turn.Moved) DisableItem();
            //    else EnableItem();
            //    break;
        }
    }

    public override void Selected()
    {
        base.Selected();

        Item.transform.parent.GetComponent<CarouselBar>().Option = Option;
        if (Disabled()) return;
        if (GameObject.Find("CarouselBar").GetComponent<CarouselBar>().State != MenuBar.States.Enabled) return;

        switch (Option)
        {
            case Options.Select:
                break;
            case Options.Info:
                break;
        }
    }

}
