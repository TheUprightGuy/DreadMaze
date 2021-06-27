using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public enum PlayerDirs
    {
        N = 0,
        E,
        S,
        W
    }
    PlayerDirs movingTowardDir = PlayerDirs.N;

    public GameHandler gh;
    public MazeGen MazeToMoveOn;
    MazeCell[,] MazeGrid;


    [Header("Speeds")]
    public float rotTime = 1.0f;
    public float moveTime = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        if (MazeToMoveOn != null)
        {
            MazeGrid = MazeToMoveOn.MazeGrid;
        }

        Vector2Int enterPos = MazeToMoveOn.enterCoord;
        transform.position = MazeToMoveOn.GetPosOfIndex(enterPos);

        MazeCell enterCell = MazeGrid[enterPos.x, enterPos.y];
        Directions lookDir = Directions.N;
        Vector3 newForward = Vector3.zero;
        for (int i = 0; i < enterCell.walls.Length; i++)
        {
            if (enterCell.walls[i]) //This wall is open
            {
                lookDir = (Directions)i;
                break;
            }
        }

        switch (lookDir)
        {
            case Directions.N:
                movingTowardDir = PlayerDirs.N;
                newForward = Vector3.forward;
                break;
            case Directions.E:
                movingTowardDir = PlayerDirs.E;
                newForward = Vector3.right;
                break;
            case Directions.S:
                movingTowardDir = PlayerDirs.S;
                newForward = Vector3.back;
                break;
            case Directions.W:
                movingTowardDir = PlayerDirs.W;
                newForward = Vector3.left;
                break;
            default:
                break;
        }

        transform.forward = newForward;
        
        CurrentCell = enterPos;

        //gh = GetComponent<GameHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        RotatePlayer();
        MovePlayer();
    }

    [HideInInspector]
    public Vector2Int CurrentCell;
    void MovePlayer()
    {
        if (!rotDone || !moveDone || gh.CurrentAP < 1) //still moving so ignore the rest
        {                                               //out of AP
            return;
        }

        int forwardAxis = 0;
        if (Input.GetKeyDown(KeyCode.W)) //Move Forward
        {
            forwardAxis++;
        }
        else if (Input.GetKeyDown(KeyCode.S)) //Move Back
        {
            forwardAxis--;
        }
        else
        {
            return;
        }


        if (Physics.Raycast(transform.position, transform.forward * forwardAxis, 2.5f))
        {
            return;
        }

        switch (movingTowardDir)
        {
            case PlayerDirs.N:
                CurrentCell.y += forwardAxis;

                break;
            case PlayerDirs.S:
                CurrentCell.y -= forwardAxis;
                break;
            case PlayerDirs.E:
                CurrentCell.x += forwardAxis;
                break;
            case PlayerDirs.W:
                CurrentCell.x -= forwardAxis;
                break;
            default:
                break;
        }

        gh.CurrentAP--;
        gh.MonsterCurrentDistance++;
        
        Vector3 newPos = MazeToMoveOn.GetPosOfIndex(CurrentCell);

        StartCoroutine(LerpPos(transform, transform.position, newPos, moveTime));
    }
    void RotatePlayer()
    {
        if (!rotDone || !moveDone) //still moving so ignore the rest
        {
            return;
        }

        int playerDir = (int)movingTowardDir;
        if (Input.GetKeyDown(KeyCode.A))
        {
            playerDir--;
            if (playerDir < 0) { playerDir = 3; }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            playerDir++;
        }
        else
        {
            return;
        }

        playerDir = playerDir % 4;


        

        movingTowardDir = (PlayerDirs)playerDir;

        Vector3 newForward = transform.forward;
        switch (movingTowardDir)
        {
            case PlayerDirs.N:
                newForward = Vector3.forward;
                break;
            case PlayerDirs.E:
                newForward = Vector3.right;
                break;
            case PlayerDirs.S:
                newForward = Vector3.back;
                break;
            case PlayerDirs.W:
                newForward = Vector3.left;
                break;
            default:
                break;
        }


        StartCoroutine(LerpForward(this.transform, transform.forward, newForward, rotTime));
    }

    bool rotDone = true;
    IEnumerator LerpForward(Transform _transform, Vector3 _startForward, Vector3 _newForward, float time)
    {
        float elapsedTime = 0;

        rotDone = false;
        while (elapsedTime < time)
        {
            _transform.forward = Vector3.Lerp(_startForward, _newForward, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        rotDone = true;
        _transform.forward = _newForward;
    }

    bool moveDone = true;
    IEnumerator LerpPos(Transform _transform, Vector3 _startPos, Vector3 _newPos, float time)
    {
        float elapsedTime = 0;

        moveDone = false;
        while (elapsedTime < time)
        {
            _transform.position = Vector3.Lerp(_startPos, _newPos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        moveDone = true;
        _transform.position = _newPos;
    }
}
