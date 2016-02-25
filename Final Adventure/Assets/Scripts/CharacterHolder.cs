using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Model;
using Assets.Scripts.Movement;
using Assets.Scripts.Util;

public class CharacterHolder : MonoBehaviour
{

    public Character Character;

    public Jobs Job;
    public enum Jobs
    {
        Wizard,
        Archer
    }

    // Use this for initialization
    void Start () {
        switch (Job)
        {
            case Jobs.Wizard:
                Character = new Wizard();
                break;
            case Jobs.Archer:
                Character = new Archer();
                break;
        }
        Character.Position =transform.position;
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.L))
        {
            Character.WhoAmI();
        }

	    if (Input.GetKeyDown(KeyCode.M))
	    {

            Vector2 floorPosition = new Vector2(Character.Position.x, Character.Position.z);
            GameObject.Find("Floor").GetComponent<FloorHighlight>().SetMovement(floorPosition, Character.Speed);

        }
	}
}
