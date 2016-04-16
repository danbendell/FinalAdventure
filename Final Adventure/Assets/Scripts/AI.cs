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

    private int _minimumThreatValue;

    private bool _attacking = false;

   
    public class MapTile
    {
        public Vector2 Position { get; set; }
        public int ThreatLevel { get; set; }
        public int DistanceLevel { get; set; }
        public bool IsAvaliable { get; set; }

        public MapTile()
        {
            Position = new Vector2(0,0);
            ThreatLevel = 0;
            DistanceLevel = 0;
            IsAvaliable = true;
        }
    }

    private MapTile[,] _map;

    private delegate void Action();

    private void Attack()
    {
        StartCoroutine(AttackCharacter());
    }

    private void Heal()
    {
        StartCoroutine(HealCharacter());
    }

    private void Move()
    {
        StartCoroutine(MoveCharacter());
    }


    // Update is called once per frame
    void Update()
    {
        CharactersController cc = GameObject.Find("Characters").GetComponent<CharactersController>();
        if (transform.GetComponent<CharacterHolder>() != cc.CurrentCharacterHolder) return;
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
        
        //1 is attack range
        if (DistanceBetweenCharacters() > _character.AttackRange.y)
        {
            StartCoroutine(WalkTowardsPlayer());
        }
        else if (DistanceBetweenCharacters() < _character.AttackRange.x)
        {
            StartCoroutine(WalkAwayFromPlayer());
        }
        else
        {
            CheckAttackRange();
        }
    }

    private float DistanceBetweenCharacters()
    {
        float differenceInX = CalculatePositiveDifference(_position.x, _targetPosition.x);
        float differnceInY = CalculatePositiveDifference(_position.y, _targetPosition.y);
        float totalDifference = differenceInX + differnceInY;
        return totalDifference;
    }

    private float DistanceBetweenPoints(Vector2 start, Vector2 end)
    {
        float differenceInX = CalculatePositiveDifference(start.x, end.x);
        float differnceInY = CalculatePositiveDifference(start.y, end.y);
        float totalDifference = differenceInX + differnceInY;
        return totalDifference;
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
        var targetableCharacters = GetOppositionInAttackRange();
        
        targetableCharacters = OrderTargets(targetableCharacters);
        _targetCharacter = targetableCharacters[0].Character;
    }

    private List<CharacterHolder> GetOppositionInAttackRange()
    {
        CharactersController cc = GameObject.Find("Characters").GetComponent<CharactersController>();
        _character = cc.CurrentCharacterHolder.Character;

        var oppostion = GetOpposition();
        var targetableCharacters = new List<CharacterHolder>();
        foreach (var character in oppostion)
        {
            if (!InAttackRange(character, cc.CurrentCharacterHolder)) continue;
            targetableCharacters.Add(character);
        }

        return targetableCharacters;
    }

    private List<CharacterHolder> GetOpposition()
    {
        CharactersController cc = GameObject.Find("Characters").GetComponent<CharactersController>();
        var holders = cc.CharacterHolders;

        var opposition = new List<CharacterHolder>();
        foreach (var holder in holders)
        {
            if (holder.IsAi) continue;
            opposition.Add(holder);
        }

        return opposition;
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

        if (CalcPercentHP(healableCharacters[0]) < 0.25f)
        {
            _targetCharacter = healableCharacters[0].Character;
        }

        if (CalcPercentHP(healableCharacters[0]) < 0.5f && cc.CurrentCharacterHolder.Job == CharacterHolder.Jobs.Wizard)
        {
            _targetCharacter = healableCharacters[0].Character;
        }
    }

    private bool InAttackRange(CharacterHolder defender, CharacterHolder attacker)
    {
        var distanceFromAI = CalcDistance(defender, attacker);
        var attackRange = attacker.Character.AttackRange.y;
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

        SetFlatPath(new Vector2(0, _character.Speed));
        //Want to walk next to th player, not on them
        if(flatPath[flatPath.Count - 1] == _targetPosition) flatPath = flatPath.GetRange(0, flatPath.Count - 1);

        StartCoroutine(MovePointer(flatPath[_movementCount], Move));
    }

    private IEnumerator WalkAwayFromPlayer()
    {
        yield return new WaitForSeconds(2.0f);
        GameObject.Find("Characters").GetComponent<CharactersController>().HighlightCharacterMovement();
        GameObject.Find("ActionBar").GetComponent<ActionBar>().State = MenuBar.States.Disabled;

        var tempPosition = _targetPosition;
        _targetPosition = RunAwayFromTarget(_targetPosition);
        SetFlatPath(new Vector2(0, _character.Speed));
        _targetPosition = tempPosition;
        _movementCount = 0;
        StartCoroutine(MovePointer(flatPath[_movementCount], Move));

    }

    private IEnumerator HighlightAttackRange()
    {
        yield return new WaitForSeconds(1f);
        GameObject.Find("Characters").GetComponent<CharactersController>().HighlightCharacterAttackRange();
        GameObject.Find("ActionBar").GetComponent<ActionBar>().State = MenuBar.States.Disabled;
        
        SetFlatPath(_character.AttackRange);

        if (flatPath.Count == 0 && _targetCharacter == _character)
        {
            SelfCast();
        }
        else
        {
            StartCoroutine(MovePointer(flatPath[_movementCount], Attack));
        }
    }

    private IEnumerator MovePointer(Vector2 node, Action action)
    {
        yield return new WaitForSeconds(0.5f);

        GameObject.Find("Floor").GetComponent<FloorHighlight>().SetPointerPosition(node);

        _movementCount++;
        //Repeat this function until at destination
        if (_movementCount < flatPath.Count) StartCoroutine(MovePointer(flatPath[_movementCount], action));
        else action();
    }

    private Vector2 RunAwayFromTarget(Vector2 targetPosition)
    {
        var newPosition = new Vector2(0,0);
        //AR.x = 2  - DBC = 1
        //DNTM = 1
        var distanceNeedToMove = _character.AttackRange.x - DistanceBetweenCharacters();
       
        FillWithMovementPossibilities();

        FillWithThreatValues();

        FillWithDistanceValue();

        newPosition = FindNewPosition();
        
        return newPosition;
    }

    private void FillWithMovementPossibilities()
    {
        var width = GameObject.Find("Floor").GetComponent<FloorHighlight>().Width;
        var height = GameObject.Find("Floor").GetComponent<FloorHighlight>().Height;
        bool[,] map = GameObject.Find("Floor").GetComponent<FloorHighlight>().FloorMap;

        _map = new MapTile[width, height];
        InitMapTiles();

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                _map[x,y].Position = new Vector2(x,y);
                if (map[x, y] == false)
                {
                    _map[x, y].IsAvaliable = false;
                    continue;
                }
                var distance = DistanceBetweenPoints(_position, new Vector2(x, y));
                if (distance > _character.Speed)
                {
                    map[x, y] = false;
                    _map[x, y].IsAvaliable = false;
                }
            }
        }
    }

    private void InitMapTiles()
    {
        for (var x = 0; x < _map.GetLength(0); x++)
        {
            for (var y = 0; y < _map.GetLength(1); y++)
            {
                _map[x,y] = new MapTile();
            }
        }
    }

    private void FillWithThreatValues()
    {
        var opposition = GetOpposition();
        foreach (CharacterHolder holder in opposition)
        {
            InsertCharacterThreatValues(holder.Character.XyPosition());
        }
        
    }

    private void FillWithDistanceValue()
    {
        var opposition = GetOpposition();
        foreach (CharacterHolder holder in opposition)
        {
            for (var x = 0; x < _map.GetLength(0); x++)
            {
                for (var y = 0; y < _map.GetLength(1); y++)
                {
                    var distance = (int)DistanceBetweenPoints(new Vector2(x,y), holder.Character.XyPosition());
                    _map[x, y].DistanceLevel += distance;
                }
            }
        }
    }

    private void InsertCharacterThreatValues(Vector2 position)
    {
        var width = GameObject.Find("Floor").GetComponent<FloorHighlight>().Width;
        var height = GameObject.Find("Floor").GetComponent<FloorHighlight>().Height;

        var playerThreatValue = 4;
        
        Vector2 startPosition = new Vector2(position.x - (playerThreatValue - 1), position.y - (playerThreatValue - 1));
        var squareSize = (playerThreatValue*2) - 1;
        for (var x = 0; x < squareSize; x++)
        {
            for (var y = 0; y < squareSize; y++)
            {
                var newX = startPosition.x + x;
                var newY = startPosition.y + y;
                if (newX > width - 1 || newX < 0) continue;
                if (newY > height - 1 || newY < 0) continue;

                var distance = DistanceBetweenPoints(new Vector2(newX, newY), position);
                if (distance < playerThreatValue)
                {
                    _map[(int)newX, (int) newY].ThreatLevel += (playerThreatValue - (int) distance);
                }
            }
        }
    }

    private Vector2 FindNewPosition()
    {
        var width = GameObject.Find("Floor").GetComponent<FloorHighlight>().Width;
        var height = GameObject.Find("Floor").GetComponent<FloorHighlight>().Height;
        List<MapTile> potentiaMapTiles = new List<MapTile>();

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                if (_map[x, y].IsAvaliable == false) continue;
                var distance = DistanceBetweenPoints(_targetPosition, new Vector2(x, y));
                if (distance <= _character.AttackRange.y && distance >= _character.AttackRange.x)
                {
                    potentiaMapTiles.Add(_map[x,y]);
                }

            }
        }
        potentiaMapTiles = new List<MapTile>(potentiaMapTiles.OrderByDescending(p => p.DistanceLevel).ThenBy(p => p.ThreatLevel));

        return potentiaMapTiles[0].Position;
    }

    private void CheckAttackRange()
    {
        _targetPosition = _targetCharacter.XyPosition();
        _position = _character.XyPosition();

        //1 is attack range
        if (DistanceBetweenCharacters() <= _character.AttackRange.y && DistanceBetweenCharacters() >= _character.AttackRange.x)
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

    private void SetFlatPath(Vector2 range)
    {
        bool[,] map = GameObject.Find("Floor").GetComponent<FloorHighlight>().FloorMap;
        map[(int)_targetPosition.x, (int)_targetPosition.y] = true;

        SearchParameters searchParameters = new SearchParameters(_position, _targetPosition, map);

        AStar pathFinder = new AStar(searchParameters);
        flatPath = pathFinder.FindPath();
        if (flatPath.Count > range.y) flatPath = flatPath.GetRange((int) range.x, (int) (range.y - range.x));
        _movementCount = 0;
    } 

    private void SelfCast()
    {
        GameObject.Find("Floor").GetComponent<FloorHighlight>().SetPointerPosition(_position);
        Heal();
    }

    private IEnumerator MoveCharacter()
    {
        yield return new WaitForSeconds(0.5f);
        var floor = GameObject.Find("Floor").GetComponent<FloorHighlight>().FloorArray;
        var pointer = GameObject.Find("Floor").GetComponent<FloorHighlight>().PointerPosition;
        Tile tile = floor[(int)pointer.x, (int)pointer.y];

        transform.GetComponent<Movement>().SetPosition(tile, pointer);
        GameObject.Find("Characters").GetComponent<CharactersController>().CurrentCharacterHolder.Turn.Moved = true;
        GameObject.Find("Floor").GetComponent<FloorHighlight>().ResetFloorHighlight();

        //This is waiting for the character to move into position, continues in the update function
    }

    private IEnumerator AttackCharacter()
    {
        yield return new WaitForSeconds(0.5f);
       
        var pointer = GameObject.Find("Floor").GetComponent<FloorHighlight>().PointerPosition;
        Abilities abilities = new Abilities(_character, pointer);
        abilities.Attack();

        CheckNextAction();
    }

    private IEnumerator HealCharacter()
    {
        yield return new WaitForSeconds(0.5f);

        var pointer = GameObject.Find("Floor").GetComponent<FloorHighlight>().PointerPosition;
        Abilities abilities = new Abilities(_character, pointer);
        abilities.Heal();

        CheckNextAction();
    }

    private void CheckNextAction()
    {
        CharactersController cc = GameObject.Find("Characters").GetComponent<CharactersController>();
        CharacterHolder ch = cc.CurrentCharacterHolder;
        if (!ch.Turn.Moved) CheckMovement();
    }

    private void CheckMovement()
    {
        //Need to check in here to see if the AI wants to move away from its current position.
        //For now it is going to end its go
        Wait();
    }

    private void Wait()
    {
        _attacking = false;
        GameObject.Find("Characters").GetComponent<CharactersController>().CurrentCharacterHolder.Turn.Skip();
    }
}
