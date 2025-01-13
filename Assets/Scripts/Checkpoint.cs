using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameObject ally;

    private Vector3 CheckpointPosition;

    public float spawnXOffset;
    private Vector3 spawnPosition;

    // Start is called before the first frame update
    private void Start()
    {
        CheckpointPosition = transform.position;

        spawnPosition = CheckpointPosition;
        spawnPosition.x += spawnXOffset;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            GetComponent<PlayerHealth>().FullHeal();
            AudioManager.Instance.PlayWarmLight();
            CheckpointPosition = other.transform.position;

            spawnPosition = CheckpointPosition;
            spawnPosition.x += spawnXOffset;

            ally.GetComponent<Checkpoint>().AllyCheckpoint();

            Debug.Log("Checkpoint set to " + CheckpointPosition);
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
}
