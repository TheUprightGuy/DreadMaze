using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipCounter : MonoBehaviour
{
    public GameObject PipPrefab;
    public GameHandler GH;

    public int MaxAP;
    int cacheAP = 0;
    // Start is called before the first frame update
    void Start()
    {
        MaxAP = (int)GH.DiceCount * (int)GH.DiceSize;
        InstantatiatePips();
    }

    // Update is called once per frame
    void Update()
    {
        if (cacheAP != GH.CurrentAP)
        {
            cacheAP = GH.CurrentAP;
            UpdateCurrentAP();
        }
    }

    List<GameObject> Pips = new List<GameObject>();
    void InstantatiatePips()
    {
        for (int i = 0; i < MaxAP; i++)
        {
            GameObject newPip = Instantiate(PipPrefab, transform);
            newPip.SetActive(false);
            Pips.Add(newPip);
        }
    }

    void UpdateCurrentAP()
    {
        for (int i = 0; i < MaxAP; i++)
        {
            Pips[i].SetActive(GH.CurrentAP > i);
        }
    }
}
