using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchRange : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float LerpVal = 0.0f;
    // Start is called before the first frame update
    const float startRange = 0.9f;
    const float endRange = 1.5f;

    AudioSource thisAudio;
    void Start()
    {
        thisAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        thisAudio.pitch = Mathf.Lerp(startRange, endRange, LerpVal);
    }
}
