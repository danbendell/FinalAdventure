using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GamePause : MonoBehaviour
{

    public GameObject PauseMenu;
    public GameObject ControlsPanel;

    public bool isPaused;
    public bool isEndGame;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (GameObject.Find("Canvas").GetComponent<EndGameController>() == null) return;
	    isEndGame = GameObject.Find("Canvas").GetComponent<EndGameController>().EndGame.activeSelf;
	    if (isEndGame) return;

        if (Input.GetKeyDown(KeyCode.Escape))
	    {
	        if (isPaused)
	        {
	            isPaused = false;
	            PauseMenu.SetActive(false);
                ControlsPanel.SetActive(false);
            }
	        else
            { 
                isPaused = true;
                PauseMenu.SetActive(true);
            }
	    }
	
	}
}
