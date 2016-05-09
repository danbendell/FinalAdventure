using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterCount : MonoBehaviour
{
    public Color HighlightColor = new Color(0.48f, 0.79f, 0.87f, 0.47f);
    public Color NormalColor = new Color(0.6f, 0.6f, 0.6f, 0.47f);

    private int count;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	    count = GameObject.Find("CharacterCarousel").GetComponent<CarouselController>().ChosenCharacters.Count;
	    HighlightBarItems();
	}

    private void HighlightBarItems()
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            GameObject GO = transform.GetChild(i).gameObject;
            if (i < count) HighlightItem(GO);
            else UnhighlightItem(GO); 
        }
    }

    private void HighlightItem(GameObject Item)
    {
        Item.GetComponent<Image>().color = HighlightColor;
    }

    private void UnhighlightItem(GameObject Item)
    {
        Item.GetComponent<Image>().color = NormalColor;
    }
}
