using UnityEngine;
using UnityEngine.InputSystem;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audios;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            PlayAudio();
        }
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            PauseAudio();
        }
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            PlayRandom();
        }
    }
    public void PlayAudio()
    {
        audioSource.Play();
    }
    public void PauseAudio()
    {
        audioSource.Pause();
    }
    public void PlayRandom()
    {
        int randomNum = Random.Range(0, audios.Length);
        audioSource.PlayOneShot(audios[randomNum]);
    }
}
