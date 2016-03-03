using UnityEngine;
using System.Collections;

public class Turn
{


    public bool Moved { get; set; }
    public bool CompletedAction { get; set; }

    public Turn()
    {
        Moved = false;
        CompletedAction = false;
    }

    public bool Complete()
    {
        return (Moved && CompletedAction);
    }

    public void Skip()
    {
        Moved = true;
        CompletedAction = true;
    }

// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
