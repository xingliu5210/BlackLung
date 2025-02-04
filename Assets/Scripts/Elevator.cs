using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private Elevator pairedElevator; // Destination floor for the elevator
    [SerializeField] private float teleportCooldown = 1f; // Cooldown to prevent instant re-teleportation  // Speed at which the elevator moves
    private bool isActivated;              // Tracks if the elevator is permanently activated
    private bool isCooldown;               // Tracks if the teleport is on cooldown

    private void Awake()
    {
        isActivated = false;
        isCooldown = false;
        // Ensure the paired elevator is set
        if (pairedElevator == null)
        {
            Debug.LogError("Paired elevator not assigned for " + gameObject.name);
        }
    }

    public void ActivateElevator(PlayerInventory playerInventory)
    {
        if (!isActivated)
        {
            if (playerInventory.GetKeyCount() > 0)
            {
                playerInventory.UseKey(); // Decrease the key count
                isActivated = true;
                Debug.Log(gameObject.name + " activated permanently!");

                // Activate the paired elevator
                if (pairedElevator != null)
                {
                    pairedElevator.ActivatePairedElevator();
                }
            }
            else
            {
                Debug.Log("Not enough keys to activate the elevator.");
            }
        }
        else
        {
            Debug.Log(gameObject.name + " is already activated.");
        }
        // Teleport the player
        if (!isCooldown)
        {
            TeleportPlayer();
        }

    }

    private void ActivatePairedElevator()
    {
        isActivated = true;
        Debug.Log(gameObject.name + " paired elevator activated.");
    }

    private void TeleportPlayer()
    {
        if (isActivated) 
        {    if(pairedElevator != null)
            {
                // Find the player
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                GameObject dog = GameObject.FindGameObjectWithTag("Dog");
                if (player != null)
                {
                    Debug.Log($"Teleporting player to {pairedElevator.name}");
                    player.transform.position = pairedElevator.transform.position;
                    dog.transform.position = pairedElevator.transform.position;


                    // Start cooldown
                    StartCoroutine(TeleportCooldown());
                }
                else
                {
                    Debug.LogError("Player not found in the scene.");
                }
            }
            else
            {
                Debug.LogError("Paired elevator not assigned for teleportation.");
            }
        }else {
            Debug.LogError("The elevator isn't activated yet.");
        }
    }

    private IEnumerator TeleportCooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(teleportCooldown);
        isCooldown = false;
    }
}