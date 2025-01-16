using System.Collections;
using UnityEngine;

public class AudioEvents : MonoBehaviour
{
    [Header("Randomizer Settings")]
    [SerializeField] private float minDelay = 5f; // Minimum delay between sounds
    [SerializeField] private float maxDelay = 15f; // Maximum delay between sounds 

    private void Start()
    {
        // Start the theme music when the game begins
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StartThemeMusic();
        }

        // Start the ambience routine
        StartCoroutine(AmbienceRoutine());
    }

    private IEnumerator AmbienceRoutine()
    {
        while (true)
        {
            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);

            // Trigger randomized ambience sounds via AudioManager
            if (AudioManager.Instance != null)
            {
                // Randomly decide between water drip and rocks fall
                if (Random.value > 0.5f)
                {
                    AudioManager.Instance.PlayWaterDrip();
                }
                else
                {
                    AudioManager.Instance.PlayRocksFall();
                }
            }
        }
    }
}
