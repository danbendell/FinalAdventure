using UnityEngine;
using System.Collections;

public class RotationNodes : MonoBehaviour
{
    public Material Normal;
    public Material Highlight;

    private Vector3 _left = new Vector3(0f, 0f, -1f); 
    private Vector3 _right = new Vector3(0f, 0f, 1f); 
    private Vector3 _top = new Vector3(-1f, 0f, 0f); 
    private Vector3 _bottom = new Vector3(1f, 0f, 0f); 

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update () {
        CharactersController CC = GameObject.Find("Characters").GetComponent<CharactersController>();
        CharacterHolder CH = CC.CurrentCharacterHolder;
        
        if (Input.GetKeyDown(KeyCode.W))
	    {
	        FindChild("Top");
            //CH.transform.transform.forward = _top;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            FindChild("Bottom");
            //CH.transform.transform.forward = _bottom;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            FindChild("Left");
           // CH.transform.transform.forward = _left;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            FindChild("Right");
            //CH.transform.transform.forward = _right;
        }


    }

    private void FindChild(string value)
    {
        HighlightNode(value);
       
    }

    private void HighlightNode(string value)
    {
        Renderer[] renderers = transform.GetComponentsInChildren<Renderer>();
        for (var i = 0; i < renderers.Length; i++)
        {
            if (renderers[i].name != value)
            {
                renderers[i].material = Normal;
            }
            else
            {
                renderers[i].material = Highlight;
                Light light = transform.GetComponentInChildren<Light>();
                light.transform.position = renderers[i].transform.position;
            }
        }
    }
}
