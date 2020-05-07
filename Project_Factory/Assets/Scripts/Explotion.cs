using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explotion : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip sound;
    public float time;

    float timeToGone;

    void Start()
    {
        timeToGone = time + Time.time;

        audioSource = GetComponent<AudioSource>();

        audioSource.clip = sound;
        audioSource.Play();
    }

    private void LateUpdate()
    {
        if (timeToGone < Time.time)
        {
            Destroy(gameObject);
        }
    }
}
