using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SubMenuBar : MenuBar
{

    protected float ItemHeight = 45.5f;
    protected float ItemGap = 2f;

    protected float ItemCount;
    
    // Use this for initialization
    public void Init<T> (List<T> list) {
        base.Init();

        ItemCount = list.Count;

        State = States.Hidden;

        AdjustListHeight();

    }
	
	// Update is called once per frame
	public void UpdateSubMenu() {
        UpdateMenu();
    }

    public void ClearMenuBar()
    {
        Items.RemoveRange(0, Items.Count);
        for (var i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    private void AdjustListHeight()
    {
        float combinedItemHeight = ItemCount * (ItemHeight + ItemGap);

        if (combinedItemHeight < transform.GetComponent<RectTransform>().sizeDelta.y) return;

        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(transform.GetComponent<RectTransform>().sizeDelta.x, combinedItemHeight);
    }
}
