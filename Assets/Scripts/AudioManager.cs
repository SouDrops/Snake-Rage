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
    public AudioClip gameplaySound;

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
        audioSource.PlayOneShot(clickSound);
    }

    public void PlayGameplaySound()
    {
        audioSource.PlayOneShot(clickSound);
    }

    public void StartGameplaySound()
    {
        if (audioSource != null && gameplaySound != null)
        {
            audioSource.clip = gameplaySound;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void StopGameplaySound()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
