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
        Play,
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
            case Options.Select:
                break;
            case Options.Info:
                break;
            case Options.Play:
                if (GameObject.Find("CharacterCarousel").GetComponent<CarouselController>().ChosenCharacters.Count == 5) EnableItem();
                else DisableItem();
                break;
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
                GameObject.Find("CharacterCarousel").GetComponent<CarouselController>().PlaceCharacter();
                GameObject.Find("CarouselBar").GetComponent<CarouselBar>().State = MenuBar.States.Hidden;
                GameObject.Find("MapBar").GetComponent<MapBar>().State = MenuBar.States.Enabled;
                GameObject.Find("Stats").GetComponent<StatsBar>()._enabled = false;
                GameObject.Find("Info").GetComponent<Text>().text = "Place the selected character within the grey area\n" +
                                                                    "Press enter to confirm postion\n";
                GameObject.Find("Main Camera")
                    .GetComponent<MoveCamera>()
                    .MoveToMap();
                break;
            case Options.Info:
                break;
            case Options.Play:
                Application.LoadLevel("MainScene");
                break;
        }
    }

}
