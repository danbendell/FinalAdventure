using UnityEngine;
using System.Collections;
using Assets.Scripts.Model;
using Assets.Scripts.Movement;

public class Turn
{


    public bool Moved { get; set; }
    public bool CompletedAction { get; set; }
    public bool Rotated { get; set; }

    public Turn()
    {
        Moved = false;
        CompletedAction = false;
        Rotated = false;
    }

    public bool Complete()
    {
        return (Moved && CompletedAction);
        //return (Moved && CompletedAction & Rotated);
    }

    public void Skip()
    {
        Moved = true;
        CompletedAction = true;
        Rotated = true;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
