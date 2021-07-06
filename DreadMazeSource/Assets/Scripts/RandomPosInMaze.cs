using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPosInMaze : MonoBehaviour
{
    public MazeGen mg;

    public List<Vector2Int> deadEnds = new List<Vector2Int>();

    public Vector3 placeOffset = Vector3.zero;
    public int WallCountCondition = 3;
    public bool stickToWalls = false;
    // Start is called before the first frame update
    void Start()
    {
        FindAllDeadEnds();
        RandomiseChildrenPos();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RandomiseChildrenPos()
    {
        foreach (Transform item in transform)
        {
            int randIndex = Random.Range(0, deadEnds.Count);
            Vector3 cellPos = mg.GetPosOfIndex(deadEnds[randIndex]);
            if (stickToWalls)
            {
                Vector3[] dirs = { Vector3.forward, Vector3.left, Vector3.back, Vector3.right };
                foreach (Vector3 dir in dirs)
                {
                    RaycastHit hitInfo;
                    if (Physics.Raycast(cellPos, dir, out hitInfo, 2.0f))
                    {
                        Vector3 newPos = hitInfo.point;
                        Vector3 newForward = -hitInfo.normal;

                        newPos -= (newForward * 0.01f);

                        item.transform.forward = newForward;
                        item.transform.position = (newPos + placeOffset);
                        break;
                    }
                }
            }
            else
            {
                item.position = cellPos + placeOffset;
            }


            deadEnds.Remove(deadEnds[randIndex]); //Remove it so it doesn't come up again
        }


    }
    void FindAllDeadEnds()
    {
        for (int i = 0; i < mg.MazeRes; i++)
        {
            for (int j = 0; j < mg.MazeRes; j++)
            {
                if (mg.enterCoord == new Vector2Int(i,j)) //there is already text at the start
                {
                    continue;
                }
                int counter = 0;
                foreach (bool item in mg.MazeGrid[i,j].walls)
                {
                    if (item)
                    {
                        counter++;
                    }
                }

                if (counter == WallCountCondition)
                {
                    deadEnds.Add(new Vector2Int(i, j));
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        //foreach (Vector2Int item in deadEnds)
        //{
        //    Vector3 pos = mg.GetPosOfIndex(item);

        //    Gizmos.DrawSphere(pos, 1.0f);
        //}
    }
}
