using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class MenuOptions : MonoBehaviour
{
    private Color _red = new Color(143.0f / 255.0f, 26.0f / 255.0f, 31.0f / 255.0f, 255.0f / 255.0f);
    private List<Text> _menuOptions = new List<Text>();


	// Use this for initialization
	void Start () {
	    for (var i = 0; i < transform.childCount; i++)
	    {
	        Transform trans = transform.GetChild(i);

            for (var x = 0; x < trans.childCount; x++)
            {
                if (trans.GetChild(x).GetComponent<Text>() != null)
                {
                    _menuOptions.Add(trans.GetChild(x).GetComponent<Text>());
                }
            }
	    }

        _menuOptions[0].color = _red;
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.DownArrow))
	    {
	        for (var i = 0; i < _menuOptions.Count; i++)
	        {
	            if (_menuOptions[i].color != _red) continue;
	            if (i + 1 >= _menuOptions.Count) continue;

	            _menuOptions[i].color = Color.black;
	            _menuOptions[i + 1].color = _red;
                break;
	        }
	    }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            for (var i = _menuOptions.Count - 1; i > 0; i--)
            {
                if (_menuOptions[i].color != _red) continue;
                if (i - 1 <= -1) continue;

                _menuOptions[i].color = Color.black;
                _menuOptions[i - 1].color = _red;
                break;
            }
        }
    }
}
