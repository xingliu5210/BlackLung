using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [Tooltip("Index of the popup to trigger in the TutorialManager.")]
    public int popUpIndex;
    [SerializeField] private bool boTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (boTrigger)
        {
            if (other.CompareTag("Dog"))
            {
                TriggerPopUp();
            }
        }
        // Check if the player entered the trigger
        if (other.CompareTag("Player"))
        {

            TriggerPopUp();
        }
    }

    private void TriggerPopUp()
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

        // ✅ Save tutorial state immediately
        string tutorialKey = "Tutorial_" + popUpIndex;
        PlayerPrefs.SetInt(tutorialKey, 1);
        PlayerPrefs.Save();

        // Destroy this trigger after it has been activated
        // Destroy(gameObject);
        gameObject.SetActive(false);
    }
}
