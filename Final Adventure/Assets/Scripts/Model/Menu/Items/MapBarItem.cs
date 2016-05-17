using System;
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using Assets.Scripts.Damage;
using Assets.Scripts.Damage.Abilities;
using UnityEngine.UI;

public class MapBarItem : SubMenuBarItem
{
    public Options Option;

    public enum Options
    {
        Place,
        Remove,
        None
    }

    public MapBarItem(GameObject item, string name, float bottom, float top)
    {
        base.Create(item, name, "", bottom, top);
        Option = (Options)Enum.Parse(typeof(Options), name);
    }

    public override void Update()
    {
        base.Update();
        switch (Option)
        {
            case Options.Place:
                if (GameObject.Find("CharacterCarousel").GetComponent<CarouselController>().IsPositionTaken()) DisableItem();
                else if (GameObject.Find("CharacterCarousel").GetComponent<CarouselController>().ChosenCharacters.Count == 5) DisableItem();
                else EnableItem();
                break;
            case Options.Remove:
                if (Active) GameObject.Find("CharacterCarousel").GetComponent<CarouselController>().RemoveCharacter();
                if (!GameObject.Find("CharacterCarousel").GetComponent<CarouselController>().IsPositionTaken()) DisableItem();
                else EnableItem();
                break;
        }
    }

    public override void Selected()
    {
        base.Selected();

        Item.transform.parent.GetComponent<MapBar>().Option = Option;
        if (Disabled()) return;
        if (GameObject.Find("MapBar").GetComponent<MapBar>().State != MenuBar.States.Enabled) return;

        switch (Option)
        {
            case Options.Place:
                GameObject.Find("CharacterCarousel").GetComponent<CarouselController>().SavePlayer();
                break;
            case Options.Remove:
                GameObject.Find("CharacterCarousel").GetComponent<CarouselController>().RemoveTargetCharacter();
                break;
        }
    }
}
