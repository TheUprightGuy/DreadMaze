using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class LandMark : MonoBehaviour
{
    // Start is called before the first frame update

    public GameHandler gh;
    public UnityEvent OnCollectEvent;

    public uint APDrain = 0;
    public DialogueSet DialogueInfo;

    public GameObject LockIfActive;
    public bool collected = false;
    void Start()
    {
        OnCollectEvent.AddListener(DrainAP);
    }

    // Update is called once per frame
    void Update()
    {
        if (LockIfActive != null)
        {
            collected = LockIfActive.activeSelf;
        }
    }

    void DrainAP()
    {
        gh.CurrentAP -= (int)APDrain;
    }

    public void CallOnCollect()
    {
        if (!collected)
        {
            collected = true;
            OnCollectEvent.Invoke();
        }
    }
}
