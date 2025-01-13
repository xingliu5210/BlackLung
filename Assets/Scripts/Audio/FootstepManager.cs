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
    public float footstepInterval = 0.5f; // Time between footsteps

    private bool isLeftFoot = true; // To toggle between left and right
    private bool isLeftFootBo = true; // To toggle between left and right
    private bool isWalking = false; // To check if the player is walking
    private bool isGrounded = true; // To track if the character is grounded

    void Update()
    {
        // Here to check if the player is walking (Pressing D or A Key) and grounded
        if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)) && isGrounded)
        {
            StartWalking();
        }
        else
        {
            StopWalking();
        }
    }

    public void StartWalking()
    {
        if (!isWalking)
        {
            isWalking = true;
            StartCoroutine(PlayFootsteps());
        }
    }

    public void StopWalking()
    {
        isWalking = false;
    }

    private System.Collections.IEnumerator PlayFootsteps()
    {
        while (isWalking)
        {
            PlayFootstep();
            PlayCloths();
            yield return new WaitForSeconds(footstepInterval);
        }
    }

    public void PlayFootstep()
    {
        // Choose a random clip from the respective container
        if (isLeftFoot)
        {
            PlayRandomClip(leftFootAudioSource, leftFootClips);
        }
        else
        {
            PlayRandomClip(rightFootAudioSource, rightFootClips);
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
            PlayRandomClip(leftFootAudioSource, leftFootClips);
        }
        else
        {
            PlayRandomClip(rightFootAudioSource, rightFootClips);
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

    // This method is called whenever the player's grounded state changes
    public void UpdateGroundedState(bool grounded)
    {
        isGrounded = grounded;
    }
}
