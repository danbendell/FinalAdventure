using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Damage;
using Assets.Scripts.Damage.Abilities;
using Assets.Scripts.Damage.Magic;
using Assets.Scripts.Model;
using Assets.Scripts.Movement;
using Assets.Scripts.Util;
using Flare = Assets.Scripts.Damage.Flare;

public class AI : MonoBehaviour
{
    private List<Vector2> flatPath;
    private int _movementCount = 0;

    private Character _character;
    private Vector2 _position;
    private Character _targetCharacter;
    private Vector2 _targetPosition;

    private bool _isTargetOpposition;
    private bool _finishedMoving;

    private int _minimumThreatValue;
   
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

    private void AttackCoroutine()
    {
        StartCoroutine(AttackCharacter());
    }

    private void FlareCoroutine()
    {
        StartCoroutine(FlareCharacter());
    }

    private void WindCoroutine()
    {
        StartCoroutine(WindCharacter());
    }

    private void HealCoroutine()
    {
        StartCoroutine(HealCharacter());
    }

    private void FocusCoroutine()
    {
        StartCoroutine(FocusCharacter());
    }
    private void SlashCoroutine()
    {
        StartCoroutine(SlashCharacter());
    }

    private void AssassinateCoroutine()
    {
        StartCoroutine(AssassinateCharacter());
    }

    private void BloodBladeCoroutine()
    {
        StartCoroutine(BloodBladeCharacter());
    }

    private void MoveCoroutine()
    {
        StartCoroutine(MoveCharacter());
    }


    // Update is called once per frame
    void Update()
    {
        CharactersController cc = GameObject.Find("Characters").GetComponent<CharactersController>();
        if (transform.GetComponent<CharacterHolder>() != cc.CurrentCharacterHolder) return;
        if (transform.GetComponent<Movement>().State == Movement.States.AtDestination && _finishedMoving == false)
        {
            print("FINSIHED MOVING");
            _finishedMoving = true;
            if (transform.GetComponent<CharacterHolder>().Turn.CompletedAction) return;
            _position = _character.XyPosition();
            var distance = CalculateUtil.CalcDistance(_character, _targetCharacter);
            CompleteAction(distance);
        }
    }

    public IEnumerator BeginTurn()
    {
        yield return new WaitForSeconds(2.5f);

        print("AI BEGIN TURN");
        CharactersController cc = GameObject.Find("Characters").GetComponent<CharactersController>();

        ResetValues();
        AnalyseOpposition();
        AnalyseData();
        FindTarget();

        print("FOUND TARGET");

        _targetPosition = _targetCharacter.XyPosition();
        _position = _character.XyPosition();

        print("TARGET POSITION - " + _targetPosition);
        print("AI POSITION - " + _position);

        AnalyseOptions();
    }

    private void AnalyseOptions()
    {
        var distance = CalculateUtil.CalcDistance(_character, _targetCharacter);
        if (distance > _character.AttackRange.y || distance < _character.AttackRange.x)
        {
            //Move
            print("MOVE FIRST");
            MoveIntoAttackRange(distance);
        }
        else
        {
            //Action
            print("COMPLETE ACTION FIRST");
            CompleteAction(distance);
        }
    }

    private void MoveIntoAttackRange(float distance)
    {
        if (distance > _character.AttackRange.y)
        {
            StartCoroutine(WalkTowardsPlayer());
        }
        else if (distance < _character.AttackRange.x)
        {
            StartCoroutine(WalkAwayFromPlayer());
        }
    }

    private void CompleteAction(float distance)
    {
        if (distance == 0)
        {
            print("SELF CAST HEAL");
            //SelfCast
            SelfCast();
            return;
        }
        //Decide what action to complete
        if (_isTargetOpposition)
        {
            print("ATTACKING OPPOSITION");
            if (distance <= _character.AttackRange.y && distance >= _character.AttackRange.x)
            {
                StartCoroutine(HighlightAttackRange());
            }
            else
            {
                print("TARGET NOT IN RANGE");
                StartCoroutine(Wait());
            }
        }
        else
        {
            //This is an Ally
            print("NEED TO HEAL");
            StartCoroutine(HighlightAttackRange());
        }

    }

