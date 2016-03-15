using System.Collections.Generic;
using System.Runtime.InteropServices;
using Assets.Scripts.Model;
using UnityEditorInternal;
using UnityEngine;

namespace Assets.Scripts.Movement
{
    public class FloorHighlight : MonoBehaviour {

        public Vector2 PointerPosition = new Vector2(0,0);

        public Tile[,] FloorArray;
        private Vector2 _position = new Vector2(0,0);

        public int Width { get; private set; }
        
        public int Height { get; private set; }

        public bool[,] FloorMap { get; private set; }

        private string _material = Tile.Normal;

        private int _tileAllowance = 0;

        public void SetMovement(Vector2 position, int speed)
        {
            _position = position;
            PointerPosition = position;

            _material = Tile.Walkpath;

            _tileAllowance = speed;
            HighlLightNewPositionTile();
        }

        public void SetAttackRange(Vector2 position, int range)
        {
            _position = position;
            PointerPosition = position;

            _material = Tile.Attack;

            _tileAllowance = range;
            HighlLightNewPositionTile();
        }

        public void SetPosition(Vector2 position)
        {
            _position = position;
            PointerPosition = position;
            
            HighlLightNewPositionTile();
        }

        public void SetPointerPosition(Vector2 position)
        {
            ClearPointer();
            PointerPosition = position;
            HighlLightNewPositionTile();
        }

        // Use this for initialization
        void Start ()
        {
            Width = transform.GetChild(0).childCount;
            Height = transform.childCount;
            FloorArray = new Tile[Width, Height];

            for (int y = 0; y < transform.childCount; y++)
            {
                for (int x = 0; x < transform.GetChild(y).childCount; x++)
                {
                    Tile tile = new Tile();
                    tile.SetGameObject(transform.GetChild(y).GetChild(x).gameObject);
                    tile.SetState(Tile.State.Unwalkable);
                    FloorArray[x, y] = tile;
                }
            }

            FloorMap = new bool[Width, Height];
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    FloorMap[x, y] = true;


            FloorArray[(int) PointerPosition.x, (int) PointerPosition.y].SetMaterial(Tile.Highlight);
        }
	
        // Update is called once per frame
        void Update () {
            KeyboardInput();
        }

        private void KeyboardInput()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                if(PointerPosition.x > 0)
                {
                    ClearPointer();
                    PointerPosition.x--;
                    HighlLightNewPositionTile();
                }
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                if(PointerPosition.y > 0)
                {
                    ClearPointer();
                    PointerPosition.y--;
                    HighlLightNewPositionTile();
                }
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                if (PointerPosition.x < 5)
                {
                    ClearPointer();
                    PointerPosition.x++;
                    HighlLightNewPositionTile();
                }
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                if (PointerPosition.y < 3)
                {
                    ClearPointer();
                    PointerPosition.y++;
                    HighlLightNewPositionTile();
                }
            }
        }

        private void ClearPointer()
        {
            Tile tile = FloorArray[(int) PointerPosition.x, (int) PointerPosition.y];
            tile.SetMaterial(Tile.Normal);
        }

        private void MovePointer()
        {
            Tile tile = FloorArray[(int) PointerPosition.x, (int) PointerPosition.y];
            tile.SetMaterial(Tile.Highlight);
        }

        public void HighlLightNewPositionTile()
        {
            Tile tile = FloorArray[(int) _position.x, (int) _position.y];
            tile.SetMaterial(Tile.Normal);

            HighllightTilesWith();
            MovePointer();
        }

        private void HighlightMovementTile(int positionX, int positionY)
        {
            Tile tile = FloorArray[positionX, positionY];
            tile.SetMaterial(_material);
        }

        private void RemoveTileHighlight(Tile tile)
        {
            tile.SetMaterial(Tile.Normal);
            tile.SetState(Tile.State.Unwalkable);
        }

        private void HighllightTilesWith()
        {
            ClearFloor();

            Vector2 startPosition;
            startPosition.y = _position.y - _tileAllowance;
            startPosition.x = _position.x;

            if (startPosition.y < 0)
            {
                startPosition.y = 0;
                startPosition.x = _position.x - _tileAllowance;
                if (startPosition.x < 0) startPosition.x = 0;
            }

            for(int y = (int) startPosition.y; y < Height; y++)
            {
                var currentY = y;
                for(int x = 0; x < Width; x++)
                { 
                    var currentX = x;
                    float differenceInX = CalculatePositiveDifference(_position.x, currentX);
                    float differnceInY = CalculatePositiveDifference(_position.y, currentY);
                    float totalDifference = differenceInX + differnceInY;

                    if (totalDifference == 0 && _material == Tile.Walkpath) continue;
                    if (totalDifference <= _tileAllowance)
                    {
                        HighlightMovementTile(currentX, currentY);
                    }

                }
            }
        }

        public void SetNewPosition()
        {
            _position.x = PointerPosition.x;
            _position.y = PointerPosition.y;
        }

        private void ClearFloor()
        {
            for(int y = 0; y < Height; y++)
            {
                for(int x = 0; x < Width; x++)
                {
                    RemoveTileHighlight(FloorArray[x, y]);
                }
            }
        }

        public void ResetFloorHighlight()
        {
            _material = Tile.Normal;
            ClearFloor();
            _tileAllowance = 0;
            ShowPointer();
        }

        private void ShowPointer()
        {
            FloorArray[(int)PointerPosition.x, (int)PointerPosition.y].SetMaterial(Tile.Highlight);
        }

        private float CalculatePositiveDifference(float valueOne, float valueTwo)
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

        public List<Vector3> AddTileHeights(List<Vector2> flatPath)
        {
            List<Vector3> modifiedPath = new List<Vector3>();
            foreach (Vector2 position in flatPath)
            {
                Tile tile = FloorArray[(int) position.x, (int) position.y];
                modifiedPath.Add(new Vector3(tile.Position.x, tile.GetHeight(), tile.Position.y));
            }

            return modifiedPath;
        }
    }
}
