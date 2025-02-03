using UnityEngine;

public class AudioPlayer : StateMachineBehaviour
{
    [SerializeField] private AudioClip sound;
    private AudioSource audioSource;
    [SerializeField] private float speedUpFactor = 1f;
    [SerializeField] private bool reverse = false;

    private AudioClip reversedSound;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        audioSource = animator.GetComponent<AudioSource>();

        if (audioSource != null && sound != null)
        {
            if (reverse && reversedSound == null)
            {
                reversedSound = ReverseAudioClip(sound);
            }

            AudioClip clipToPlay = reverse ? reversedSound : sound;

            audioSource.clip = clipToPlay;
            audioSource.PlayOneShot(clipToPlay);

            float animationDuration = stateInfo.length;
            float audioDuration = clipToPlay.length;

            if (audioDuration > 0f && animationDuration > 0f)
            {
                float speedFactor = animationDuration / audioDuration * speedUpFactor;
                audioSource.pitch = speedFactor;
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private AudioClip ReverseAudioClip(AudioClip clip)
    {
        float[] samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);

        System.Array.Reverse(samples);

        AudioClip reversedClip = AudioClip.Create(clip.name + "_Reversed", clip.samples, clip.channels, clip.frequency, false);
        reversedClip.SetData(samples, 0);

        return reversedClip;
    }
}
