using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMarkHandler : MonoBehaviour
{
    public DialogueText dialogue;

    public DialogueSet ImTooTired;
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
                    if (!dialogue.DialogueQueue.Contains(currentLandmark.DialogueInfo) && !currentLandmark.collected)
                    {
                        currentLandmark.DialogueInfo.ForceSkip = false;
                        dialogue.DialogueQueue.Add(currentLandmark.DialogueInfo);
                    }
                }
            }
            else
            {
                if (currentLandmark != null)
                {
                    if (dialogue.DialogueQueue.Contains(currentLandmark.DialogueInfo) )
                    {
                        currentLandmark.DialogueInfo.ForceSkip = true;
                        
                    }
                }
                currentLandmark = null;
            }
        }
        else
        {
            if (currentLandmark != null)
            {
                if (dialogue.DialogueQueue.Contains(currentLandmark.DialogueInfo))
                {
                    currentLandmark.DialogueInfo.ForceSkip = true;

                }
            }

            currentLandmark = null;
        }

        if (currentLandmark != null)
        {
            if (Input.GetKeyDown(currentLandmark.DialogueInfo.keyToEnter) && dialogue.WaitingOnKey)
            {
                if(currentLandmark.CallOnCollect())
                {
                    dialogue.DialogueQueue.Add(currentLandmark.AfterCollection);
                }
                else if (currentLandmark.gh.CurrentAP < currentLandmark.APDrain)
                {
                    dialogue.DialogueQueue.Add(ImTooTired);
                }
            }
        }
        //else if(dialogue.currentUsingObject == null)
        //{
        //    dialogue.gameObject.SetActive(false);
        //    dialogue.currentUsingObject = null;
        //}
    }
}
