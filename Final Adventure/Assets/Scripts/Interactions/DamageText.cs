using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    private Color _red = new Color(143.0f/255.0f, 26.0f/255.0f, 31.0f/255.0f, 255.0f/255.0f);
    private readonly Color _green = new Color(51.0f/255.0f, 181.0f/255.0f, 56.0f/255.0f, 255.0f/255.0f);
    public GameObject GameObject;
    private Text _text;
    private float _yPosition;
    private float _alpha;

	// Use this for initialization
	void Start ()
	{
        _text = GameObject.GetComponent<Text>();
        _yPosition = 0f;
        _text.color = new Color(_text.color.a, _text.color.g, _text.color.b, 0.0f); 
	    //_alpha = 0.75f;
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.H))
	    {
	        AnimateDamage(5);
	    }
        GameObject.GetComponent<RectTransform>().anchoredPosition3D = Vector3.Lerp(GameObject.GetComponent<RectTransform>().anchoredPosition3D, new Vector3(GameObject.GetComponent<RectTransform>().anchoredPosition3D.x, _yPosition, GameObject.GetComponent<RectTransform>().anchoredPosition3D.z), 3 * Time.deltaTime);
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, Mathf.Lerp(_text.color.a, _alpha, 5 * Time.deltaTime));
    }

    public void AnimateDamage(int value)
    {
        _text.color = _red;
        _text.text = value.ToString();
        StartCoroutine(Animate());
    }

    public void AnimateHeal(int value)
    {
        _text.color = _green;
        _text.text = value.ToString();
        StartCoroutine(Animate());
    }

    public void AnimateMessage(string message)
    {
        _text.color = Color.white;
        _text.text = message;
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        Reset();
       
        yield return new WaitForSeconds(1.5f);
        _yPosition = 20f;
        _alpha = 0f;
    }

    private void Reset()
    {
        _yPosition = 0f;
        GameObject.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(GameObject.GetComponent<RectTransform>().anchoredPosition3D.x, _yPosition, GameObject.GetComponent<RectTransform>().anchoredPosition3D.z);

        _alpha = _text.color.a;
    }
}
