using UnityEngine;
using System.Collections;

public class StatsBar : MonoBehaviour {

    public bool _enabled = true;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        AnimateStatsBar();
    }

    private void AnimateStatsBar()
    {
        if (_enabled == false) Animate(-98f);
        else Animate(145f);
    }

    private void Animate(float x)
    {
        transform.GetComponent<RectTransform>().anchoredPosition3D =
                Vector3.Lerp(transform.GetComponent<RectTransform>().anchoredPosition3D, new Vector3(x, transform.GetComponent<RectTransform>().anchoredPosition3D.y, transform.GetComponent<RectTransform>().anchoredPosition3D.z), 5 * Time.deltaTime);
    }

    public void Show()
    {
         _enabled = true;
    }

    public void Hide()
    {
        _enabled = false;
    }
}
