using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AbilityBarItem : SubMenuBarItem
{
    public Abilities Ability;

    public enum Abilities
    {
        Focus,
        None
    }

    public AbilityBarItem(GameObject item, string name, float bottom, float top)
    {
        base.Create(item, name, bottom, top);
        Ability = (Abilities) Enum.Parse(typeof(Abilities), name);
    }

    public override void Selected()
    {
        base.Selected();

        Item.transform.parent.GetComponent<AbilityBar>().Ability = Ability;
        if (Disabled()) return;
        if (GameObject.Find("AbilityBar").GetComponent<AbilityBar>().State != MenuBar.States.Enabled) return;

        switch (Ability)
        {
            case Abilities.Focus:
                GameObject.Find("Characters").GetComponent<CharactersController>().HighlightCharacterAttackRange();
                GameObject.Find("AbilityBar").GetComponent<AbilityBar>().State = MenuBar.States.Disabled;
                break;
        }
    }
}
