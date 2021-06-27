using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceRoll : MonoBehaviour
{

    public List<Sprite> DiceFaceList;
    public int LandedOnNum = 0;

    public float RollTime = 1.0f;
    public int RollCount = 10;
    Image thisImage;
    // Start is called before the first frame update
    void Start()
    {
        thisImage = GetComponent<Image>();
        RollDice();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RollDice()
    {
        Done = false;
        StartCoroutine(Roll(RollTime));
    }

    public bool Done = true;
    IEnumerator Roll(float time)
    {
        float elapsedTime = 0;

        float faceTimer = 0;
        float faceTimerMax = RollTime / RollCount;

        while (elapsedTime < time)
        {
            if (faceTimer > faceTimerMax)
            {
                faceTimer = 0;
                int index = Random.Range(0, DiceFaceList.Count);

                thisImage.sprite = DiceFaceList[index];
                LandedOnNum = index + 1;
            }

            elapsedTime += Time.deltaTime;
            faceTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Done = true;
    }
}
