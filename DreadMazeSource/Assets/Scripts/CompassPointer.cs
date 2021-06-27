using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassPointer : MonoBehaviour
{
    // Start is called before the first frame update

    public MazeGen Maze;
    public Transform playerTransform;

    RectTransform thisRT;
    Vector3 endPos;
    void Start()
    {
        endPos = Maze.GetPosOfIndex(Maze.exitCoord);
        thisRT = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 dir = (endPos - playerTransform.position).normalized;
        float rotANgle = Vector3.SignedAngle(playerTransform.forward, dir, Vector3.up);
        thisRT.rotation = Quaternion.Euler(0.0f, 0.0f, -rotANgle);


    }

    private void OnDrawGizmosSelected()
    {
    }
}
