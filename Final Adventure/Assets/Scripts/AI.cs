using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Model;
using Assets.Scripts.Movement;
using Assets.Scripts.Util;
using UnityEditor;

public class AI : MonoBehaviour
{
    private List<Vector2> flatPath;
    private int _movementCount = 0;

    private Character _character;
    private Vector2 _position;
    private Character _targetCharacter;
    private Vector2 _targetPosition;

    private bool _attacking = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.GetComponent<Movement>().State == Movement.States.AtDestination && _attacking == false)
        {
            if (transform.GetComponent<CharacterHolder>().Turn.CompletedAction) return;
            CheckAttackRange();
        }
    }

    public void BeginTurn()
    {
        AnalyseOpposition();

        _attacking = false;
        _character= GameObject.Find("Characters").GetComponent<CharactersController>().CurrentCharacterHolder.Character;
        _targetCharacter = GameObject.Find("Characters").GetComponent<CharactersController>().CharacterHolders[0].Character;

        _targetPosition = _targetCharacter.XyPosition();
        _position = _character.XyPosition();

        float differenceInX = CalculatePositiveDifference(_position.x, _targetPosition.x);
        float differnceInY = CalculatePositiveDifference(_position.y, _targetPosition.y);
        float totalDifference = differenceInX + differnceInY;
        //1 is attack range
        if (totalDifference > 1f)
        {
            StartCoroutine(WalkTowardsPlayer());
        }
        else
        {
            CheckAttackRange();
        }
    }

    private void AnalyseOpposition()
    {
        var aiCharacterHolder = GameObject.Find("Characters").GetComponent<CharactersController>().CurrentCharacterHolder;
        BayesianProbability BP = new BayesianProbability(aiCharacterHolder);
        var prob = BP.CharacterProbabilities();
        foreach (var p in prob)
        {
            print("Attack = "  + p.Attack);
            print("Move = " + p.Move);
            print("Heal = " + p.Heal);
        }
    }

    private IEnumerator WalkTowardsPlayer()
    {
        yield return new WaitForSeconds(2.0f);
        GameObject.Find("Characters").GetComponent<CharactersController>().HighlightCharacterMovement();
        GameObject.Find("ActionBar").GetComponent<ActionBar>().State = MenuBar.States.Disabled;

        //This is for test purposes. Taking the position of the only other player in the game(None AI)

        bool[,] map = GameObject.Find("Floor").GetComponent<FloorHighlight>().FloorMap;
        SearchParameters searchParameters = new SearchParameters(_position, _targetPosition, map);

        AStar pathFinder = new AStar(searchParameters);
        flatPath = pathFinder.FindPath();
        if(flatPath.Count > _character.Speed) flatPath = flatPath.GetRange(0, _character.Speed);
        if(flatPath[flatPath.Count - 1] == _targetPosition) flatPath = flatPath.GetRange(0, flatPath.Count - 1);
        _movementCount = 0;
        StartCoroutine(MovePointer(flatPath[_movementCount], Move));
    }

    private IEnumerator MovePointer(Vector2 node, Action action)
    {
        yield return new WaitForSeconds(0.5f);

        GameObject.Find("Floor").GetComponent<FloorHighlight>().SetPointerPosition(node);

        _movementCount++;
        if (_movementCount < flatPath.Count) StartCoroutine(MovePointer(flatPath[_movementCount], action));
        else action();
    }

    private delegate void Action();

    private void Move()
    {
        StartCoroutine(MoveCharacter());
    }

    private IEnumerator MoveCharacter()
    {
        yield return new WaitForSeconds(0.5f);
        var floor = GameObject.Find("Floor").GetComponent<FloorHighlight>().FloorArray;
        var pointer = GameObject.Find("Floor").GetComponent<FloorHighlight>().PointerPosition;
        Tile tile = floor[(int) pointer.x, (int) pointer.y];

        transform.GetComponent<Movement>().SetPosition(tile, pointer);
        GameObject.Find("Characters").GetComponent<CharactersController>().CurrentCharacterHolder.Turn.Moved = true;
        GameObject.Find("Floor").GetComponent<FloorHighlight>().ResetFloorHighlight();
    }

    private void CheckAttackRange()
    {
        _targetPosition = _targetCharacter.XyPosition();
        _position = _character.XyPosition();

        float differenceInX = CalculatePositiveDifference(_position.x, _targetPosition.x);
        float differnceInY = CalculatePositiveDifference(_position.y, _targetPosition.y);
        float totalDifference = differenceInX + differnceInY;
        //1 is attack range
        if (totalDifference <= 1f)
        {
            _attacking = true;
            StartCoroutine(HighlightAttackRange());
        }
        else
        {
            Wait();
        }
    }

    private float CalculatePositiveDifference(float valueOne, float valueTwo)
    {
        if (valueOne > valueTwo)
        {
            return valueOne - valueTwo;
        }
        else
        {
            return valueTwo - valueOne;
        }

    }

    private IEnumerator HighlightAttackRange()
    {
        yield return new WaitForSeconds(1f);
        GameObject.Find("Characters").GetComponent<CharactersController>().HighlightCharacterAttackRange();
        GameObject.Find("ActionBar").GetComponent<ActionBar>().State = MenuBar.States.Disabled;


        bool[,] map = GameObject.Find("Floor").GetComponent<FloorHighlight>().FloorMap;
        SearchParameters searchParameters = new SearchParameters(_position, _targetPosition, map);

        AStar pathFinder = new AStar(searchParameters);
        flatPath = pathFinder.FindPath();
        //1 is attack range
        flatPath = flatPath.GetRange(0, 1);
        _movementCount = 0;
        StartCoroutine(MovePointer(flatPath[_movementCount], Attack));
    }

    private void Attack()
    {
        StartCoroutine(AttackCharacter());
    }

    private IEnumerator AttackCharacter()
    {
        yield return new WaitForSeconds(0.5f);
       
        var pointer = GameObject.Find("Floor").GetComponent<FloorHighlight>().PointerPosition;
        Abilities abilities = new Abilities(_character, pointer);
        abilities.Attack();

        GameObject.Find("Characters").GetComponent<CharactersController>().CurrentCharacterHolder.Turn.CompletedAction =
            true;

        GameObject.Find("ActionBar").GetComponent<ActionBar>().DisableAction();
        GameObject.Find("Floor").GetComponent<FloorHighlight>().ResetFloorHighlight();
        GameObject.Find("ActionBar").GetComponent<ActionBar>().Show();
    }

    private void Wait()
    {
        _attacking = false;
        GameObject.Find("Characters").GetComponent<CharactersController>().CurrentCharacterHolder.Turn.Skip();
    }
}
