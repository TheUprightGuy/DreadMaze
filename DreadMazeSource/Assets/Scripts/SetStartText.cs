using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetStartText : MonoBehaviour
{
    public GameObject startText; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    bool placed = false;
    // Update is called once per frame
    void Update()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 2.0f) && startText != null && !placed)
        {
            Vector3 newPos = hitInfo.point;
            Vector3 newForward = -hitInfo.normal;

            newPos -= (newForward * 0.01f);
            GameObject newObj = Instantiate(startText, newPos, Quaternion.identity);
            newObj.transform.forward = newForward;

            placed = true;
        }
    }
}
