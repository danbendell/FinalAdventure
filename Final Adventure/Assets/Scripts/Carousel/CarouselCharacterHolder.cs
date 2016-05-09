using UnityEngine;
using System.Collections;
using Assets.Scripts.Model;
using Assets.Scripts.Model.Character_Types;

public class CarouselCharacterHolder : MonoBehaviour
{

    public Character Character;

    public CharacterHolder.Jobs Job;

    // Use this for initialization
    void Start () {
        switch (Job)
        {
            case CharacterHolder.Jobs.Wizard:
                Character = new Wizard();
                break;
            case CharacterHolder.Jobs.Archer:
                Character = new Archer();
                break;
            case CharacterHolder.Jobs.Warrior:
                Character = new Warrior();
                break;
            case CharacterHolder.Jobs.Assassin:
                Character = new Assassin();
                break;
            case CharacterHolder.Jobs.Priest:
                Character = new Priest();
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
