using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using UnityEngine.UI;

public class ActionBar : MenuBar
{

    public ActionBarItem.Actions Action = ActionBarItem.Actions.None;

    private List<string> _actions = new List<string> { "Attack", "Move", "Magic", "Ability", "Wait" };
    private float _itemHeight = 45.5f;
    private float _itemGap = 2f;

    // Use this for initialization
    void Start () {

        Init();

        State = States.Enabled;

        AdjustListHeight();

        Create();

       
    }
	
	// Update is called once per frame
    void Update()
    {
        CharactersController CC = GameObject.Find("Characters").GetComponent<CharactersController>();
        if (CC.CurrentCharacterHolder.IsAi) return;

        UpdateMenu();
        UpdateActionBar();

        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            if (State == States.Enabled) return;

            GameObject.Find("Characters").GetComponent<CharactersController>().CancelAction();
            Action = ActionBarItem.Actions.None;
            if(State == States.Disabled) State = States.Enabled;
            
        }
    }

    private void Create()
    {
        CharactersController CC = GameObject.Find("Characters").GetComponent<CharactersController>();
        CharacterHolder CH = CC.CurrentCharacterHolder;

        float parentHeight = transform.GetComponent<RectTransform>().sizeDelta.y;

        for (var i = 0; i < _actions.Count; i++)
        {
            GameObject abilityBarItem = (GameObject)Instantiate(Resources.Load("UIBarItem"));
            abilityBarItem.transform.SetParent(transform);
            abilityBarItem.name = _actions[i];

            float bottom = parentHeight - ((_itemHeight * (i + 1)) + (_itemGap * i));
            float top = -(_itemHeight * i) - (_itemGap * i);

            ActionBarItem ABI = new ActionBarItem(abilityBarItem, _actions[i], bottom, top);
            Items.Add(ABI);
        }

        if (Items.Count > 0) Items[0].Active = true;

        ScrollPercent = 1 / (float)(_actions.Count - 4);

        var overFlowItems = _actions.Count - 4;
        if (overFlowItems > 0) ScrollPercent = 1/(float) overFlowItems;
        else ScrollPercent = 0;

        ParentScrollBar.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, ScrollPosition);
    }

    public void Refresh(List<string> actions)
    {
        base.Refresh();
        ClearMenuBar();
        _actions = actions;
        Create();
    }

    public void ClearMenuBar()
    {
        Items.RemoveRange(0, Items.Count);
        for (var i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    public override void Show()
    {
        base.Show();
        GameObject.Find("AbilityBar").GetComponent<AbilityBar>().State = States.Hidden;
        GameObject.Find("MagicBar").GetComponent<MagicBar>().State = States.Hidden;
    }

    public void DisableAction()
    {
        foreach (ActionBarItem item in Items)
        {
            if (item.Action == Action) item.DisableItem();
        }
    }

    private void UpdateActionBar()
    {
        if (State != States.Enabled) return;
        if(State == States.Enabled) GameObject.Find("AbilityBar").GetComponent<AbilityBar>().State = States.Hidden;
        if (State == States.Enabled) GameObject.Find("MagicBar").GetComponent<MagicBar>().State = States.Hidden;

        foreach (MenuBarItem item in Items)
        {
            item.Update();
        }
        if (ItemIsActive() == false) ActiveNextAvaliableItem();

    }

    private bool ItemIsActive()
    {
        return Items.Any(t => t.Active);
    }

    private void ActiveNextAvaliableItem()
    {
        foreach (MenuBarItem item in Items)
        {
            if (item.Disabled()) continue;
            item.Active = true;
            return;
        }
    }

    private void AdjustListHeight()
    {
        float combinedItemHeight = _actions.Count * (_itemHeight + _itemGap);

        if (combinedItemHeight < transform.GetComponent<RectTransform>().sizeDelta.y) return;

        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(transform.GetComponent<RectTransform>().sizeDelta.x, combinedItemHeight);
    }
}
