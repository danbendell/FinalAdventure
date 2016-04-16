using System;
using UnityEngine;
using System.Collections;

public class MagicBarItem : SubMenuBarItem
{
    public Spells Spell;

    public enum Spells
    {
        Heal,
        Flare,
        Wind,
        Aqua,
        Earth,
        None
    }

    public MagicBarItem(GameObject item, string name, float bottom, float top)
    {
        base.Create(item, name, bottom, top);
        Spell = (Spells) Enum.Parse(typeof(Spells), name); ;
    }

    public override void Selected()
    {
        base.Selected();

        Item.transform.parent.GetComponent<MagicBar>().Spell = Spell;
        if (Disabled()) return;
        if (GameObject.Find("MagicBar").GetComponent<MagicBar>().State != MenuBar.States.Enabled) return;

        switch (Spell)
        {
            case Spells.Heal:
                GameObject.Find("Characters").GetComponent<CharactersController>().HighlightCharacterAttackRange();
                GameObject.Find("MagicBar").GetComponent<MagicBar>().State = MenuBar.States.Disabled;
                break;
            case Spells.Flare:
                GameObject.Find("Characters").GetComponent<CharactersController>().HighlightCharacterAttackRange();
                GameObject.Find("MagicBar").GetComponent<MagicBar>().State = MenuBar.States.Disabled;
                break;
            case Spells.Wind:
                break;
            case Spells.Aqua:
                break;
            case Spells.Earth:
                break;
        }
    }
}
