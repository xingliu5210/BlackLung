using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameObject ally;

    private Vector3 CheckpointPosition;

    public float spawnXOffset;
    private Vector3 spawnPosition;

    public GameObject saveNotificationPanel; // Reference to "Game Saved UI panel"

    public float saveNotificationDuration = 2f;
    [SerializeField] private float checkpointCooldown = 1f;
    private bool checkpointActive = false;

    private Coroutine notificationCoroutine;

    // Start is called before the first frame update
    private void Start()
    {
        CheckpointPosition = transform.position;

        spawnPosition = CheckpointPosition;
        spawnPosition.x += spawnXOffset;

        // Hide the save UI at start
        if (saveNotificationPanel != null)
        {
            saveNotificationPanel.SetActive(false);
        }

        StartCoroutine(EnableCheckpointTrigger());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!checkpointActive) return;

        if (other.CompareTag("Checkpoint"))
        {
            GetComponent<PlayerHealth>().FullHeal();

            CheckpointPosition = other.transform.position;
            spawnPosition = CheckpointPosition;
            spawnPosition.x += spawnXOffset;
            // CheckpointPosition = other.transform.position;
            // ally.transform.position = spawnPosition;

            ally.GetComponent<Checkpoint>().AllyCheckpoint();

            bool allyIsCloseEnough = Vector3.Distance(ally.transform.position, spawnPosition) < 2.0f;
            bool allyIsFollowing = ally.GetComponent<BoControls>()?.amosback ?? false;

            // Only teleport ally if NOT close and NOT following
            if (!allyIsCloseEnough && !allyIsFollowing)
            {
                // ally.transform.position = spawnPosition;
                ally.GetComponent<Checkpoint>().AllyCheckpoint();
                Debug.Log("Teleported ally to checkpoint.");
            } 

            Debug.Log("Checkpoint set to " + CheckpointPosition);
            InventoryManager.Instance.SaveInventoryAtCheckpoint();
            // SaveSystem.SaveGame(other.transform, ally.transform);
            StartCoroutine(SaveAfterPausingBo());

            // Show "Game Saved" notification
            if (saveNotificationPanel != null)
            {
                if (notificationCoroutine != null)
                    StopCoroutine(notificationCoroutine);
                
                notificationCoroutine = StartCoroutine(ShowSaveNotification());
            }
        }
    }

    public void AllyCheckpoint()
    {
        CheckpointPosition = ally.GetComponent<Checkpoint>().CheckpointPosition;

        spawnPosition = CheckpointPosition;
        spawnPosition.x += spawnXOffset;
    }

    public void Respawn()
    {
        GetComponent<PlayerMovement>().Freeze();
        transform.position = spawnPosition;
        GetComponent<PlayerHealth>().FullHeal();
    }

    private IEnumerator ShowSaveNotification()
    {
        Debug.Log("Showing save notification"); 
        saveNotificationPanel.SetActive(true);
        yield return new WaitForSeconds(saveNotificationDuration);
        saveNotificationPanel.SetActive(false);
        Debug.Log("Hiding save notification"); 
    }

    private IEnumerator EnableCheckpointTrigger()
    {
        checkpointActive = false;
        yield return new WaitForSeconds(checkpointCooldown);
        checkpointActive = true;
    }

    private IEnumerator SaveAfterPausingBo()
    {
        BoControls boControl = ally.GetComponent<BoControls>();
        if (boControl != null)
        {
            boControl.isMovementPaused = true; // ✨ Pause Bo movement
        }

        yield return new WaitForEndOfFrame(); // Wait one frame to stabilize

        SaveSystem.SaveGame(transform, ally.transform);

        if (saveNotificationPanel != null)
        {
            if (notificationCoroutine != null)
                StopCoroutine(notificationCoroutine);

            notificationCoroutine = StartCoroutine(ShowSaveNotification());
        }

        // After 0.2 seconds, resume Bo's movement
        yield return new WaitForSeconds(0.2f);

        if (boControl != null)
        {
            boControl.isMovementPaused = false; // ✨ Resume Bo movement
        }
    }
}
