using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class MenuBarItem {

    public Color HighlightColor = new Color(0.48f, 0.79f, 0.87f, 0.47f);
    public Color NormalColor = new Color(0.6f, 0.6f, 0.6f, 0.47f);
    public Color NormalText = new Color(1f, 1f, 1f, 1f);
    public Color DisabledColor = new Color(0.6f, 0.6f, 0.6f, 0.2f);
    public Color DisabledText = new Color(1f, 1f, 1f, 0.2f);

    public string Name;

    public Vector3 Scale = new Vector3(1f, 1f, 1f);
    public Vector2 AnchorMin = new Vector2(0f, 0f);
    public Vector2 AnchorMax = new Vector2(1f, 1f);

    public float LeftPoition = -35f;
    public float NormalPosition = 0f;

    public GameObject Item { get; set; }

    private bool _active;

    public bool Active
    {
        get { return _active; }
        set
        {
            if (value) HighlightItem();
            else UnhighlightItem();
        }
    }

    private void HighlightItem()
    {
        Item.GetComponent<Image>().color = HighlightColor;
        Item.transform.GetChild(0).GetComponent<Text>().color = NormalText;
        _active = true;
    }

    private void UnhighlightItem()
    {
        Item.GetComponent<Image>().color = NormalColor;
        Item.transform.GetChild(0).GetComponent<Text>().color = NormalText;
        _active = false;
    }

    public void DisableItem()
    {
        Item.GetComponent<Image>().color = DisabledColor;
        Item.transform.GetChild(0).GetComponent<Text>().color = DisabledText;
        _active = false;
    }

    public void EnableItem()
    {
        if (_active) HighlightItem();
        else UnhighlightItem();
    }

    public bool Disabled()
    {
        return Item.GetComponent<Image>().color == DisabledColor;
    }


    public virtual void Selected()
    {
        
    }

    // Update is called once per frame
    public virtual void Update () {
        if (_active) Item.GetComponent<RectTransform>().offsetMin = Vector2.Lerp(Item.GetComponent<RectTransform>().offsetMin, new Vector2(LeftPoition, Item.GetComponent<RectTransform>().offsetMin.y), 5 * Time.deltaTime);
        else Item.GetComponent<RectTransform>().offsetMin = Vector2.Lerp(Item.GetComponent<RectTransform>().offsetMin, new Vector2(NormalPosition, Item.GetComponent<RectTransform>().offsetMin.y), 5 * Time.deltaTime);
    }
}
