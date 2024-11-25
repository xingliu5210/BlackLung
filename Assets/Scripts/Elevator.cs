using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private Transform destination; // Destination floor for the elevator
    [SerializeField] private float moveSpeed = 2f;  // Speed at which the elevator moves

    private Vector3 originalPosition;              // Original position of the elevator
    private bool isActivated = false;              // Tracks if the elevator is permanently activated
    private bool isMoving = false;                 // Tracks if the elevator is currently moving
    private bool atDestination = false;           // Tracks if the elevator is at the destination

    private void Awake()
    {
        // Store the original position of the elevator
        originalPosition = transform.position;
    }

    private void Update()
    {
        // Smoothly move the elevator to the target position if it's activated and moving
        if (isMoving)
        {
            Vector3 targetPosition = atDestination ? originalPosition : destination.position;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Stop moving if the elevator reaches the target position
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                isMoving = false;
                atDestination = !atDestination; // Toggle the destination flag
                Debug.Log("Elevator reached the " + (atDestination ? "destination." : "original position."));
            }
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
                Debug.Log("Elevator activated permanently!");
                StartMoving();
            }
            else
            {
                Debug.Log("Not enough keys to activate the elevator.");
            }
        }
        else
        {
            Debug.Log("Elevator is already activated.");
            StartMoving();
        }
    }

    private void StartMoving()
    {
        if (destination != null)
        {
            Debug.Log("Elevator is moving to the destination.");
            isMoving = true;
        }
        else
        {
            Debug.LogError("Destination not set for the elevator.");
        }
    }
}