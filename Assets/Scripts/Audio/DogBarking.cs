using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogBarking : MonoBehaviour
{
    [Header("Defensive Barking")]
    [SerializeField] private AudioSource dogBarking;
    [SerializeField] private AudioClip[] dogBarkingClips;

    [Header("Barking Settings")]
    [SerializeField] private float minDelayBetweenBarks = 0.5f; 
    [SerializeField] private float maxDelayBetweenBarks = 1.5f; 
    [SerializeField] private int minBarkCount = 1; 
    [SerializeField] private int maxBarkCount = 4;

    public void StartBarking()
    {
        int randomBarkCount = Random.Range(minBarkCount, maxBarkCount + 1);
        StartCoroutine(BarkSequence(randomBarkCount));
    }

    private IEnumerator BarkSequence(int barkCount)
    {
        for (int i = 0; i < barkCount; i++)
        {
            PlayRandomBark();

            // Randomize delay between barks
            float randomDelay = Random.Range(minDelayBetweenBarks, maxDelayBetweenBarks);
            yield return new WaitForSeconds(randomDelay);
        }
    }

    // Play raandom bark sound for the clips
    private void PlayRandomBark()
    {
        if (dogBarkingClips.Length > 0 && dogBarking != null)
        {
            int randomIndex = Random.Range(0, dogBarkingClips.Length);
            dogBarking.clip = dogBarkingClips[randomIndex];
            dogBarking.Play();
        }
    }
}
