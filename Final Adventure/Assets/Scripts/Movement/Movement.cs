using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Movement : MonoBehaviour {

    public Material m_PlainGrass;
    public Material m_MovementGrass;

    private int m_PointerPositionX = 2;
    private int m_PointerPositionY = 2;

    private List<List<GameObject>> m_FloorArray = new List<List<GameObject>>();
    private int m_PositionX = 0;
    private int m_PositionY = 0;

    public int m_MovementAllowance = 2;

	// Use this for initialization
	void Start () {

        int i = 0;
        foreach (Transform row in transform)
        {
            List<GameObject> rowList = new List<GameObject>();

            foreach (Transform block in row)
            {
                rowList.Add(block.gameObject);
            }
            m_FloorArray.Add(rowList);
            i++;
        }

        GameObject Tile = m_FloorArray[m_PointerPositionY][m_PointerPositionX];
        Tile.GetComponent<Renderer>().material = Tile.GetComponent<Tile>().Highlight;
    }
	
	// Update is called once per frame
	void Update () {
        KeyboardInput();
    }

    private void KeyboardInput()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            m_PositionX = m_PointerPositionX;
            m_PositionY = m_PointerPositionY;
            HighlLightNewPositionTile();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            print(m_PositionX + " " + m_PositionY);
            if (m_PositionX > 0)
            {
                m_PositionX--;
                HighlLightNewPositionTile();
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            print(m_PositionX + " " + m_PositionY);
            if (m_PositionY > 0)
            {
                m_PositionY--;
                HighlLightNewPositionTile();
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            print(m_PositionX + " " + m_PositionY);
            if (m_PositionX < 5)
            {
                m_PositionX++;
                HighlLightNewPositionTile();
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            print(m_PositionX + " " + m_PositionY);
            if (m_PositionY < 3)
            {
                m_PositionY++;
                HighlLightNewPositionTile();
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if(m_PointerPositionX > 0)
            {
                ClearPointer();
                m_PointerPositionX--;
                HighlLightNewPositionTile();
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if(m_PointerPositionY > 0)
            {
                ClearPointer();
                m_PointerPositionY--;
                HighlLightNewPositionTile();
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if (m_PointerPositionX < 5)
            {
                ClearPointer();
                m_PointerPositionX++;
                HighlLightNewPositionTile();
            }
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            if (m_PointerPositionY < 3)
            {
                ClearPointer();
                m_PointerPositionY++;
                HighlLightNewPositionTile();
            }
        }
    }

    private void ClearPointer()
    {
        GameObject Tile = m_FloorArray[m_PointerPositionY][m_PointerPositionX];
        Tile.GetComponent<Renderer>().material = Tile.GetComponent<Tile>().Normal;
    }

    private void MovePointer()
    {
        GameObject Tile = m_FloorArray[m_PointerPositionY][m_PointerPositionX];
        Tile.GetComponent<Renderer>().material = Tile.GetComponent<Tile>().Highlight;
    }

    private void HighlLightNewPositionTile()
    {
        GameObject Tile = m_FloorArray[m_PositionY][m_PositionX];
        Tile.GetComponent<Renderer>().material = Tile.GetComponent<Tile>().Normal;

        HighlightMovementTiles();

        MovePointer();
    }

    private void HighlightMovementTile(int positionX, int positionY)
    {
        GameObject Tile = m_FloorArray[positionY][positionX];
        Tile.GetComponent<Renderer>().material = Tile.GetComponent<Tile>().Walkpath;
    }

    private void RemoveTileHighlight(GameObject Tile)
    {
        Tile.GetComponent<Renderer>().material = Tile.GetComponent<Tile>().Normal;
    }

    private void HighlightMovementTiles()
    {
        ClearFloor();

        int startPositionY = m_PositionY - m_MovementAllowance;
        int startPositionX = m_PositionX;
        if (startPositionY < 0)
        {
            startPositionY = 0;
            startPositionX = m_PositionX - m_MovementAllowance;
            if (startPositionX < 0) startPositionX = 0;
        }

        int currentY, currentX;

        for(int y = startPositionY; y < m_FloorArray.Count; y++)
        {
            currentY = y;
            for(int x = 0; x < m_FloorArray[0].Count; x++)
            {
                currentX = x;
                int differenceInX = CalculatePositiveDifference(m_PositionX, currentX);
                int differnceInY = CalculatePositiveDifference(m_PositionY, currentY);
                int totalDifference = differenceInX + differnceInY;

                if (totalDifference == 0) continue;
                if (totalDifference <= m_MovementAllowance)
                {
                    HighlightMovementTile(currentX, currentY);
                }

            }
        }
    }

    private void ClearFloor()
    {
        for(int y = 0; y < m_FloorArray.Count; y++)
        {
            for(int x = 0; x < m_FloorArray[0].Count; x++)
            {
                RemoveTileHighlight(m_FloorArray[y][x]);
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
