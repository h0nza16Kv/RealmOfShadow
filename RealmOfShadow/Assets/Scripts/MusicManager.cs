using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;  
    public AudioClip[] sceneMusic; 

    void Start()
    {
        PlayRandomMusic();  
    }

    public void PlayRandomMusic()
    {
        if (sceneMusic.Length > 0)
        {
            audioSource.clip = sceneMusic[Random.Range(0, sceneMusic.Length)];
            audioSource.Play();
        }
    }

    public void ChangeMusic() 
    {
        PlayRandomMusic();
    }
}
