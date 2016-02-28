using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Model;
using Assets.Scripts.Movement;
using Assets.Scripts.Util;

public class Movement : MonoBehaviour
{
    private Character _character;
    
    private List<Vector3> _optimalMovementPath;
    private Vector3 _nextTile;
    private int _movementCount = 0;

    // Use this for initialization
    void Start () {
        _character = transform.GetComponent<CharacterHolder>().Character;
    }
	
	// Update is called once per frame
	void Update ()
	{
	    KeyboardInput();
	}

    public void SetCharacter(Character character)
    {
        _character = character;
    }

    private void KeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (GameObject.Find("Characters").GetComponent<CharactersController>().CurrentCharacterHolder.Character !=
                _character) return;

            var floor = GameObject.Find("Floor").GetComponent<FloorHighlight>().FloorArray;
            var pointer = GameObject.Find("Floor").GetComponent<FloorHighlight>().PointerPosition;

            Tile tile = floor[(int)pointer.x, (int)pointer.y];
            if (tile.GetState() == Tile.State.Walkable)
            {
                SetPosition(new Vector3(pointer.x, tile.GetHeight(), pointer.y));
                GameObject.Find("Floor").GetComponent<FloorHighlight>().SetNewPosition();
                GameObject.Find("Floor").GetComponent<FloorHighlight>().HighlLightNewPositionTile();
            }
        }

        if (transform.position != _character.Position)
        {
            MoveCharacter();
        }
    }

    private void MoveCharacter()
    {
        RotateCharacter();
        if (transform.position == _nextTile)
        {
            _movementCount++;
            _nextTile = _optimalMovementPath[_movementCount];
        }

        float step = 2 * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _nextTile, step);
    }

    private void RotateCharacter()
    {
        if (transform.position.x < _nextTile.x)
        {
            transform.forward = new Vector3(0, 0, 1f);
        } 
        else if (transform.position.x > _nextTile.x)
        {
            transform.forward = new Vector3(0, 0, -1f);
        }
        else if (transform.position.z < _nextTile.z)
        {
            transform.forward = new Vector3(-1f, 0, 0);
        }
        else if (transform.position.z > _nextTile.z)
        {
            transform.forward = new Vector3(1f, 0, 0);
        }
    }

    private void SetPosition(Vector3 position)
    {
        _character.Position = position;

        CreateOptimalPath();
    }

    private void CreateOptimalPath()
    {
        bool[,] map = GameObject.Find("Floor").GetComponent<FloorHighlight>().FloorMap;

        Vector2 startLocation = new Vector2(transform.position.x, transform.position.z);
        Vector2 endLocation = new Vector2(_character.Position.x, _character.Position.z);

        SearchParameters searchParameters = new SearchParameters(startLocation, endLocation, map);

        AStar pathFinder = new AStar(searchParameters);
        List<Vector2> flatPath = pathFinder.FindPath();

        _optimalMovementPath = GameObject.Find("Floor").GetComponent<FloorHighlight>().AddTileHeights(flatPath);
        _movementCount = 0;
        _nextTile = _optimalMovementPath[_movementCount];
    }
}
