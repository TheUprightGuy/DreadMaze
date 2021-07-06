using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrazyText : MonoBehaviour
{
    public Sprite TextSprite;
    public float RandRotRange;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 randomEuler = new Vector3(0.0f, 0.0f, Random.Range(-RandRotRange, RandRotRange));

        Quaternion newRot = Quaternion.Euler(randomEuler);
        transform.localRotation = newRot;
        this.GetComponent<MeshRenderer>().material.SetTexture("_BaseMap", TextSprite.texture);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
