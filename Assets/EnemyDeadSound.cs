using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadSound : MonoBehaviour
{
    public AudioSource audioSource;
    public float minPitch;
    public float maxPitch;

    void Start()
    {
        audioSource.pitch = Random.Range(minPitch,maxPitch);
        audioSource.Play();
        Destroy(gameObject,2f);
    }
}
