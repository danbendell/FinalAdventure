using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuSelect : MonoBehaviour
{

    public GameObject Menu;
    public GameObject HowTo;

    public Image ForegroundHighlight;
    public Image MidgroundHighlight;

    public Text PlayText;
    public Text HowToPlayText;

	// Use this for initialization
	void Start () {
        ForegroundHighlight.enabled = false;
        MidgroundHighlight.enabled = true;

    }
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.DownArrow))
	    {
	        ForegroundHighlight.enabled = true;
	        MidgroundHighlight.enabled = false;
	    }

	    if (Input.GetKeyDown(KeyCode.UpArrow))
	    {
            ForegroundHighlight.enabled = false;
            MidgroundHighlight.enabled = true;
        }

	    if (Input.GetKeyDown(KeyCode.Return))
	    {
	        if (MidgroundHighlight.enabled)
	        {
                //Play
                SceneManager.LoadScene("CharacterSelect", LoadSceneMode.Single);
            }
	        else
	        {
                //HowTo
                Menu.SetActive(false);
                HowTo.SetActive(true);
            }
	    }

	    if (MidgroundHighlight.enabled)
	    {
	        PlayText.transform.localPosition = Vector3.Lerp(PlayText.transform.localPosition,
	            new Vector3(PlayText.transform.localPosition.x, -30, PlayText.transform.localPosition.z), Time.deltaTime*5f);

            HowToPlayText.transform.localPosition = Vector3.Lerp(HowToPlayText.transform.localPosition,
                new Vector3(HowToPlayText.transform.localPosition.x, -210, HowToPlayText.transform.localPosition.z), Time.deltaTime * 5f);
        }
	    else
	    {
            PlayText.transform.localPosition = Vector3.Lerp(PlayText.transform.localPosition,
                new Vector3(PlayText.transform.localPosition.x, -110f, PlayText.transform.localPosition.z), Time.deltaTime * 5f);

            HowToPlayText.transform.localPosition = Vector3.Lerp(HowToPlayText.transform.localPosition,
               new Vector3(HowToPlayText.transform.localPosition.x, -65, HowToPlayText.transform.localPosition.z), Time.deltaTime * 5f);
        }
 	}
}