    private float DistanceBetweenPoints(Vector2 start, Vector2 end)
    {
        float differenceInX = CalculateUtil.CalculatePositiveDifference(start.x, end.x);
        float differnceInY = CalculateUtil.CalculatePositiveDifference(start.y, end.y);
        float totalDifference = differenceInX + differnceInY;
        return totalDifference;
    }

    private void ResetValues()
    {
        transform.GetComponent<Movement>().State = Movement.States.NotMoved;
        _finishedMoving = false;
        CharactersController cc = GameObject.Find("Characters").GetComponent<CharactersController>();
        _character = cc.CurrentCharacterHolder.Character;
    }

    private void AnalyseOpposition()
    {
        var aiCharacterHolder = GameObject.Find("Characters").GetComponent<CharactersController>().CurrentCharacterHolder;
        BayesianProbability BP = new BayesianProbability(aiCharacterHolder);
        //var prob = BP.Probabilities();
        //foreach (var p in prob)
        //{
        //    print("Job = " + p.Job);
        //    print("Attack = "  + p.AttackCoroutine);
        //    print("Move = " + p.MoveCoroutine);
        //    print("Heal = " + p.HealCoroutine);
        //}
    }

    private void AnalyseData()
    {
        print(APIController.TurnList);
        //if (APIController.TurnList.Count < 10) return;
        if (APIController.TurnList.Count > 0)
        {
            var orderedTurnList =
                APIController.TurnList.GroupBy(x => x.Action).OrderByDescending(g => g.Count()).SelectMany(g => g).ToList();
        }


    }

    private void FindTarget()
    {
        _isTargetOpposition = FindTargetableOpposition();

        if (_isTargetOpposition == false)
        {
            _isTargetOpposition = FindOutOfRangeOpposition();
        }

        _isTargetOpposition = !FindAlliesForHealing();
    }

    private bool FindTargetableOpposition()
    {
        var targetableCharacters = GetOppositionInAttackRange();
        
        targetableCharacters = OrderTargets(targetableCharacters);
        if (targetableCharacters.Count == 0) return false;

        _targetCharacter = targetableCharacters[0].Character;
        return true;
    }

    private bool FindOutOfRangeOpposition()
    {

        var targetableCharacters = GetOpposition();

        targetableCharacters = OrderTargets(targetableCharacters);
        if (targetableCharacters.Count == 0) return false;

        _targetCharacter = targetableCharacters[0].Character;
        return true;
    }

