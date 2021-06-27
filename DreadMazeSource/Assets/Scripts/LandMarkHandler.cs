using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMarkHandler : MonoBehaviour
{
    public DialogueText dialogue;

     
    // Start is called before the first frame update
    void Start()
    {

    }

    LandMark currentLandmark = null;
    // Update is called once per frame
    void Update()
    {
        RaycastHit hitinfo;

        if (Physics.Raycast(transform.position, transform.forward, out hitinfo, 2.5f) )//Hit something
        {

            if (hitinfo.transform.GetComponent<LandMark>())//this is a landmark
            {
                if (currentLandmark == null)
                {
                    currentLandmark = hitinfo.transform.GetComponent<LandMark>();
                    dialogue.DialogueQueue.Add(currentLandmark.DialogueInfo);
                }
            }
            else
            {
                currentLandmark = null;
            }
        }
        else
        {
            currentLandmark = null;
        }

        if (currentLandmark != null)
        {
            if (Input.GetKeyDown(currentLandmark.DialogueInfo.keyToEnter))
            {
                currentLandmark.CallOnCollect();
            }
        }
        //else if(dialogue.currentUsingObject == null)
        //{
        //    dialogue.gameObject.SetActive(false);
        //    dialogue.currentUsingObject = null;
        //}
    }
}
