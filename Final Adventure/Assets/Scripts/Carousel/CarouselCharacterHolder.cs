using UnityEngine;
using System.Collections;
using Assets.Scripts.Model;

public class CarouselCharacterHolder : MonoBehaviour
{

    public Character Character;

    public Jobs Job;
    public enum Jobs
    {
        Wizard,
        Archer,
        Worrior
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
            case Jobs.Worrior:
                Character = new Warrior();
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