    private List<CharacterHolder> GetOppositionInAttackRange()
    {
        CharactersController cc = GameObject.Find("Characters").GetComponent<CharactersController>();
        _character = cc.CurrentCharacterHolder.Character;

        var oppostion = GetOpposition();
        var targetableCharacters = new List<CharacterHolder>();
        foreach (var character in oppostion)
        {
            if (!CalculateUtil.InAttackRange(character, cc.CurrentCharacterHolder)) continue;
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

    private List<CharacterHolder> GetAlliesInHealRange()
    {
        CharactersController cc = GameObject.Find("Characters").GetComponent<CharactersController>();
        _character = cc.CurrentCharacterHolder.Character;

        var allies = GetAllies();
        var inRangeAllies = new List<CharacterHolder>();
        foreach (var character in allies)
        {
            if (!CalculateUtil.InAttackRange(character, cc.CurrentCharacterHolder)) continue;
            inRangeAllies.Add(character);
        }

        return inRangeAllies;
    }

    private List<CharacterHolder> GetAllies()
    {
        CharactersController cc = GameObject.Find("Characters").GetComponent<CharactersController>();
        var holders = cc.CharacterHolders;

        var allies = new List<CharacterHolder>();
        foreach (var holder in holders)
        {
            if (!holder.IsAi) continue;
            allies.Add(holder);
        }

        return allies;
    }

    private bool FindAlliesForHealing()
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
            if (!CalculateUtil.InAttackRange(holder, cc.CurrentCharacterHolder)) continue;
            //Check they aren't full health
            if (holder.Character.Health == holder.Character.MaxHealth) continue;

            healableCharacters.Add(holder);
        }

        //No opposition in range of attack
        if (healableCharacters.Count <= 0) return false;

        //Order them by health lost
        healableCharacters = healableCharacters.OrderByDescending(x => CalculateUtil.CalcPercentHP(x)).ToList();

        if (CalculateUtil.CalcPercentHP(healableCharacters[0]) < 0.25f)
        {
            _targetCharacter = healableCharacters[0].Character;
            return true;
        }

        if (CalculateUtil.CalcPercentHP(healableCharacters[0]) < 0.5f && cc.CurrentCharacterHolder.Job == CharacterHolder.Jobs.Wizard)
        {
            _targetCharacter = healableCharacters[0].Character;
            return true;
        }

        return false;
    }

    private List<CharacterHolder> OrderTargets(List<CharacterHolder> holders)
    {
        var missingHpMod = 3.0f;
        var healChanceMod = 2.0f;
        var attackChanceMod = 1.0f;

        foreach (var holder in holders)
        {
            //Gives a value from 0-3. Low target characters are a high priority
            var missingHealthPriority = CalcPriority(1f - CalculateUtil.CalcPercentHP(holder), missingHpMod);
            //Gives a value from 0-2. Healers are mid priority
            var healChancePriority = CalcPriority(holder.Probabilities.Heal, healChanceMod);
            //Gives a value from 1-0. All attacks are standard priority
            var attackChancePriority = CalcPriority(holder.Probabilities.Attack, attackChanceMod);
            //A value out of 6 given the sum of the above 
            holder.PriorityLevel = missingHealthPriority + healChancePriority + attackChancePriority;
        }
        
        return holders.OrderByDescending(x => x.PriorityLevel).ToList();
    }

    private float CalcPriority(float value, float mod)
    {
        return (value*mod);
    }


    private IEnumerator WalkTowardsPlayer()
    {
        yield return new WaitForSeconds(2.0f);
        if (IsCurrentCharacterHolder() == false) yield break;

        GameObject.Find("Characters").GetComponent<CharactersController>().HighlightCharacterMovement();
        GameObject.Find("ActionBar").GetComponent<ActionBar>().State = MenuBar.States.Disabled;

        SetFlatPath(new Vector2(0, _character.Speed));
        //Want to walk next to th player, not on them
        if(flatPath[flatPath.Count - 1] == _targetPosition) flatPath = flatPath.GetRange(0, flatPath.Count - 1);

        StartCoroutine(MovePointer(flatPath[_movementCount], MoveCoroutine));
    }

    private IEnumerator WalkAwayFromPlayer()
    {
        yield return new WaitForSeconds(2.0f);
        if (IsCurrentCharacterHolder() == false) yield break;

        GameObject.Find("Characters").GetComponent<CharactersController>().HighlightCharacterMovement();
        GameObject.Find("ActionBar").GetComponent<ActionBar>().State = MenuBar.States.Disabled;

        var tempPosition = _targetPosition;
        _targetPosition = RunAwayFromTarget(_targetPosition);
        if (_targetPosition == _character.XyPosition())
        {
            //Aready in the best position;
            StartCoroutine(Wait());
            yield break;
        }
        SetFlatPath(new Vector2(0, _character.Speed));
        _targetPosition = tempPosition;
        _movementCount = 0;
        StartCoroutine(MovePointer(flatPath[_movementCount], MoveCoroutine));

    }

