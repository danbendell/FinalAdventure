using UnityEngine;
using System.Collections;
using Assets.Scripts.Model;
using Assets.Scripts.Movement;

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
	
	// Update is called once per frame
	void Update () {
	
	}
}
