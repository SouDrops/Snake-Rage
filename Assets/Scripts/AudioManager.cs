using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip eatSound;
    public AudioClip gameoverSound;
    public AudioClip clickSound;

    public void PlayEatSound()
    {
        audioSource.PlayOneShot(eatSound);
    }

    public void PlayGameOverSound()
    {
        audioSource.PlayOneShot(gameoverSound);
    }

    public void PlayClickSound()
    {
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}
