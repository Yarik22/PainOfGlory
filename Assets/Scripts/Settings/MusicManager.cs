using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] musicTracks;
    private int currentTrackIndex = 0;

    private void Update()
    {
        if (!audioSource.isPlaying && musicTracks.Length > 0)
        {
            PlayNextTrack();
        }
    }

    public void PlayMusic(int trackIndex)
    {
        if (musicTracks.Length == 0 || trackIndex < 0 || trackIndex >= musicTracks.Length)
        {
            Debug.LogWarning("Invalid track index");
            return;
        }

        currentTrackIndex = trackIndex;
        audioSource.clip = musicTracks[currentTrackIndex];
        audioSource.Play();
    }

    public void PlayNextTrack()
    {
        currentTrackIndex = (currentTrackIndex + 1) % musicTracks.Length;
        PlayMusic(currentTrackIndex);
    }

    public void PlayPreviousTrack()
    {
        currentTrackIndex = (currentTrackIndex - 1 + musicTracks.Length) % musicTracks.Length;
        PlayMusic(currentTrackIndex);
    }

    public void PlayRandomTrack()
    {
        if (musicTracks.Length == 0)
        {
            Debug.LogWarning("No music tracks available.");
            return;
        }

        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, musicTracks.Length);
        } while (randomIndex == currentTrackIndex);

        PlayMusic(randomIndex);
    }
}