    private IEnumerator HighlightAttackRange()
    {
        yield return new WaitForSeconds(1f);
        if (IsCurrentCharacterHolder() == false) yield break;

        GameObject.Find("Characters").GetComponent<CharactersController>().HighlightCharacterAttackRange();
        GameObject.Find("ActionBar").GetComponent<ActionBar>().State = MenuBar.States.Disabled;
        
        SetFlatPath(_character.AttackRange);

        if (flatPath.Count == 0 && _targetCharacter == _character)
        {
            SelfCast();
        }
        else if (!_isTargetOpposition)
        {
            StartCoroutine(MovePointer(flatPath[_movementCount], HealCoroutine));
        }
        else
        {
            var action = PickAnAction();
            StartCoroutine(MovePointer(flatPath[_movementCount], action));
        }
    }

    private Action PickAnAction()
    {
        Ability ability = _character.GetRandomAbility();

        if (ability != null)
        {
            Focus focus = new Focus();
            if (ability.Name == focus.Name) return this.FocusCoroutine;

            Slash slash = new Slash();
            if (ability.Name == slash.Name) return this.SlashCoroutine;

            Assassinate assassinate = new Assassinate();
            if (ability.Name == assassinate.Name) return this.AssassinateCoroutine;

            BloodBlade bloodBlade = new BloodBlade();
            if (ability.Name == bloodBlade.Name) return this.BloodBladeCoroutine;
        }
        

        Spell spell = _character.GetRandomSpell();

        if (spell != null)
        {
             Flare flare = new Flare();
            if (spell.Name == flare.Name) return this.FlareCoroutine;

            Wind wind = new Wind();
            if (spell.Name == wind.Name) return this.WindCoroutine;
        }

        return AttackCoroutine;
    }

    private IEnumerator MovePointer(Vector2 node, Action action)
    {
        yield return new WaitForSeconds(0.5f);
        if (IsCurrentCharacterHolder() == false) yield break;

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
        var distanceNeedToMove = _character.AttackRange.x - CalculateUtil.CalcDistance(_character, _targetCharacter);
       
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
        HealCoroutine();
    }

    private IEnumerator MoveCharacter()
    {
        yield return new WaitForSeconds(0.5f);
        if (IsCurrentCharacterHolder() == false) yield break;

        var floor = GameObject.Find("Floor").GetComponent<FloorHighlight>().FloorArray;
        var pointer = GameObject.Find("Floor").GetComponent<FloorHighlight>().PointerPosition;
        Tile tile = floor[(int)pointer.x, (int)pointer.y];

        Turn turn = GameObject.Find("Characters").GetComponent<CharactersController>().CurrentCharacterHolder.Turn;
        transform.GetComponent<Movement>().SetPosition(tile, pointer, turn);
       
        GameObject.Find("Floor").GetComponent<FloorHighlight>().ResetFloorHighlight();

        //This is waiting for the character to move into position, continues in the update function
    }

    private IEnumerator AttackCharacter()
    {
        yield return new WaitForSeconds(0.5f);
        if (IsCurrentCharacterHolder() == false) yield break;

        var pointer = GameObject.Find("Floor").GetComponent<FloorHighlight>().PointerPosition;
        Abilities abilities = new Abilities(_character, pointer);
        abilities.Attack();

        CheckMovement();
    }

    private IEnumerator HealCharacter()
    {
        yield return new WaitForSeconds(0.5f);
        if (IsCurrentCharacterHolder() == false) yield break;

        var pointer = GameObject.Find("Floor").GetComponent<FloorHighlight>().PointerPosition;
        Abilities abilities = new Abilities(_character, pointer);
        abilities.Heal();

        CheckMovement();
    }

    private IEnumerator WindCharacter()
    {
        yield return new WaitForSeconds(0.5f);
        if (IsCurrentCharacterHolder() == false) yield break;

        var pointer = GameObject.Find("Floor").GetComponent<FloorHighlight>().PointerPosition;
        Abilities abilities = new Abilities(_character, pointer);
        abilities.Wind();

        CheckMovement();
    }

