using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [Tooltip("Index of the popup to trigger in the TutorialManager.")]
    public int popUpIndex;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the trigger
        if (other.CompareTag("Player"))
        {
            TutorialManager tutorialManager = FindObjectOfType<TutorialManager>();

            if (tutorialManager != null)
            {
                tutorialManager.TriggerPopup(popUpIndex);
                Debug.Log($"Triggered popup index: {popUpIndex}");
            }
            else
            {
                Debug.LogError("TutorialManager not found in the scene.");
            }

            // Destroy this trigger after it has been activated
            Destroy(gameObject);
        }
    }
}
