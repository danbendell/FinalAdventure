using UnityEngine;
using System.Collections;
using Assets.Scripts.Damage.Abilities;
using Assets.Scripts.Model;
using Assets.Scripts.Movement;

public class Abilities
{

    private Character _caster;
    private Vector2 _pointer;
    private Damage _damage;
    private SoundUtil _sounds;

    public Abilities(Character caster, Vector2 pointer)
    {
        _caster = caster;
        _pointer = pointer;
        _damage = GameObject.Find("Util").GetComponent<Damage>();
        _sounds = GameObject.Find("Sounds").GetComponent<SoundUtil>();
    }

    public void Heal()
    {
        CharactersController charactersController = GameObject.Find("Characters").GetComponent<CharactersController>();
        CharacterHolder characterHolder = charactersController.CurrentCharacterHolder;
        ParticleController particleController = GameObject.Find("Heal").GetComponent<ParticleController>();

        characterHolder.Turn.CompletedAction = _damage.Heal(_caster, _pointer);
        if (characterHolder.Turn.CompletedAction)
        {
            particleController.Play(_pointer);
            _sounds.PlaySound(SoundUtil.Sounds.Heal);
        }

        APIController.SetAction("Heal");
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
        if (characterHolder.Turn.CompletedAction)
        {
            particleController.Play(_pointer);
            _sounds.PlaySound(SoundUtil.Sounds.Fire);
        }

        APIController.SetAction("Flare");
        GameObject.Find("ActionBar").GetComponent<ActionBar>().DisableAction();
        GameObject.Find("Floor").GetComponent<FloorHighlight>().ResetFloorHighlight();
        GameObject.Find("ActionBar").GetComponent<ActionBar>().Show();
    }

    public void Wind()
    {
        CharactersController charactersController = GameObject.Find("Characters").GetComponent<CharactersController>();
        CharacterHolder characterHolder = charactersController.CurrentCharacterHolder;
        ParticleController particleController = GameObject.Find("Wind").GetComponent<ParticleController>();

        characterHolder.Turn.CompletedAction = _damage.Wind(_caster, _pointer);
        if (characterHolder.Turn.CompletedAction)
        {
            particleController.Play(_pointer);
            _sounds.PlaySound(SoundUtil.Sounds.Wind);
        }

        APIController.SetAction("Wind");
        GameObject.Find("ActionBar").GetComponent<ActionBar>().DisableAction();
        GameObject.Find("Floor").GetComponent<FloorHighlight>().ResetFloorHighlight();
        GameObject.Find("ActionBar").GetComponent<ActionBar>().Show();
    }

    public void Focus()
    {
        CharactersController charactersController = GameObject.Find("Characters").GetComponent<CharactersController>();
        CharacterHolder characterHolder = charactersController.CurrentCharacterHolder;

        characterHolder.Turn.CompletedAction = _damage.Focus(_caster, _pointer);
        if (characterHolder.Turn.CompletedAction)
        {
            _sounds.PlaySound(SoundUtil.Sounds.Attack);
        }

        APIController.SetAction("Focus");
        GameObject.Find("ActionBar").GetComponent<ActionBar>().DisableAction();
        GameObject.Find("Floor").GetComponent<FloorHighlight>().ResetFloorHighlight();
        GameObject.Find("ActionBar").GetComponent<ActionBar>().Show();
    }

    public void Slash()
    {
        CharactersController charactersController = GameObject.Find("Characters").GetComponent<CharactersController>();
        CharacterHolder characterHolder = charactersController.CurrentCharacterHolder;

        characterHolder.Turn.CompletedAction = _damage.Slash(_caster, _pointer);
        if (characterHolder.Turn.CompletedAction)
        {
            _sounds.PlaySound(SoundUtil.Sounds.Attack);
        }

        APIController.SetAction("Slash");
        GameObject.Find("ActionBar").GetComponent<ActionBar>().DisableAction();
        GameObject.Find("Floor").GetComponent<FloorHighlight>().ResetFloorHighlight();
        GameObject.Find("ActionBar").GetComponent<ActionBar>().Show();
    }

    public void Assassinate()
    {
        CharactersController charactersController = GameObject.Find("Characters").GetComponent<CharactersController>();
        CharacterHolder characterHolder = charactersController.CurrentCharacterHolder;

        characterHolder.Turn.CompletedAction = _damage.Assassinate(_caster, _pointer);
        if (characterHolder.Turn.CompletedAction)
        {
            _sounds.PlaySound(SoundUtil.Sounds.Attack);
        }

        APIController.SetAction("Assassinate");
        GameObject.Find("ActionBar").GetComponent<ActionBar>().DisableAction();
        GameObject.Find("Floor").GetComponent<FloorHighlight>().ResetFloorHighlight();
        GameObject.Find("ActionBar").GetComponent<ActionBar>().Show();
    }

    public void BloodBlade()
    {
        CharactersController charactersController = GameObject.Find("Characters").GetComponent<CharactersController>();
        CharacterHolder characterHolder = charactersController.CurrentCharacterHolder;

        characterHolder.Turn.CompletedAction = _damage.BloodBlade(_caster, _pointer);
        if (characterHolder.Turn.CompletedAction)
        {
            _sounds.PlaySound(SoundUtil.Sounds.Attack);
        }

        APIController.SetAction("BloodBlade");
        GameObject.Find("ActionBar").GetComponent<ActionBar>().DisableAction();
        GameObject.Find("Floor").GetComponent<FloorHighlight>().ResetFloorHighlight();
        GameObject.Find("ActionBar").GetComponent<ActionBar>().Show();
    }

    public void Attack()
    {
        CharactersController charactersController = GameObject.Find("Characters").GetComponent<CharactersController>();
        CharacterHolder characterHolder = charactersController.CurrentCharacterHolder;
        characterHolder.Turn.CompletedAction = _damage.Attack(_caster, _pointer);
        if (characterHolder.Turn.CompletedAction)
        {
            _sounds.PlaySound(SoundUtil.Sounds.Attack);
        }

        APIController.SetAction("Attack");
        GameObject.Find("ActionBar").GetComponent<ActionBar>().DisableAction();
        GameObject.Find("Floor").GetComponent<FloorHighlight>().ResetFloorHighlight();
        GameObject.Find("ActionBar").GetComponent<ActionBar>().Show();
    }
    
    

}
