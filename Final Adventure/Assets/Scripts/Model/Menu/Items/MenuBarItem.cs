using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class MenuBarItem {

    public Color Highlight = new Color(0.48f, 0.79f, 0.87f, 0.47f);
    public Color Normal = new Color(0.6f, 0.6f, 0.6f, 0.47f);
    public Color Disabled = new Color(0.6f, 0.6f, 0.6f, 0.2f);
    public Color DisabledText = new Color(1f, 1f, 1f, 0.2f);

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
        Item.GetComponent<Image>().color = Highlight;
        _active = true;
    }

    private void UnhighlightItem()
    {
        Item.GetComponent<Image>().color = Normal;
        _active = false;
    }

    public void DisableItem()
    {
        Item.GetComponent<Image>().color = Disabled;
        Item.transform.GetChild(0).GetComponent<Text>().color = DisabledText;
        _active = false;
    }


    public virtual void Selected()
    {
        
    }

    // Update is called once per frame
    public void Update () {
        if (_active) Item.GetComponent<RectTransform>().offsetMin = Vector2.Lerp(Item.GetComponent<RectTransform>().offsetMin, new Vector2(LeftPoition, Item.GetComponent<RectTransform>().offsetMin.y), 5 * Time.deltaTime);
        else Item.GetComponent<RectTransform>().offsetMin = Vector2.Lerp(Item.GetComponent<RectTransform>().offsetMin, new Vector2(NormalPosition, Item.GetComponent<RectTransform>().offsetMin.y), 5 * Time.deltaTime);
    }
}
