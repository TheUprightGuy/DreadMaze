using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPosInMaze : MonoBehaviour
{
    public MazeGen mg;

    public List<Vector2Int> deadEnds = new List<Vector2Int>();
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

            item.position = mg.GetPosOfIndex(deadEnds[randIndex]);
            deadEnds.Remove(deadEnds[randIndex]); //Remove it so it doesn't come up again
        }


    }
    void FindAllDeadEnds()
    {
        for (int i = 0; i < mg.MazeRes; i++)
        {
            for (int j = 0; j < mg.MazeRes; j++)
            {
                int counter = 0;
                foreach (bool item in mg.MazeGrid[i,j].walls)
                {
                    if (item)
                    {
                        counter++;
                    }
                }

                if (counter == 3)
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
