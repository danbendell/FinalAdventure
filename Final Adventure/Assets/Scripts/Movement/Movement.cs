using System.Collections.Generic;
using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts.Movement
{
    public class Movement : MonoBehaviour {

        private int _pointerPositionX = 2;
        private int _pointerPositionY = 2;

        private List<List<Tile>> _floorArray = new List<List<Tile>>();
        private int _positionX = 0;
        private int _positionY = 0;

        public int MovementAllowance = 2;

        // Use this for initialization
        void Start () {

            int i = 0;
            foreach (Transform row in transform)
            {
                List<Tile> rowList = new List<Tile>();

                foreach (Transform block in row)
                {
                    Tile tile = new Tile();
                    tile.SetGameObject(block.gameObject);
                    rowList.Add(tile);
                }
                _floorArray.Add(rowList);
                i++;
            }

            _floorArray[_pointerPositionY][_pointerPositionX].SetMaterial(Tile.Highlight);
        }
	
        // Update is called once per frame
        void Update () {
            KeyboardInput();
        }

        private void KeyboardInput()
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                Tile tile = _floorArray[_pointerPositionY][_pointerPositionX];
                if (tile.GetState() == Tile.State.Walkable)
                {
                    _positionX = _pointerPositionX;
                    _positionY = _pointerPositionY;
                    HighlLightNewPositionTile();
                } 
                else
                {
                    print("Not reachable");
                }
            
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (_positionX > 0)
                {
                    _positionX--;
                    HighlLightNewPositionTile();
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (_positionY > 0)
                {
                    _positionY--;
                    HighlLightNewPositionTile();
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (_positionX < 5)
                {
                    _positionX++;
                    HighlLightNewPositionTile();
                }
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (_positionY < 3)
                {
                    _positionY++;
                    HighlLightNewPositionTile();
                }
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                if(_pointerPositionX > 0)
                {
                    ClearPointer();
                    _pointerPositionX--;
                    HighlLightNewPositionTile();
                }
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                if(_pointerPositionY > 0)
                {
                    ClearPointer();
                    _pointerPositionY--;
                    HighlLightNewPositionTile();
                }
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                if (_pointerPositionX < 5)
                {
                    ClearPointer();
                    _pointerPositionX++;
                    HighlLightNewPositionTile();
                }
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                if (_pointerPositionY < 3)
                {
                    ClearPointer();
                    _pointerPositionY++;
                    HighlLightNewPositionTile();
                }
            }
        }

        private void ClearPointer()
        {
            Tile tile = _floorArray[_pointerPositionY][_pointerPositionX];
            tile.SetMaterial(Tile.Normal);
        }

        private void MovePointer()
        {
            Tile tile = _floorArray[_pointerPositionY][_pointerPositionX];
            tile.SetMaterial(Tile.Highlight);
        }

        private void HighlLightNewPositionTile()
        {
            Tile tile = _floorArray[_positionY][_positionX];
            tile.SetMaterial(Tile.Normal);

            HighlightMovementTiles();
            MovePointer();
        }

        private void HighlightMovementTile(int positionX, int positionY)
        {
            Tile tile = _floorArray[positionY][positionX];
            tile.SetMaterial(Tile.Walkpath);
            tile.SetState(Tile.State.Walkable);
        }

        private void RemoveTileHighlight(Tile tile)
        {
            tile.SetMaterial(Tile.Normal);
            tile.SetState(Tile.State.Unwalkable);
        }

        private void HighlightMovementTiles()
        {
            ClearFloor();

            int startPositionY = _positionY - MovementAllowance;
            int startPositionX = _positionX;

            if (startPositionY < 0)
            {
                startPositionY = 0;
                startPositionX = _positionX - MovementAllowance;
                if (startPositionX < 0) startPositionX = 0;
            }

            for(int y = startPositionY; y < _floorArray.Count; y++)
            {

                var currentY = y;
                for(int x = 0; x < _floorArray[0].Count; x++)
                { 
                    var currentX = x;
                    int differenceInX = CalculatePositiveDifference(_positionX, currentX);
                    int differnceInY = CalculatePositiveDifference(_positionY, currentY);
                    int totalDifference = differenceInX + differnceInY;

                    if (totalDifference == 0) continue;
                    if (totalDifference <= MovementAllowance)
                    {
                        HighlightMovementTile(currentX, currentY);
                    }

                }
            }
        }

        private void ClearFloor()
        {
            for(int y = 0; y < _floorArray.Count; y++)
            {
                for(int x = 0; x < _floorArray[0].Count; x++)
                {
                    RemoveTileHighlight(_floorArray[y][x]);
                }
            }
        }

        private int CalculatePositiveDifference(int valueOne, int valueTwo)
        {
            if(valueOne > valueTwo)
            {
                return valueOne - valueTwo;
            }
            else
            {
                return valueTwo - valueOne;
            }
       
        }
    }
}
