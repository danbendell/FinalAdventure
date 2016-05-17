using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class CarouselBar : MenuBar
{

    public CarouselBarItem.Options Option = CarouselBarItem.Options.None;

    private List<string> _options = new List<string> { "Select", "Info", "Play"};
    private float _itemHeight = 45.5f;
    private float _itemGap = 2f;

    // Use this for initialization
    void Start()
    {

        Init();

        State = MenuBar.States.Enabled;

        AdjustListHeight();

        Create();

    }

    // Update is called once per frame
    void Update()
    {
        UpdateMenu();
        UpdateActionBar();
        CheckCameraPosition();

        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            GameObject.Find("Canvas").GetComponent<MoreInfo>().MoreInfoGameObject.SetActive(false);
            if (State == MenuBar.States.Enabled) return;
            GameObject.Find("Main Camera").GetComponent<MoveCamera>().MoveToCarousel();
            GameObject.Find("CharacterCarousel").GetComponent<CarouselController>().RemoveCharacter();
            GameObject.Find("Stats").GetComponent<StatsBar>()._enabled = true;
            Option = CarouselBarItem.Options.None;
            if (State == MenuBar.States.Disabled) State = MenuBar.States.Enabled;

        }
    }

    private void CheckCameraPosition()
    {
        if (GameObject.Find("Main Camera").GetComponent<MoveCamera>().IsAtCarousel())
        {
            GameObject.Find("Stats").GetComponent<StatsBar>().Show();
            GameObject.Find("CharacterCount").GetComponent<StatsBar>().Hide();
            GameObject.Find("CarouselBar").GetComponent<CarouselBar>().Option = CarouselBarItem.Options.None;
            GameObject.Find("CarouselBar").GetComponent<CarouselBar>().State = MenuBar.States.Enabled;
            GameObject.Find("MapBar").GetComponent<MapBar>().State = MenuBar.States.Hidden;
        }

        if (GameObject.Find("Main Camera").GetComponent<MoveCamera>().IsAtMap())
        {
            GameObject.Find("Stats").GetComponent<StatsBar>().Hide();
            GameObject.Find("CharacterCount").GetComponent<StatsBar>().Close();
            GameObject.Find("CarouselBar").GetComponent<CarouselBar>().Option = CarouselBarItem.Options.Select;
            GameObject.Find("CarouselBar").GetComponent<CarouselBar>().State = MenuBar.States.Hidden;
            GameObject.Find("MapBar").GetComponent<MapBar>().State = MenuBar.States.Enabled;
        }
    }

    private void Create()
    {

        float parentHeight = transform.GetComponent<RectTransform>().sizeDelta.y;

        for (var i = 0; i < _options.Count; i++)
        {
            GameObject carouselBarItem = (GameObject)Instantiate(Resources.Load("UIBarItem"));
            carouselBarItem.transform.SetParent(transform);
            carouselBarItem.name = _options[i];

            float bottom = parentHeight - ((_itemHeight * (i + 1)) + (_itemGap * i));
            float top = -(_itemHeight * i) - (_itemGap * i);

            CarouselBarItem CBI = new CarouselBarItem(carouselBarItem, _options[i], bottom, top);
            Items.Add(CBI);
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
    }

    public void DisableAction()
    {
        foreach (CarouselBarItem item in Items)
        {
            if (item.Option == Option) item.DisableItem();
        }
    }

    private void UpdateActionBar()
    {
        if (State != MenuBar.States.Enabled) return;

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
        float combinedItemHeight = _options.Count * (_itemHeight + _itemGap);

        if (combinedItemHeight < transform.GetComponent<RectTransform>().sizeDelta.y) return;

        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(transform.GetComponent<RectTransform>().sizeDelta.x, combinedItemHeight);
    }
}
