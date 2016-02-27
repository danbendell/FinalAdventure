using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ActionBarItem
{

    private Color Highlight = new Color(0.48f, 0.79f, 0.87f, 0.47f);
    private Color Normal = new Color(0.6f, 0.6f, 0.6f, 0.47f);

    private float _rightPoition = 35f;
    private float _normalPosition = 0f;

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


    public ActionBarItem(GameObject item)
    {
        Item = item;
        _active = false;
    }

    private void HighlightItem()
    {
        Item.GetComponent<RectTransform>().offsetMax = new Vector2(_rightPoition, Item.GetComponent<RectTransform>().offsetMax.y);
        Item.GetComponent<Image>().color = Highlight;
        _active = true;
    }

    private void UnhighlightItem()
    {
        Item.GetComponent<RectTransform>().offsetMax = new Vector2(_normalPosition, Item.GetComponent<RectTransform>().offsetMax.y);
        Item.GetComponent<Image>().color = Normal;
        _active = false;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
