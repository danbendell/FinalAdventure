using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class MapBar : SubMenuBar
{

    public MapBarItem.Options Option = MapBarItem.Options.None;
    private List<string> _options = new List<string> { "Place", "Remove" };
    private float _itemHeight = 45.5f;
    private float _itemGap = 2f;

    // Use this for initialization
    void Start()
    {
        Init(_options);

        Create();

        if (Items.Count > 0) Items[0].Active = true;

        ScrollPercent = 1 / (ItemCount - 4);

        ParentScrollBar.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, ScrollPosition);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMapBar();
        UpdateSubMenu();

        KeyboardInput();

    }

    private void Create()
    {
        float parentHeight = transform.GetComponent<RectTransform>().sizeDelta.y;

        for (var i = 0; i < _options.Count; i++)
        {
            GameObject mapBarItem = (GameObject)Instantiate(Resources.Load("UIBarItem"));
            mapBarItem.transform.SetParent(transform);
            mapBarItem.name = _options[i];

            float bottom = parentHeight - ((_itemHeight * (i + 1)) + (_itemGap * i));
            float top = -(_itemHeight * i) - (_itemGap * i);

            MapBarItem MBI = new MapBarItem(mapBarItem, _options[i], bottom, top);
            Items.Add(MBI);
        }

        if (Items.Count > 0) Items[0].Active = true;

        ScrollPercent = 1 / (float)(_options.Count - 4);

        var overFlowItems = _options.Count - 4;
        if (overFlowItems > 0) ScrollPercent = 1 / (float)overFlowItems;
        else ScrollPercent = 0;

        ParentScrollBar.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, ScrollPosition);
    }

    public void Refresh(List<string> actions)
    {
        base.Refresh();
        ClearMenuBar();
        _options = actions;
        Create();
    }

    public void DisableAction()
    {
        foreach (MapBarItem item in Items)
        {
            if (item.Option == Option) item.DisableItem();
        }
    }

    private void UpdateMapBar()
    {
        if (State != States.Enabled) return;

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

    private void KeyboardInput()
    {

        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            if (State == States.Disabled)
            {
                State = States.Enabled;
                Option = MapBarItem.Options.None;
                if (State == States.Disabled) State = States.Enabled;
            }
            else if (State == States.Enabled)
            {
                State = States.Hidden;
                Option = MapBarItem.Options.None;
                GameObject.Find("CharacterCarousel").GetComponent<CarouselController>().RemoveCharacter();
                GameObject.Find("CarouselBar").GetComponent<MenuBar>().State = MenuBar.States.Enabled;
            }

        }

    }
}
