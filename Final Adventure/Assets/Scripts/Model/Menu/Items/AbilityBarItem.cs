using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Damage;
using Assets.Scripts.Damage.Abilities;
using UnityEngine.UI;

public class AbilityBarItem : SubMenuBarItem
{
    public Abilities Ability;

    public enum Abilities
    {
        Focus,
        Slash,
        Assassinate,
        BloodBlade,
        None
    }

    public AbilityBarItem(GameObject item, string name, string cost, float bottom, float top)
    {
        base.Create(item, name, cost, bottom, top);
        Ability = (Abilities) Enum.Parse(typeof(Abilities), name);
    }

    public override void Update()
    {
        base.Update();
        var mana = GameObject.Find("Characters").GetComponent<CharactersController>().CurrentCharacterHolder.Character.Mana;
        switch (Ability)
        {
            case Abilities.Focus:
                Focus focus = new Focus();
                if (mana < focus.Cost) DisableItem();
                else EnableItem();
                break;
            case Abilities.Slash:
                Slash slash = new Slash();
                if (mana < slash.Cost) DisableItem();
                else EnableItem();
                break;
            case Abilities.Assassinate:
                Assassinate assassinate = new Assassinate();
                if (mana < assassinate.Cost) DisableItem();
                else EnableItem();
                break;
            case Abilities.BloodBlade:
                BloodBlade bloodBlade = new BloodBlade();
                if (mana < bloodBlade.Cost) DisableItem();
                else EnableItem();
                break;
        }
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
            case Abilities.Slash:
                GameObject.Find("Characters").GetComponent<CharactersController>().HighlightCharacterAttackRange();
                GameObject.Find("AbilityBar").GetComponent<AbilityBar>().State = MenuBar.States.Disabled;
                break;
            case Abilities.Assassinate:
                GameObject.Find("Characters").GetComponent<CharactersController>().HighlightCharacterAttackRange();
                GameObject.Find("AbilityBar").GetComponent<AbilityBar>().State = MenuBar.States.Disabled;
                break;
            case Abilities.BloodBlade:
                GameObject.Find("Characters").GetComponent<CharactersController>().HighlightCharacterAttackRange();
                GameObject.Find("AbilityBar").GetComponent<AbilityBar>().State = MenuBar.States.Disabled;
                break;
        }
    }
}
