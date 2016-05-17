using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject ControlsPanel;

    public Color HighlightColor = new Color(0.48f, 0.79f, 0.87f, 0.47f);
    public Color NormalColor = new Color(0.6f, 0.6f, 0.6f, 0.47f);

    public Image Resume;
    public Image Controls;
    public Image Quit;

    private List<Image> _images = new List<Image>();
    private int _currentHighlight = 0;

	// Use this for initialization
	void Start () {
        _images.Add(Resume);
        _images.Add(Controls);
        _images.Add(Quit);
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _currentHighlight--;
            if (_currentHighlight < 0) _currentHighlight = 0;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _currentHighlight++;
            if (_currentHighlight > _images.Count - 1) _currentHighlight = _images.Count - 1;
        }

	    if (Input.GetKeyDown(KeyCode.Return))
	    {
	        if (_images[0].color == HighlightColor)
	        {
	            GameObject.Find("Canvas").GetComponent<GamePause>().isPaused = false;
	            PauseMenu.SetActive(false);
                ControlsPanel.SetActive(false);
            }
	        if (_images[1].color == HighlightColor)
	        {
                //Controls
                ControlsPanel.SetActive(true);

            }
	        if (_images[2].color == HighlightColor)
	        {
                Destroy(GameObject.Find("Characters"));
                SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            }
	    }

	    if (Input.GetKeyDown(KeyCode.RightShift))
	    {
            ControlsPanel.SetActive(false);
	    }

	    for (int i = 0; i < _images.Count; i++)
	    {
	        if (i == _currentHighlight)
	        {
	            _images[i].color = HighlightColor;
                _images[i].rectTransform.localPosition = Vector3.Lerp(_images[i].rectTransform.localPosition, new Vector3(-14f, _images[i].rectTransform.localPosition.y, _images[i].rectTransform.localPosition.z), Time.deltaTime * 5f);
                _images[i].rectTransform.sizeDelta = Vector2.Lerp(_images[i].rectTransform.sizeDelta, new Vector2(233, 50), Time.deltaTime * 5f);
                continue;
	        }

	        _images[i].color = NormalColor;
            _images[i].rectTransform.localPosition = Vector3.Lerp(_images[i].rectTransform.localPosition, new Vector3(4f, _images[i].rectTransform.localPosition.y, _images[i].rectTransform.localPosition.z), Time.deltaTime * 5f);
            _images[i].rectTransform.sizeDelta = Vector2.Lerp(_images[i].rectTransform.sizeDelta, new Vector2(197, 50), Time.deltaTime * 5f);
        }
	}
}