    private IEnumerator FlareCharacter()
    {
        yield return new WaitForSeconds(0.5f);
        if (IsCurrentCharacterHolder() == false) yield break;

        var pointer = GameObject.Find("Floor").GetComponent<FloorHighlight>().PointerPosition;
        Abilities abilities = new Abilities(_character, pointer);
        abilities.Flare();

        CheckMovement();
    }

    private IEnumerator FocusCharacter()
    {
        yield return new WaitForSeconds(0.5f);
        if (IsCurrentCharacterHolder() == false) yield break;

        var pointer = GameObject.Find("Floor").GetComponent<FloorHighlight>().PointerPosition;
        Abilities abilities = new Abilities(_character, pointer);
        abilities.Focus();

        CheckMovement();
    }

    private IEnumerator SlashCharacter()
    {
        yield return new WaitForSeconds(0.5f);
        if (IsCurrentCharacterHolder() == false) yield break;

        var pointer = GameObject.Find("Floor").GetComponent<FloorHighlight>().PointerPosition;
        Abilities abilities = new Abilities(_character, pointer);
        abilities.Slash();

        CheckMovement();
    }

    private IEnumerator AssassinateCharacter()
    {
        yield return new WaitForSeconds(0.5f);
        if(IsCurrentCharacterHolder() == false) yield break;

        var pointer = GameObject.Find("Floor").GetComponent<FloorHighlight>().PointerPosition;
        Abilities abilities = new Abilities(_character, pointer);
        abilities.Assassinate();

        CheckMovement();
    }

    private IEnumerator BloodBladeCharacter()
    {
        yield return new WaitForSeconds(0.5f);
        if (IsCurrentCharacterHolder() == false) yield break;

        var pointer = GameObject.Find("Floor").GetComponent<FloorHighlight>().PointerPosition;
        Abilities abilities = new Abilities(_character, pointer);
        abilities.BloodBlade();

        CheckMovement();
    }

    private void CheckMovement()
    {

        CharactersController cc = GameObject.Find("Characters").GetComponent<CharactersController>();
        CharacterHolder ch = cc.CurrentCharacterHolder;
        CharacterHolder thisCH = transform.GetComponent<CharacterHolder>();
        if (ch.Turn.Moved) return;

        //Need to check in here to see if the AI wants to move away from its current position.
        //For now it is going to end its go
        if (NeedToMove())
        {
            StartCoroutine(WalkAwayFromPlayer());
        }
        else
        {
            StartCoroutine(Wait());
        }
    }

    private bool NeedToMove()
    {
        var opposition = GetOppositionInAttackRange();
        var allies = GetAlliesInHealRange();

        var threatCount = opposition.Count;
        if (allies.Count > threatCount) threatCount = allies.Count;

        var oppositionThreatLevel = GetTeamThreatLevel(opposition, threatCount);
        var allyThreatLevel = GetTeamThreatLevel(allies, threatCount);

        return (oppositionThreatLevel > allyThreatLevel);
    }

    private float GetTeamThreatLevel(List<CharacterHolder> characterHolders, int threatCount)
    {
        var threatLevel = characterHolders.Sum(characterHolder => CalculateUtil.CalcPercentHP(characterHolder));
        threatLevel /= threatCount;
        return threatLevel;
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);

        print("WAITING");
        GameObject.Find("Characters").GetComponent<CharactersController>().CurrentCharacterHolder.Turn.Skip();
    }

    private bool IsCurrentCharacterHolder()
    {
        CharactersController cc = GameObject.Find("Characters").GetComponent<CharactersController>();
        CharacterHolder ch = cc.CurrentCharacterHolder;
        CharacterHolder thisCH = transform.GetComponent<CharacterHolder>();
        return (ch == thisCH);
    }
}
