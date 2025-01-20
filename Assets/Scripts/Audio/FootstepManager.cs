using UnityEngine;

public class FootstepAudioManager : MonoBehaviour
{
    [Header("Footstep Audio Sources")]
    public AudioSource leftFootAudioSource;
    public AudioSource rightFootAudioSource;
    public AudioSource clothsAudioSource;
    public AudioSource LeftFootBoSource;
    public AudioSource RightFootBoSource;

    [Header("Footstep Audio Clips")]
    public AudioClip[] leftFootClips;
    public AudioClip[] rightFootClips;
    public AudioClip[] clothsClips;
    public AudioClip[] LeftFootBoClips;
    public AudioClip[] RightFootBoClips;

    [Header("Footstep Timing")]
    public float footstepInterval = 0.45f; // Time between footsteps

    private bool isLeftFoot = true; // To toggle between left and right
    private bool isLeftFootBo = true; // To toggle between left and right


    public void PlayFootstep()
    {
        // Choose a random clip from the respective container
        if (isLeftFoot)
        {
            PlayRandomClip(leftFootAudioSource, leftFootClips);
            PlayCloths();
        }
        else
        {
            PlayRandomClip(rightFootAudioSource, rightFootClips);
            PlayCloths();
        }

        // Toggle foot
        isLeftFoot = !isLeftFoot;
    }

    public void PlayCloths()
    {
        PlayRandomClip(clothsAudioSource, clothsClips);
    }

    public void PlayBoFootstep()
    {
        // Choose a random clip from the respective container
        if (isLeftFootBo)
        {
            PlayRandomClip(LeftFootBoSource, LeftFootBoClips);
        }
        else
        {
            PlayRandomClip(RightFootBoSource, RightFootBoClips);
        }

        // Toggle foot
        isLeftFootBo = !isLeftFootBo;
    }

    void PlayRandomClip(AudioSource audioSource, AudioClip[] clips)
    {
        if (clips.Length == 0) return;

        int randomIndex = Random.Range(0, clips.Length);
        audioSource.clip = clips[randomIndex];
        audioSource.Play();
    }

}
