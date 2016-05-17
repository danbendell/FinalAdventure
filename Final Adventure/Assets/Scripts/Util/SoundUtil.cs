using System;
using UnityEngine;
using System.Collections;

public class SoundUtil : MonoBehaviour
{

    private AudioSource _attackSound;
    private AudioSource _deadSound;
    private AudioSource _fireSound;
    private AudioSource _healSound;
    private AudioSource _windSound;
    
    public enum Sounds
    {
        Attack,
        Dead,
        Fire,
        Heal,
        Wind,
        None
    }

    // Use this for initialization
    void Start ()
	{
	    _attackSound = GameObject.Find("AttackSound").GetComponent<AudioSource>();
        _deadSound = GameObject.Find("DeadSound").GetComponent<AudioSource>();
        _fireSound = GameObject.Find("FireSound").GetComponent<AudioSource>();
        _healSound = GameObject.Find("HealSound").GetComponent<AudioSource>();
        _windSound = GameObject.Find("WindSound").GetComponent<AudioSource>();
	}
	

    public void PlaySound(Sounds sound)
    {
        switch (sound)
        {
            case Sounds.Attack:
                _attackSound.Play();
                break;
            case Sounds.Dead:
                _deadSound.Play();
                break;
            case Sounds.Fire:
                _fireSound.Play();
                break;
            case Sounds.Heal:
                _healSound.Play();
                break;
            case Sounds.Wind:
                _windSound.Play();
                break;
        }

    }
}
