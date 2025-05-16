using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{

    [SerializeField] private float interactionRadius = 2f; // Radius to detect items
    [SerializeField] private LayerMask interactableLayer = 1 << 10;  // Layer for interactable items
    private PlayerInventory playerInventory; // Reference to the player's inventory
    private Elevator currentElevator; // Reference to the elevator being interacted with
    private InteractionZone currentInteractionZone;
    private GameObject rock; // Reference to the rock being interacted with
    public GameObject mount; // Reference to the rock being interacted with

    private void Awake()
    {
        playerInventory = GetComponent<PlayerInventory>();
        if (playerInventory == null)
        {
            Debug.LogError("PlayerInventory component is missing.");
        }
    }

    private Collider currentInteractable;

    /// <summary>
    /// Called to detect and interact with nearby items.
    /// </summary>
    public void Interact()
    {
        // Detect interactable objects within the radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRadius, interactableLayer);

        if (colliders.Length > 0)
        {
            currentInteractable = colliders[0]; // Use the first detected interactable

            // Attempt to interact with the item
            Item item = currentInteractable.GetComponent<Item>();
            if (item != null)
            {
                item.Interact(playerInventory); // Pass the player inventory to the item
                return;
            }
            else
            {
                Debug.LogWarning("Interactable object does not have an Item component.");
            }
        }
        else if (currentElevator != null)
        {
            // If an elevator is currently detected, interact with it
            Debug.Log("Interacting with the elevator.");
            currentElevator.ActivateElevator(playerInventory);
        }
        else if (currentInteractionZone != null)
        {
            Debug.Log("Interacting with elevator system");
            currentInteractionZone.InteractWithElevator();
        }
        else if (rock != null)
        {
            // If a rock is currently detected, interact with it
            Debug.Log("Interacting with the rock.");
            GetComponent<Pickaxe>().rock = rock;
            GetComponent<Pickaxe>().Mining();
        }
        else if (mount != null)
        {
            Debug.Log("Mounting object.");
            GetComponent<PlayerMovement>().mount = mount;
            GetComponent<PlayerMovement>().OnMount();
        }
        else
        {
            Debug.Log("No interactable objects nearby.");
        }
    }

        /// <summary>
    /// Detect when the player enters an elevator trigger.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Elevator")) // Ensure the elevator has the correct tag
        {
            Debug.Log($"Player entered the elevator trigger: {other.name}");
            currentElevator = other.GetComponent<Elevator>();

            if (currentElevator == null)
            {
                Debug.LogError("Elevator script missing on the elevator object.");
            }
        }

        if (other.CompareTag("Rock")) // Ensure the elevator has the correct tag
        {
            Debug.Log($"Player entered the mining trigger: {other.name}");
            rock = other.gameObject;
        }

        if (other.CompareTag("Mount")) // Ensure the elevator has the correct tag
        {
            Debug.Log($"Player entered the mount trigger: {other.name}");
            mount = other.gameObject;
        }

        if (other.CompareTag("ElevatorSystem"))
        {
            Debug.Log("Entered elevator system interaction zone");
            currentInteractionZone = other.GetComponent<InteractionZone>();
        }
    }

    /// <summary>
    /// Detect when the player exits an elevator trigger.
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Elevator")) // Ensure the elevator has the correct tag
        {
            Debug.Log($"Player exited the elevator trigger: {other.name}");
            if (currentElevator != null && other.GetComponent<Elevator>() == currentElevator)
            {
                currentElevator = null; // Clear the current elevator reference
            }
        }

        if (other.CompareTag("Rock")) // Ensure the rock has the correct tag
        {
            Debug.Log($"Player exited the rock trigger: {other.name}");
            rock = null;  // Clear the rock reference
        }

        if (other.CompareTag("Mount")) // Ensure the mount has the correct tag
        {
            Debug.Log($"Player exited the mount trigger: {other.name}");
            mount = null;  // Clear the mount reference
        }

        if (other.CompareTag("ElevatorSystem"))
        {
            Debug.Log("Exit elevator system interaction zone");
            currentInteractionZone = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the interaction radius in the editor for debugging
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }

    public Collider GetCurrentInteractable()
    {
        return currentInteractable;
    }

}
