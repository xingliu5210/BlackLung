using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private Transform destination; // Destination floor for the elevator
    [SerializeField] private float moveSpeed = 2f;  // Speed at which the elevator moves
    private bool isActivated = false;              // Tracks if the elevator is permanently activated
    private bool isMoving = false;                 // Tracks if the elevator is currently moving

    private void Update()
    {
        // Smoothly move the elevator to the destination if it's activated and moving
        if (isMoving && destination != null)
        {
            // Move the platform (and any children, including the player)
            transform.position = Vector3.MoveTowards(transform.position, destination.position, moveSpeed * Time.deltaTime);

            // Stop moving if the elevator reaches the destination
            if (Vector3.Distance(transform.position, destination.position) < 0.01f)
            {
                isMoving = false;
                Debug.Log("Elevator reached the destination.");
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