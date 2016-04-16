using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TitleAnimation : MonoBehaviour
{

    private Color _red = new Color(143.0f / 255.0f, 26.0f / 255.0f, 31.0f / 255.0f, 255.0f / 255.0f);
    private Text[] _titleUnderline;

    private Image[] _titleNewUnderline;
    private const string Underline = "UnderlineV3";
    public Text Xspot;

    // Use this for initialization
    void Start()
    {

        for (var i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name.Equals(Underline))
            {
                _titleUnderline = transform.GetChild(i).GetComponentsInChildren<Text>();
            }
            if (transform.GetChild(i).name.Equals(Underline))
            {
                _titleNewUnderline = transform.GetChild(i).GetComponentsInChildren<Image>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(AnimateNextDash(_titleNewUnderline[0], 0));

            //for (var i = 0; i < _titleNewUnderline.Length; i++)
            //{
            //    _titleNewUnderline[i].canvasRenderer.SetAlpha(0.01f);
            //    _titleNewUnderline[i].CrossFadeAlpha(255.0f, .5f, false);
            //}
        }
    }

    private IEnumerator AnimateNextDash(Image dash, int currentDash)
    {
        yield return new WaitForSeconds(0.2f);
        _titleNewUnderline[currentDash].canvasRenderer.SetAlpha(0.01f);
        _titleNewUnderline[currentDash].CrossFadeAlpha(255.0f, .1f, false);
        currentDash++;

        if (currentDash >= _titleNewUnderline.Length)
        {
            //StartCoroutine(DisplayX());
            yield break;
        }
        StartCoroutine(AnimateNextDash(_titleNewUnderline[currentDash], currentDash));
    }

    private IEnumerator DisplayX()
    {
        yield return new WaitForSeconds(0.05f);
        Xspot.color = _red;
    }

}
