using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RotateCharacter : MonoBehaviour
{
   
    public GameObject CurrentCharacter { get; set; }
    private float _speed = 2f;

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update ()
	{
	    KeyboardInput();
	}

    private void KeyboardInput()
    {
        if (Input.GetKey(KeyCode.A))
        {
            CurrentCharacter.transform.Rotate(Vector3.up, _speed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            CurrentCharacter.transform.Rotate(Vector3.down, _speed);
        }
    }
}
