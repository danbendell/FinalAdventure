using UnityEngine;
using System.Collections;
using Assets.Scripts.Movement;

public class Rotation : MonoBehaviour {


    private Vector3 _left = new Vector3(0f, 0f, -1f);
    private Vector3 _right = new Vector3(0f, 0f, 1f);
    private Vector3 _top = new Vector3(-1f, 0f, 0f);
    private Vector3 _bottom = new Vector3(1f, 0f, 0f);

    private CharacterHolder _characterHolder;
    private Vector2 _pointer;


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        CharactersController CC = GameObject.Find("Characters").GetComponent<CharactersController>();
        _characterHolder = CC.CurrentCharacterHolder;
	    _pointer = GameObject.Find("Floor").GetComponent<FloorHighlight>().PointerPosition;

        float differenceInX = CalculatePositiveDifference(_characterHolder.Character.XyPosition().x, _pointer.x);
        float differnceInY = CalculatePositiveDifference(_characterHolder.Character.XyPosition().y, _pointer.y);

	    float direction = differenceInX - differnceInY;
	    if (direction > 0) RotateLeftOrRight(_characterHolder.Character.XyPosition().x, _pointer.x);
	    if (direction < 0) RotateTopOrBottom(_characterHolder.Character.XyPosition().y, _pointer.y);

	}

    private void RotateLeftOrRight(float valueOne, float valueTwo)
    {
        if ((valueOne - valueTwo) > 0) _characterHolder.transform.forward = _left;
        if ((valueOne - valueTwo) < 0) _characterHolder.transform.forward = _right;
    }

    private void RotateTopOrBottom(float valueOne, float valueTwo)
    {
        if ((valueOne - valueTwo) > 0) _characterHolder.transform.forward = _bottom;
        if ((valueOne - valueTwo) < 0) _characterHolder.transform.forward = _top;
    }

    private float CalculatePositiveDifference(float valueOne, float valueTwo)
    {
        if (valueOne > valueTwo)
        {
            return valueOne - valueTwo;
        }
        else
        {
            return valueTwo - valueOne;
        }
    }
}
