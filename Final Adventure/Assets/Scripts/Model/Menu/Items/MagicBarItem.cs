using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Damage;
using Flare = Assets.Scripts.Damage.Flare;

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

    public override void Update()
    {
        base.Update();
        var mana = GameObject.Find("Characters").GetComponent<CharactersController>().CurrentCharacterHolder.Character.Mana;
        switch (Spell)
        {
            case Spells.Heal:
                Heal heal = new Heal();
                if (mana < heal.Cost) DisableItem();
                else EnableItem();
                break;
            case Spells.Flare:
                Flare flare = new Flare();
                if (mana < flare.Cost) DisableItem();
                else EnableItem();
                break;
            case Spells.Wind:
                break;
            case Spells.Aqua:
                break;
            case Spells.Earth:
                break;
        }
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
