using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpinCarousel : MonoBehaviour {

    public List<GameObject> CharacterList = new List<GameObject>();
    private List<Vector3> _characterPositions = new List<Vector3>();

    private int _position = 0;
    private bool _rotating;

    // Use this for initialization
    void Start () {
        for (var i = 0; i < transform.childCount; i++)
        {
            CharacterList.Add(transform.GetChild(i).gameObject);
            _characterPositions.Add(transform.GetChild(i).gameObject.transform.position);
        }
    }
	
	// Update is called once per frame
	void Update ()
	{
        if (GameObject.Find("CarouselBar").GetComponent<CarouselBar>().State != MenuBar.States.Enabled) return;
        KeyboardInput();
	    UpdatePositions();
	}

    private void KeyboardInput()
    {
        //if (_rotating) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _position++;
            if (_position > CharacterList.Count - 1) _position = 0;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _position--;
            if (_position < 0) _position = CharacterList.Count - 1;
        }
    }

    private void UpdatePositions()
    {
        for (var i = 0; i < CharacterList.Count; i++)
        {
            Vector3 newPosition = _characterPositions[GetListPosition(i)];
            if (newPosition == new Vector3(0, 0, 0))
                transform.GetComponent<CarouselController>().CurrentCharacter = CharacterList[i];

            CharacterList[i].transform.position = Vector3.LerpUnclamped(CharacterList[i].transform.position,
                _characterPositions[GetListPosition(i)], Time.deltaTime*5f);
        }
    }

    private int GetListPosition(int i)
    {
        var value = i;
        value += _position;
        if (value > _characterPositions.Count - 1) value = value - _characterPositions.Count;
        return value;
    }
    
}
