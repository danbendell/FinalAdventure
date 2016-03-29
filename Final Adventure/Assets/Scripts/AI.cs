using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private delegate void Action();

    private void Attack()
    {
        StartCoroutine(AttackCharacter());
    }

    private void Move()
    {
        StartCoroutine(MoveCharacter());
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
        ResetValues();
        AnalyseOpposition();
        FindTarget();

       
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

    private void ResetValues()
    {
        transform.GetComponent<Movement>().State = Movement.States.NotMoved;
        _attacking = false;
        CharactersController cc = GameObject.Find("Characters").GetComponent<CharactersController>();
        _character = cc.CurrentCharacterHolder.Character;
    }

    private void AnalyseOpposition()
    {
        var aiCharacterHolder = GameObject.Find("Characters").GetComponent<CharactersController>().CurrentCharacterHolder;
        BayesianProbability BP = new BayesianProbability(aiCharacterHolder);
        //var prob = BP.CharacterProbabilities();
        //foreach (var p in prob)
        //{
        //    print("Job = " + p.Job);
        //    print("Attack = "  + p.Attack);
        //    print("Move = " + p.Move);
        //    print("Heal = " + p.Heal);
        //}
    }

    private void FindTarget()
    {
        FindOpposition();
        FindAlliesForHealing();
    }

    private void FindOpposition()
    {
        CharactersController cc = GameObject.Find("Characters").GetComponent<CharactersController>();
        var holders = cc.CharacterHolders;
        _character = cc.CurrentCharacterHolder.Character;
        var targetableCharacters = new List<CharacterHolder>();
        foreach (var holder in holders)
        {
            if (holder.IsAi) continue;
            if (!InAttackRange(holder, cc.CurrentCharacterHolder)) continue;
            targetableCharacters.Add(holder);
        }

        //No opposition in range of attack
        if (targetableCharacters.Count <= 0) return;
        
        targetableCharacters = OrderTargets(targetableCharacters);
        _targetCharacter = targetableCharacters[0].Character;
    }

    private void FindAlliesForHealing()
    {
        CharactersController cc = GameObject.Find("Characters").GetComponent<CharactersController>();
        var holders = cc.CharacterHolders;
        _character = cc.CurrentCharacterHolder.Character;
        var healableCharacters = new List<CharacterHolder>();
        foreach (var holder in holders)
        {
            //Check they are AI
            if (!holder.IsAi) continue;
            //Check they are in range
            if (!InAttackRange(holder, cc.CurrentCharacterHolder)) continue;
            //Check they aren't full health
            if (holder.Character.Health == holder.Character.MaxHealth) continue;
            
            healableCharacters.Add(holder);
        }

        //No opposition in range of attack
        if (healableCharacters.Count <= 0) return;
        //Order them by health lost
        healableCharacters = healableCharacters.OrderByDescending(x => CalcPercentHP(x)).ToList();
        _targetCharacter = healableCharacters[0].Character;
    }

    private bool InAttackRange(CharacterHolder defender, CharacterHolder attacker)
    {
        var distanceFromAI = CalcDistance(defender, attacker);
        var attackRange = attacker.Character.AttackRange;
        var movementRange = attacker.Character.Speed;
        return (attackRange + movementRange) > distanceFromAI;
    }

    private float CalcDistance(CharacterHolder characterHolder, CharacterHolder ai)
    {
        var characterPos = characterHolder.Character.XyPosition();
        var _AIPos = ai.Character.XyPosition();

        float differenceInX = CalculatePositiveDifference(_AIPos.x, characterPos.x);
        float differnceInY = CalculatePositiveDifference(_AIPos.y, characterPos.y);
        return differenceInX + differnceInY;
    }

    private List<CharacterHolder> OrderTargets(List<CharacterHolder> holders)
    {
        var missingHpMod = 3.0f;
        var healChanceMod = 2.0f;
        var attackChanceMod = 1.0f;

        foreach (var holder in holders)
        {
            //Gives a value from 0-3. Low target characters are a high priority
            var missingHealthPriority = CalcPriority(1f - CalcPercentHP(holder), missingHpMod);
            //Gives a value from 0-2. Healers are mid priority
            var healChancePriority = CalcPriority(holder.Probabilities.Heal, healChanceMod);
            //Gives a value from 1-0. All attacks are standard priority
            var attackChancePriority = CalcPriority(holder.Probabilities.Attack, attackChanceMod);
            //A value out of 6 given the sum of the above 
            holder.PriorityLevel = missingHealthPriority + healChancePriority + attackChancePriority;
        }
        
        return holders.OrderByDescending(x => x.PriorityLevel).ToList();
    }

    private float CalcPercentHP(CharacterHolder characterHolder)
    {
        float onePercent = 1f / characterHolder.Character.MaxHealth;
        float currentPercent = onePercent * characterHolder.Character.Health;
        return currentPercent;
    }

    private float CalcPriority(float value, float mod)
    {
        return (value*mod);
    }


    private IEnumerator WalkTowardsPlayer()
    {
        yield return new WaitForSeconds(2.0f);
        GameObject.Find("Characters").GetComponent<CharactersController>().HighlightCharacterMovement();
        GameObject.Find("ActionBar").GetComponent<ActionBar>().State = MenuBar.States.Disabled;

        //This is for test purposes. Taking the position of the only other player in the game(None AI)

        bool[,] map = GameObject.Find("Floor").GetComponent<FloorHighlight>().FloorMap;
        map[(int)_targetPosition.x, (int)_targetPosition.y] = true;
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
        map[(int)_targetPosition.x, (int)_targetPosition.y] = true;

        SearchParameters searchParameters = new SearchParameters(_position, _targetPosition, map);

        AStar pathFinder = new AStar(searchParameters);
        flatPath = pathFinder.FindPath();
        //1 is attack range
        if (flatPath.Count > 1)
        {
            flatPath = flatPath.GetRange(0, 1);
        }
        _movementCount = 0;
        StartCoroutine(MovePointer(flatPath[_movementCount], Attack));
    }

    private IEnumerator AttackCharacter()
    {
        yield return new WaitForSeconds(0.5f);
       
        var pointer = GameObject.Find("Floor").GetComponent<FloorHighlight>().PointerPosition;
        Abilities abilities = new Abilities(_character, pointer);
        abilities.Attack();
    }

    private void Wait()
    {
        _attacking = false;
        GameObject.Find("Characters").GetComponent<CharactersController>().CurrentCharacterHolder.Turn.Skip();
    }
}
