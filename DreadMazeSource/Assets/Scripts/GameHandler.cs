using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public DialogueText PlayerDialogue;
    public PitchRange HeartBeat;
    public DiceRoll Dice1;
    public DiceRoll Dice2;
    public int CurrentAP = 0;

    [Header("Dice")]
    public uint DiceSize = 6;
    public uint DiceCount = 2;

    public uint MonsterDiceSize = 6;

    public const int MonsterStartDistance = 20;
    public int MonsterCurrentDistance;
    // Start is called before the first frame update
    void Start()
    {
        MonsterCurrentDistance = MonsterStartDistance;
        //PlayerMove();

       
    }

    bool diceRolled = false;
    bool firstMoveDone = false;
    bool secondMoveDone = false;

    bool dialogueDone = false;

    bool waitingOnMonsterMove = false;
    bool monsterFirstStep = true;
    // Update is called once per frame
    void Update()
    {

        if (CurrentAP < 1)
        {
            if (firstMoveDone && !waitingOnMonsterMove && monsterFirstStep)
            {
                waitingOnMonsterMove = true;
                StartCoroutine(MonsterWait(4.0f));

                DialogueSet monsterText = new DialogueSet();
                monsterText.Description = "It moves...";
                monsterText.time = 3.0f;

                PlayerDialogue.DialogueQueue.Add(monsterText);

                Dice1.gameObject.SetActive(false);
                Dice2.gameObject.SetActive(false);

                monsterFirstStep = false;
            }

            if (!diceRolled && !waitingOnMonsterMove)
            {
                Dice1.gameObject.SetActive(true);
                Dice2.gameObject.SetActive(true);

                diceRolled = true;
                Dice1.RollDice();
                Dice2.RollDice();
            }

            if (Dice1.Done && Dice2.Done && diceRolled)
            {

                if (firstMoveDone && !secondMoveDone)
                {
                    DialogueSet firstText = new DialogueSet();
                    firstText.Description = "What is this place? I can hear something moving...\nI need to escape";
                    firstText.time = 2.0f;
                    secondMoveDone = true;
                    PlayerDialogue.DialogueQueue.Add(firstText);
                }
                PlayerMove(); //Increase monster distance
                AdjustHeartBeat();
                diceRolled = false;
                firstMoveDone = true;
                monsterFirstStep = true;
            }
        }

        if (MonsterCurrentDistance < (MonsterStartDistance / 2) && !dialogueDone)
        {
            DialogueSet set = new DialogueSet();

            set.Description = "It draws near...";
            set.Prompt = "";
            set.time = 2.0f;
            PlayerDialogue.DialogueQueue.Add(set);
            dialogueDone = true;
        }
       
    }

    void PlayerMove() //Add AP here
    {
        //int dice1 = (int)(Random.Range(1, DiceSize));
        //int dice2 = (int)(Random.Range(1, DiceSize));
        CurrentAP = Dice1.LandedOnNum + Dice2.LandedOnNum;
    }


    IEnumerator MonsterWait(float forSecs)
    {
        yield return new WaitForSeconds(forSecs);
        waitingOnMonsterMove = false;
        MonsterMove();//Reduce Monster distance

    }
    void MonsterMove()
    {
        int moveNum = (int)Random.Range(1, MonsterDiceSize);
        MonsterCurrentDistance -= moveNum;
    }

    void AdjustHeartBeat()
    {
        int clamp = Mathf.Clamp(MonsterCurrentDistance, 0, MonsterStartDistance);
        float lerpVal = 1 - ((float)clamp / (float)MonsterStartDistance);
        HeartBeat.LerpVal = lerpVal;
    }

  
}
