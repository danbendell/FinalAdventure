using UnityEngine;
using System.Collections;
using Assets.Scripts.Model;
using UnityEngine.UI;

public class UICharacterStats : MonoBehaviour
{

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

    public void UpdateCharacterStats(Character character)
    {
        GameObject.Find("HP Text").GetComponent<Text>().text = character.Health + " / " + character.MaxHealth;
        HpValue = (1.0f / character.MaxHealth) * character.Health;

        GameObject.Find("MP Text").GetComponent<Text>().text = character.Mana + " / " + character.MaxMana;
        MpValue = (1.0f / character.MaxMana) * character.Mana;
    }
}
