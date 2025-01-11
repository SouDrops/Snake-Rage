using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip eatSound;
    public AudioClip gameoverSound;

    public void PlayEatSound()
    {
        audioSource.PlayOneShot(eatSound);
    }

    public void PlayGameOverSound()
    {
        audioSource.PlayOneShot(gameoverSound);
    }
}
