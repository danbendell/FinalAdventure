using UnityEngine;
using System.Collections;
using Assets.Scripts.Model;
using UnityEngine.UI;

public class UICharacterStats : MonoBehaviour
{

    private Color _ally = new Color(76.0f / 255.0f, 131.0f / 255.0f, 153.0f / 255.0f, 122.0f / 255.0f);
    private Color _enemy = new Color(153.0f / 255.0f, 76.0f / 255.0f, 77.0f / 255.0f, 122.0f / 255.0f);
    private Color _neutral = new Color(153.0f / 255.0f, 153.0f / 255.0f, 153.0f / 255.0f, 122.0f / 255.0f);

    public float HpValue;
    public float MpValue;
    
    // Use this for initialization
    void Start () {
        HpValue = GameObject.Find("HPValue").GetComponent<RectTransform>().localScale.x;
        MpValue = GameObject.Find("MPValue").GetComponent<RectTransform>().localScale.x;
    }
	
	// Update is called once per frame
	void Update ()
	{
        GameObject.Find("HPValue").GetComponent<RectTransform>().localScale = Vector3.Lerp(GameObject.Find("HPValue").GetComponent<RectTransform>().localScale, new Vector3(HpValue, 1f, 1f), 5 * Time.deltaTime);
        GameObject.Find("MPValue").GetComponent<RectTransform>().localScale = Vector3.Lerp(GameObject.Find("MPValue").GetComponent<RectTransform>().localScale, new Vector3(MpValue, 1f, 1f), 5 * Time.deltaTime);
    }

    public void UpdateCharacterStats(CharacterHolder characterHolder, bool isSelf)
    {
        Character character = characterHolder.Character;
        Color uiColor;

        if (isSelf) uiColor = _neutral;
        else uiColor = characterHolder.IsAi ? _enemy : _ally;

        GameObject.Find("StatusBar").GetComponent<Image>().color = uiColor;

        GameObject.Find("StatusBar_Job").GetComponent<Text>().text = character.Job().ToString();

        GameObject.Find("HP Text").GetComponent<Text>().text = character.Health + " / " + character.MaxHealth;
        HpValue = (1.0f / character.MaxHealth) * character.Health;

        GameObject.Find("MP Text").GetComponent<Text>().text = character.Mana + " / " + character.MaxMana;
        MpValue = (1.0f / character.MaxMana) * character.Mana;
    }
}
