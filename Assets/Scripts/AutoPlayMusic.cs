using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPlayMusic : MonoBehaviour
{
    AudioSource audioData = null;

    // This script needs to attach an Audio Source to play
    void Start()
    {
        audioData = GetComponent<AudioSource>();
        if (audioData)
        {
            audioData.Play(0);
        }
        else
        {
            Debug.LogWarning("AutoPlayMusic withouth AudioSource component");
        }
    }
}
