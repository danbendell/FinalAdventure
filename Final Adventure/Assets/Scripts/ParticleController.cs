using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Assets.Scripts.Model;
using UnityEngine.Assertions.Comparers;

public class ParticleController : MonoBehaviour
{
    private GameObject _parent;
    private Light _light;
    private List<ParticleSystem> _particalList;

    private float _lightIntencity = 0f;

	// Use this for initialization
	void Start () {
        _particalList = new List<ParticleSystem>();
	    _parent = transform.gameObject;

        for (var i = 0; i < transform.childCount; i++)
	    {
	        GameObject GO = transform.GetChild(i).gameObject;
            if (GO.name == "Particle System") _particalList.Add(GO.GetComponent<ParticleSystem>());
            else if (GO.name == "Point light") _light = GO.GetComponent<Light>();
            else if (GO.name == "Streams") FillParticleList(transform.GetChild(i));
	    }
	}

    private void FillParticleList(Transform streams)
    {
        for (var i = 0; i < streams.childCount; i++)
        {
            Transform trans = streams.GetChild(i);
            for (var x = 0; x < trans.childCount; x++)
            {
               GameObject GO = trans.GetChild(x).gameObject;
                _particalList.Add(GO.GetComponent<ParticleSystem>());
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (_particalList[0].time >= _particalList[0].duration)
        {
            _lightIntencity = 0f;
        }

	    _light.intensity = Mathf.Lerp(_light.intensity, _lightIntencity, Time.deltaTime*5f);

	}

    public void Play(Vector2 position)
    {

        MoveParticleEffect(position);
        
        foreach (var particle in _particalList)
        {
            particle.Play();
        }

        _lightIntencity = 2f;
    }

    private void MoveParticleEffect(Vector2 position)
    {
        Character character = FindCharacter(position);
        _parent.transform.position = new Vector3(character.Position.x, character.Height(), character.Position.z);
    }

    private Character FindCharacter(Vector2 pointer)
    {
        List<CharacterHolder> characterHolders =
            GameObject.Find("Characters").GetComponent<CharactersController>().CharacterHolders;

        foreach (CharacterHolder CH in characterHolders)
        {
            if (CH.Character.XyPosition() != pointer) continue;
            return CH.Character;
        }

        return null;
    }
}
