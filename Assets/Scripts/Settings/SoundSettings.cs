using UnityEngine;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [SerializeField] private VolumeManager volumeManager;
    [SerializeField] private MusicManager musicManager;
    [SerializeField] private SFXManager sfxManager;

    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    private void Start()
    {
        masterVolumeSlider.value = volumeManager.GetMasterVolume();
        musicVolumeSlider.value = volumeManager.GetMusicVolume();
        sfxVolumeSlider.value = volumeManager.GetSFXVolume();

        volumeManager.SetMasterVolume(masterVolumeSlider.value);
        volumeManager.SetMusicVolume(musicVolumeSlider.value);
        volumeManager.SetSFXVolume(sfxVolumeSlider.value);

        musicManager.PlayRandomTrack();
    }

    public void OnMasterVolumeChanged() => volumeManager.SetMasterVolume(masterVolumeSlider.value);
    public void OnMusicVolumeChanged() => volumeManager.SetMusicVolume(musicVolumeSlider.value);
    public void OnSFXVolumeChanged() => volumeManager.SetSFXVolume(sfxVolumeSlider.value);
    public void PlayRandomSFX() => sfxManager.PlayRandomSFX();
}
