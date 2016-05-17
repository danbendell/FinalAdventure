using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MagicBar : SubMenuBar
{

    public MagicBarItem.Spells Spell = MagicBarItem.Spells.None;
    private List<Spell> _spells = new List<Spell>();

    // Use this for initialization
    void Start () {
        Init(_spells);

        Create();

        if (Items.Count > 0) Items[0].Active = true;

        ScrollPercent = 1 / (ItemCount - 4);

        ParentScrollBar.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, ScrollPosition);
    }

    void Update()
    {
        CharactersController CC = GameObject.Find("Characters").GetComponent<CharactersController>();
        if (CC.CurrentCharacterHolder.IsAi) return;

        UpdateSubMenu();

        KeyboardInput();

    }

    private void Create()
    {
        CharactersController CC = GameObject.Find("Characters").GetComponent<CharactersController>();
        CharacterHolder CH = CC.CurrentCharacterHolder;

        float parentHeight = transform.GetComponent<RectTransform>().sizeDelta.y;

        for (var i = 0; i < _spells.Count; i++)
        {
            GameObject magicBarItem = (GameObject)Instantiate(Resources.Load("UIBarItem"));
            magicBarItem.transform.SetParent(transform);
            magicBarItem.name = _spells[i].Name;

            float bottom = parentHeight - ((ItemHeight * (i + 1)) + (ItemGap * i));
            float top = -(ItemHeight * i) - (ItemGap * i);
            string itemName = _spells[i].Name + "  " + _spells[i].Cost;
            MagicBarItem ABI = new MagicBarItem(magicBarItem, _spells[i].Name, _spells[i].Cost.ToString(), bottom, top);
            Items.Add(ABI);
        }

        if (Items.Count > 0) Items[0].Active = true;

        ScrollPercent = 1 / (float)(_spells.Count - 4);
    }

    public void Refresh(List<Spell> spells)
    {
        base.Refresh();
        ClearMenuBar();
        _spells = spells;
        Create();
    }

    public void DisableAction()
    {
        foreach (MagicBarItem item in Items)
        {
            if (item.Spell == Spell) item.DisableItem();
        }
    }

    private void KeyboardInput()
    {

        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            if (State == States.Disabled)
            {
                State = States.Enabled;
                Spell = MagicBarItem.Spells.None;
                if (State == States.Disabled) State = States.Enabled;
            }
            else if (State == States.Enabled)
            {
                State = States.Hidden;
                Spell = MagicBarItem.Spells.None;
                GameObject.Find("ActionBar").GetComponent<MenuBar>().State = MenuBar.States.Enabled;
            }

        }

    }
}
