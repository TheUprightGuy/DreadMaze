using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DialogueSet
{
    public float time = 0.0f;
    public KeyCode keyToEnter = KeyCode.None;
    public bool ForceSkip = false;
    public string Prompt = "";
    [TextArea]
    public string Description = "";

}
public class DialogueText : MonoBehaviour
{
    public Text Desc;
    public Text Prompt;
    public GameObject bg;
    public List<DialogueSet> DialogueQueue = new List<DialogueSet>();

    float timeBetweenChars = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        Desc.text = "";
        Prompt.text = "";

        bg.SetActive(false);
        Desc.gameObject.SetActive(false);
        Prompt.gameObject.SetActive(false);
        //StartDialogue("Testing 123", "Testing");
    }

    // Update is called once per frame
    void Update()
    {
        if (waitingOnPrompt && inProgress && stepTextDone) //Start on prompt
        {
            stepTextDone = false;
            StartCoroutine(StepThroughText(promptText, Prompt));
            waitingOnPrompt = false;
        }

        if (inProgress && DialogueQueue[0].ForceSkip)
        {
            StopCoroutine(coroutineInProgress);
            ClearCurrent();
        }

        if (!waitingOnPrompt && inProgress && stepTextDone) //Finish off
        {
            if (DialogueQueue[0].keyToEnter == KeyCode.None)
            {
                stepTextDone = false;
                StartCoroutine(SetActiveInSecs(DialogueQueue[0].time));
            }
            if(Input.GetKeyDown(DialogueQueue[0].keyToEnter))
            {
                stepTextDone = false;
                ClearCurrent();
            }
        }

        if (DialogueQueue.Count > 0 && !inProgress) //start on next
        {
            stepTextDone = true;
            inProgress = true;
            StartDialogue(DialogueQueue[0].Description, DialogueQueue[0].Prompt);
        }
    }

    [HideInInspector]
    public bool WaitingOnKey => !waitingOnPrompt && inProgress && stepTextDone && DialogueQueue[0].keyToEnter != KeyCode.None;

    bool stepTextDone = true;
    bool waitingOnPrompt = false;
    string promptText;
    bool inProgress = false;

    private IEnumerator coroutineInProgress;

    public void ClearText()
    {
        Desc.text = "";
        Prompt.text = "";
    }
    public void StartDialogue(string _Desc, string _Prompt)
    {
        bg.SetActive(true);
        Desc.gameObject.SetActive(true);
        Prompt.gameObject.SetActive(true);

        ClearText();
        
        stepTextDone = false;
        coroutineInProgress = StepThroughText(_Desc, Desc);
        StartCoroutine(coroutineInProgress);

        promptText = _Prompt;
        
        waitingOnPrompt = true;
    }
    IEnumerator StepThroughText(string newText, Text textToChange)
    {

        stepTextDone = false;
        string curText = "";

        int counter = 0;
        while (counter < newText.Length)
        {
            curText += newText[counter ];
            counter ++;

            textToChange.text = curText;
            //yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(timeBetweenChars);
        }

        textToChange.text = newText;
        stepTextDone = true;
    }

    IEnumerator SetActiveInSecs(float forSecs)
    {
        yield return new WaitForSeconds(forSecs);
        ClearCurrent();
    }

    void ClearCurrent()
    {
        waitingOnPrompt = false;
        stepTextDone = false;
        inProgress = false;

        bg.SetActive(false);
        Desc.gameObject.SetActive(false);
        Prompt.gameObject.SetActive(false);

        DialogueQueue.RemoveAt(0);

    }
}
