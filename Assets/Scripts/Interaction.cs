using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{

    [SerializeField] private float interactionRadius = 2f; // Radius to detect items
    [SerializeField] private LayerMask interactableLayer = 1 << 10;  // Layer for interactable items
    private PlayerInventory playerInventory; // Reference to the player's inventory

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
            }
            else
            {
                Debug.LogWarning("Interactable object does not have an Item component.");
            }
        }
        else
        {
            Debug.Log("No interactable objects nearby.");
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the interaction radius in the editor for debugging
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }

}
