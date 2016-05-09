using UnityEngine;
using System.Collections;

public class Probabilities {

    public CharacterHolder.Jobs Job { get; set; }
    public float Attack { get; set; }
    public float Move { get; set; }
    public float Heal { get; set; }

    public Probabilities(CharacterHolder.Jobs job)
    {
        Job = job;
        Attack = 0f;
        Move = 0f;
        Heal = 0f;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
