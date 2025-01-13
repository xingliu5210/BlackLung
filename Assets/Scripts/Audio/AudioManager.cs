using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Music")]
    [SerializeField] private AudioSource _themeMusic;

    [Header("Ambiences")]
    //[SerializeField] private AudioSource _ambienceCaveSource;
    [SerializeField] private AudioSource _waterDripSource;
    [SerializeField] private AudioClip[] _waterDripClips;
    [SerializeField] private AudioSource _rocksFallSource;
    [SerializeField] private AudioClip[] _rocksFallClips;

    [Header("SFX")]
    [SerializeField] private AudioSource _pickUpFuel;
    [SerializeField] private AudioSource _pickUpKey;
    [SerializeField] private AudioSource _SwitchOnLantern;
    [SerializeField] private AudioSource _SwitchOffLantern;
    [SerializeField] private AudioSource _Using_Elevator;
    [SerializeField] private AudioSource _StalactitesFall;
    [SerializeField] private AudioSource _WarmLight;

    private bool themeMusicPlaying = false;
    private bool ambiencePlaying = false;
    private bool hasPlayedCheckpointSound = false;

    private void Awake()
    {
        // Ensure only one instance of AudioManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep AudioManager across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartThemeMusic()
    {
        if (!themeMusicPlaying)
        {
            _themeMusic.Play();
            themeMusicPlaying = true;
        }
    }
    // Add a method to play the fuel pickup sound
    public void PlayPickUpFuel()
    {
        _pickUpFuel.Play();
    }
    // Add a method to play the key pickup sound
    public void PlayPickUpKey()
    {
        _pickUpKey.Play();
    }

    // Add a method to play a random water drip sound
    public void PlayWaterDrip()
    {
        if (!ambiencePlaying && _waterDripClips.Length > 0)
        {
            StartCoroutine(PlayAmbienceSound(_waterDripSource, _waterDripClips));
        }
    }

    public void PlayRocksFall()
    {
        if (!ambiencePlaying && _rocksFallClips.Length > 0)
        {
            StartCoroutine(PlayAmbienceSound(_rocksFallSource, _rocksFallClips));
        }
    }

    private IEnumerator PlayAmbienceSound(AudioSource source, AudioClip[] clips)
    {
        ambiencePlaying = true;

        int randomIndex = Random.Range(0, clips.Length);
        source.clip = clips[randomIndex];
        source.Play();

        yield return new WaitForSeconds(source.clip.length); // Wait for the sound to finish

        ambiencePlaying = false;
    }

    public void PlaySwitchOnLantern()
    {
        _SwitchOnLantern.Play();
    }

    public void PlaySwitchOffLantern()
    {
        _SwitchOffLantern.Play();
    }

    public void PlayUsingElevator()
    {
        _Using_Elevator.Play();
    }

    public void PlayStalactitesFall()
    {
        _StalactitesFall.Play();
        _StalactitesFall.pitch = Random.Range(0.9f, 1.1f); // Add pitch randomization

    }

    public void PlayWarmLight()
    {
        if (!hasPlayedCheckpointSound)
        {
            _WarmLight.Play();
            hasPlayedCheckpointSound = true;
            StartCoroutine(ResetCheckpointSoundFlag());
        }
    }

    private IEnumerator ResetCheckpointSoundFlag()
    {
        yield return new WaitForSeconds(10f); // Wait for 10 seconds
        hasPlayedCheckpointSound = false; // Reset the flag
    }
}