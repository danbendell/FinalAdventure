using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;

public class PagesController : MonoBehaviour {

    private List<GameObject> _pages = new List<GameObject>();

    public GameObject Menu;
    public GameObject HowTo;

    public GameObject PageOne;
    public GameObject PageTwo;
    public GameObject PageThree;
    public GameObject PageFour;
    public GameObject PageFive;
    public GameObject PageSix;
    public GameObject PageSeven;

    private int _currentPage = 0;

	// Use this for initialization
	void Start () {
        _pages.Add(PageOne);
        _pages.Add(PageTwo);
        _pages.Add(PageThree);
        _pages.Add(PageFour);
        _pages.Add(PageFive);
        _pages.Add(PageSix);
        _pages.Add(PageSeven);

        ResetPositions();

	}
	
	// Update is called once per frame
	void Update () {

	    if (Input.GetKeyDown(KeyCode.RightArrow))
	    {
	        _currentPage ++;
	        if (_currentPage > 6) ReturnToMenu();


	    }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _currentPage --;
            if (_currentPage < 0) ReturnToMenu();
        }

	    if (Input.GetKeyDown(KeyCode.RightShift))
	    {
	        ReturnToMenu();
	    }

        RectTransform objectRectTransform = GameObject.Find("Canvas").GetComponent<RectTransform>();
        float moveAmount = objectRectTransform.rect.width;

        for (var i = 0; i < _pages.Count; i++)
	    {
            if (i < _currentPage)
	        {
	            
                _pages[i].transform.localPosition = Vector3.Lerp(_pages[i].transform.localPosition,
                   new Vector3(-moveAmount * (_currentPage - i), _pages[i].transform.localPosition.y,
                       _pages[i].transform.localPosition.z), Time.deltaTime * 5f);
                continue;
            }

	        if (i == _currentPage)
	        {
                _pages[i].transform.localPosition = Vector3.Lerp(_pages[i].transform.localPosition,
                    new Vector3(0f, _pages[i].transform.localPosition.y,
                        _pages[i].transform.localPosition.z), Time.deltaTime * 5f);
                continue;
	            
	        }

	        if (i > _currentPage)
	        {
                _pages[i].transform.localPosition = Vector3.Lerp(_pages[i].transform.localPosition,
                 new Vector3(moveAmount * (i - _currentPage), _pages[i].transform.localPosition.y,
                     _pages[i].transform.localPosition.z), Time.deltaTime * 5f);
            }
        }

    }

    private void ReturnToMenu()
    {
        _currentPage = 0;
        ResetPositions();
        Menu.SetActive(true);
        HowTo.SetActive(false);
    }

    private void ResetPositions()
    {
        RectTransform objectRectTransform = GameObject.Find("Canvas").GetComponent<RectTransform>();
        float moveAmount = objectRectTransform.rect.width;

        for (var i = 0; i < _pages.Count; i++)
        {
            
            if (i == _currentPage)
            {
                _pages[i].transform.localPosition = new Vector3(0f, _pages[i].transform.localPosition.y,
                        _pages[i].transform.localPosition.z);
                continue;

            }

            if (i > _currentPage)
            {
                _pages[i].transform.localPosition = new Vector3(moveAmount * i, _pages[i].transform.localPosition.y,
                     _pages[i].transform.localPosition.z);
            }
        }
    }
}
