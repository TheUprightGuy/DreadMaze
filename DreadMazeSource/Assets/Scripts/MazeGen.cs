using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum Directions
{
    N = 0,
    S,
    E,
    W,
    NONE
}

public class MazeCell
{
    public bool Visited = false;
    public bool[] walls = { true, true, true, true };
}
public class MazeGen : MonoBehaviour
{
    public GameObject WallPrefab;
    public uint MazeRes = 10;

    public float seperation = 1;
    [HideInInspector]
    public MazeCell[,] MazeGrid;

 
    // Start is called before the first frame update
    void Awake()
    {
        MazeGrid = new MazeCell[MazeRes,MazeRes];

        for (int i = 0; i < MazeRes; i++)
        {
            for (int j = 0; j < MazeRes; j++)
            {
                MazeGrid[i, j] = new MazeCell();
            }
        }
        GenMaze(new Vector2Int(0,0));
        SetEnterExitOnBorders();
        InstantiateWalls();

    }

    [HideInInspector]
    public Vector2Int enterCoord = Vector2Int.zero;
    [HideInInspector]
    public Vector2Int exitCoord = Vector2Int.zero;

    [HideInInspector]
    public Directions enterDir;
    void SetEnterExitOnBorders(Directions _forceDir = Directions.NONE)
    {
        Directions sideToOpen = _forceDir;
        if (_forceDir == Directions.NONE)
        {
            sideToOpen = (Directions)Random.Range(0, 3);
        }

        int iRandX = (int)Random.Range(0, MazeRes - 1);
        bool[] newwalls = { false, false, false, false };
        Directions setSide = Directions.NONE;

        Vector2Int thisCoord = Vector2Int.zero;
        switch (sideToOpen)
        {
            case Directions.N:
                thisCoord = new Vector2Int(iRandX, 0);
                setSide = Directions.S;
                break;
            case Directions.S:
                thisCoord = new Vector2Int(iRandX, (int)MazeRes - 1);
                setSide = Directions.N;
                break;
            case Directions.E:
                thisCoord = new Vector2Int((int)MazeRes - 1, iRandX);
                setSide = Directions.W;
                break;
            case Directions.W:
                
                thisCoord = new Vector2Int(0, iRandX);
                setSide = Directions.E;
                break;
            default:
                break;
        }

        if (_forceDir == Directions.NONE)
        {
            enterDir = sideToOpen;
            enterCoord = thisCoord;
            SetEnterExitOnBorders(setSide);
        }
        else
        {
            MazeGrid[thisCoord.x, thisCoord.y].walls = newwalls;
            exitCoord = thisCoord;
        }
        //MazeGrid[iRandX, 0].walls = newwalls;
    }
    void GenMaze(Vector2Int _startPoint)
    {
        List<Directions> dirs = new List<Directions>{ Directions.N, Directions.S, Directions.E, Directions.W };
        var shuffled = dirs.OrderBy(x => System.Guid.NewGuid()).ToList();

        foreach (Directions item in shuffled)
        {
            Vector2Int newPoint = _startPoint;

            switch (item)
            {
                case Directions.N:
                    newPoint.y++;
                    break;
                case Directions.S:
                    newPoint.y--;
                    break;
                case Directions.E:
                    newPoint.x++;
                    break;
                case Directions.W:
                    newPoint.x--;
                    break;
                default:
                    break;
            }

            if (CellCheck(newPoint))
            {
                MazeGrid[newPoint.x, newPoint.y].Visited = true;
                switch (item)
                {
                    case Directions.N:
                        MazeGrid[_startPoint.x, _startPoint.y].walls[(int)Directions.N] = false;
                        MazeGrid[newPoint.x, newPoint.y].walls[(int)Directions.S] = false;
                        break;
                    case Directions.S:

                        MazeGrid[_startPoint.x, _startPoint.y].walls[(int)Directions.S] = false;
                        MazeGrid[newPoint.x, newPoint.y].walls[(int)Directions.N] = false;
                        break;
                    case Directions.E:

                        MazeGrid[_startPoint.x, _startPoint.y].walls[(int)Directions.E] = false;
                        MazeGrid[newPoint.x, newPoint.y].walls[(int)Directions.W] = false;
                        break;
                    case Directions.W:

                        MazeGrid[_startPoint.x, _startPoint.y].walls[(int)Directions.W] = false;
                        MazeGrid[newPoint.x, newPoint.y].walls[(int)Directions.E] = false;
                        break;
                    default:
                        break;
                }
                GenMaze(newPoint);
            }
        }
    }

    bool CellCheck(Vector2Int index)
    {
        return (WithinBounds(index) &&
            !MazeGrid[index.x, index.y].Visited); //Not visisted
    }
    bool WithinBounds(Vector2Int index)
    {
        return (index.x < MazeGrid.GetLength(0) && index.y < MazeGrid.GetLength(1) 
            && index.x >= 0 && index.y >= 0);
    }

    void InstantiateWalls()
    {
        Vector3 scale = WallPrefab.transform.localScale;
        for (int i = 0; i < MazeRes; i++)
        {
            for (int j = 0; j < MazeRes; j++)
            {

                Vector3 pos = GetPosOfIndex(new Vector2Int(i, j));//new Vector3((i * seperation) * scale.x, scale.y / 2, (j * seperation) - scale.z);
                GameObject newwall= Instantiate(WallPrefab, pos, Quaternion.identity, this.transform);

                for (int k = 0; k < 4; k++)
                {
                    newwall.transform.GetChild(k).gameObject.SetActive(MazeGrid[i, j].walls[k]);
                }
                
            }
        }
    }
    
    public Vector3 GetPosOfIndex(Vector2Int index)
    {
        Vector3 scale = WallPrefab.transform.localScale;
        return new Vector3((index.x * seperation) * scale.x, scale.y / 2, (index.y * seperation) - scale.z);
    }
}
