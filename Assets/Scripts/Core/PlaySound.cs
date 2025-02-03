using UnityEngine;

public class PlaySound : MonoBehaviour
{
    [SerializeField] private AudioClip sound;
    [SerializeField] private AudioSource audioSource;

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void PlayClip()
    {
        if (audioSource != null && sound != null)
        {
            audioSource.PlayOneShot(sound);
        }
    }

    public void StopClip()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
