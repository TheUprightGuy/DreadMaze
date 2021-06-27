using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crayon : MonoBehaviour
{
    public GameHandler gh;
    public GameObject XPrefab;
    public KeyCode drawKey = KeyCode.Space;

    public int apDrain = 1;

    public string StopRayCastTag = "";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(drawKey))
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, transform.forward, out hitInfo ,2.0f))
            {
                if (hitInfo.transform.CompareTag(StopRayCastTag))
                {
                    return;
                }
                Vector3 newPos = hitInfo.point;
                Vector3 newForward = -hitInfo.normal;

                newPos -= (newForward * 0.01f);
                GameObject newObj = Instantiate(XPrefab, newPos, Quaternion.identity);
                newObj.transform.forward = newForward;

                gh.CurrentAP--;
            }
        }    
    }
}
