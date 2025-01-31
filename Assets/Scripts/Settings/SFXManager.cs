using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] sfxArray;

    public void PlayRandomSFX()
    {
        if (sfxArray.Length == 0)
        {
            Debug.LogWarning("No SFX available.");
            return;
        }

        int randomIndex = Random.Range(0, sfxArray.Length);
        audioSource.clip = sfxArray[randomIndex];
        audioSource.Play();
    }
}
