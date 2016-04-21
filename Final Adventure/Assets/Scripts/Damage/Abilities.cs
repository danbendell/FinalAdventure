using UnityEngine;
using System.Collections;
using Assets.Scripts.Model;
using Assets.Scripts.Movement;

public class Abilities
{

    private Character _caster;
    private Vector2 _pointer;
    private Damage _damage;

    public Abilities(Character caster, Vector2 pointer)
    {
        _caster = caster;
        _pointer = pointer;
        _damage = GameObject.Find("Util").GetComponent<Damage>();
    }

    public void Heal()
    {
        CharactersController charactersController = GameObject.Find("Characters").GetComponent<CharactersController>();
        CharacterHolder characterHolder = charactersController.CurrentCharacterHolder;
        ParticleController particleController = GameObject.Find("Heal").GetComponent<ParticleController>();

        characterHolder.Turn.CompletedAction = _damage.Heal(_caster, _pointer);
        if (characterHolder.Turn.CompletedAction) particleController.Play(_pointer);

        GameObject.Find("Util").GetComponent<APIController>().SetAction("Heal");
        GameObject.Find("ActionBar").GetComponent<ActionBar>().DisableAction();
        GameObject.Find("Floor").GetComponent<FloorHighlight>().ResetFloorHighlight();
        GameObject.Find("ActionBar").GetComponent<ActionBar>().Show();
    }

    public void Flare()
    {
        CharactersController charactersController = GameObject.Find("Characters").GetComponent<CharactersController>();
        CharacterHolder characterHolder = charactersController.CurrentCharacterHolder;
        ParticleController particleController = GameObject.Find("Flare").GetComponent<ParticleController>();

        characterHolder.Turn.CompletedAction = _damage.Flare(_caster, _pointer);
        if (characterHolder.Turn.CompletedAction) particleController.Play(_pointer);

        GameObject.Find("Util").GetComponent<APIController>().SetAction("Flare");
        GameObject.Find("ActionBar").GetComponent<ActionBar>().DisableAction();
        GameObject.Find("Floor").GetComponent<FloorHighlight>().ResetFloorHighlight();
        GameObject.Find("ActionBar").GetComponent<ActionBar>().Show();
    }

    public void Wind()
    {
        ParticleController particleController = GameObject.Find("Wind").GetComponent<ParticleController>();
        particleController.Play(_pointer);
    }

    public void Focus()
    {
        CharactersController charactersController = GameObject.Find("Characters").GetComponent<CharactersController>();
        CharacterHolder characterHolder = charactersController.CurrentCharacterHolder;

        characterHolder.Turn.CompletedAction = _damage.Focus(_caster, _pointer);

        GameObject.Find("Util").GetComponent<APIController>().SetAction("Focus");
        GameObject.Find("ActionBar").GetComponent<ActionBar>().DisableAction();
        GameObject.Find("Floor").GetComponent<FloorHighlight>().ResetFloorHighlight();
        GameObject.Find("ActionBar").GetComponent<ActionBar>().Show();
    }

    public void Attack()
    {
        CharactersController charactersController = GameObject.Find("Characters").GetComponent<CharactersController>();
        CharacterHolder characterHolder = charactersController.CurrentCharacterHolder;
        characterHolder.Turn.CompletedAction = _damage.Attack(_caster, _pointer);

        GameObject.Find("Util").GetComponent<APIController>().SetAction("Attack");
        GameObject.Find("ActionBar").GetComponent<ActionBar>().DisableAction();
        GameObject.Find("Floor").GetComponent<FloorHighlight>().ResetFloorHighlight();
        GameObject.Find("ActionBar").GetComponent<ActionBar>().Show();
    }
    

}
