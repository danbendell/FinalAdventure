using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ActionBarItem
{

    private Color Highlight = new Color(0.48f, 0.79f, 0.87f, 0.47f);
    private Color Normal = new Color(0.6f, 0.6f, 0.6f, 0.47f);
    private Color Disabled = new Color(0.6f ,0.6f, 0.6f, 0.2f);
    private Color DisabledText = new Color(1f, 1f, 1f, 0.2f);

    private float _rightPoition = 35f;
    private float _normalPosition = 0f;

    public GameObject Item { get; set; }

    public Actions Action;
    
    public enum Actions
    {
        Attack,
        Move,
        Magic
    }

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


    public ActionBarItem(GameObject item, Actions actions)
    {
        Item = item;
        Action = actions;
        _active = false;
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

    public void Selected()
    {
        switch (Action)
        {
            case Actions.Attack:
                break;
            case Actions.Move:
                GameObject.Find("Characters").GetComponent<CharactersController>().HighlightCharacterMovement();
                break;
            case Actions.Magic:
                break;
        }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	public void Update () {
        if (_active) Item.GetComponent<RectTransform>().offsetMax = Vector2.Lerp(Item.GetComponent<RectTransform>().offsetMax, new Vector2(_rightPoition, Item.GetComponent<RectTransform>().offsetMax.y), 5 * Time.deltaTime);
        else Item.GetComponent<RectTransform>().offsetMax = Vector2.Lerp(Item.GetComponent<RectTransform>().offsetMax, new Vector2(_normalPosition, Item.GetComponent<RectTransform>().offsetMax.y), 5 * Time.deltaTime);
    }
}
