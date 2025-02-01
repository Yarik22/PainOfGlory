using UnityEngine;
using UnityEngine.Audio;

public class VolumeManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string masterVolumeParameter = "MasterVolume";
    [SerializeField] private string musicVolumeParameter = "MusicVolume";
    [SerializeField] private string sfxVolumeParameter = "SFXVolume";

    public void SetVolume(string parameter, float volume)
    {
        if (volume < 1) volume = .001f;
        audioMixer.SetFloat(parameter, Mathf.Log10(volume / 100) * 20f);
        PlayerPrefs.SetFloat($"Saved{parameter}", volume);
    }

    public float GetVolume(string parameter)
    {
        return PlayerPrefs.GetFloat($"Saved{parameter}", 100);
    }

    public void SetMasterVolume(float volume) => SetVolume(masterVolumeParameter, volume);
    public void SetMusicVolume(float volume) => SetVolume(musicVolumeParameter, volume);
    public void SetSFXVolume(float volume) => SetVolume(sfxVolumeParameter, volume);

    public float GetMasterVolume() => GetVolume(masterVolumeParameter);
    public float GetMusicVolume() => GetVolume(musicVolumeParameter);
    public float GetSFXVolume() => GetVolume(sfxVolumeParameter);
}
