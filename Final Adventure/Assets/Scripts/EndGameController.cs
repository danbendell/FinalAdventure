using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndGameController : MonoBehaviour
{

    public GameObject EndGame;
    public GameObject Victory;
    public GameObject Defeat;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (EndGame.activeSelf == false) return;
	    if (Input.GetKeyDown(KeyCode.Return))
	    {
            Destroy(GameObject.Find("Characters"));
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
	}

    public IEnumerator ShowVictory()
    {
        yield return new WaitForSeconds(1f);
        EndGame.SetActive(true);
        Victory.SetActive(true);
    }

    public IEnumerator ShowDefeat()
    {
        yield return new WaitForSeconds(1f);
        EndGame.SetActive(true);
        Defeat.SetActive(true);
    }
}
