using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip introMusic;
    public AudioClip bossMusic;

    private void Start()
    {
        // Comienza con la música inicial
        PlayIntroMusic();
    }

    public void PlayIntroMusic()
    {
        audioSource.clip = introMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayBossMusic()
    {
        audioSource.clip = bossMusic;
        audioSource.loop = true;
        audioSource.Play();
    }
}

