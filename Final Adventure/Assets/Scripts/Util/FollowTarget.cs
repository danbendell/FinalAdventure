using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour
{

    private bool _follow;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (_follow == false) return;
        CharactersController CC = GameObject.Find("Characters").GetComponent<CharactersController>();
	    CharacterHolder CH = CC.CurrentCharacterHolder;
        transform.localPosition = new Vector3(CH.transform.localPosition.x, CH.transform.localPosition.y + 1, CH.transform.localPosition.z);
	}

    public void BeginFollow()
    {
        _follow = true;
    }

}
