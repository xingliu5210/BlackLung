using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Elevator : MonoBehaviour
{
    [SerializeField] private Transform destination; // Destination floor for the elevator
    [SerializeField] private float moveSpeed = 2f;  // Speed at which the elevator moves
    private bool isActivated = false; // Tracks if the elevator is permanently activated
    private bool isMoving = false;    // Tracks if the elevator is currently moving
    private Transform passenger; // Reference to the player riding the elevator
    private Rigidbody passengerRigidbody;          // Reference to the player's Rigidbody

    private void Update()
    {
        // Smoothly move the elevator to the destination if it's activated and moving
        if (isMoving && destination != null)
        {
            Vector3 previousPosition = transform.position; // Store the elevator's previous position

            transform.position = Vector3.MoveTowards(transform.position, destination.position, moveSpeed * Time.deltaTime);

            // If there's a passenger, update their position relative to the elevator's movement
            if (passenger != null && passengerRigidbody != null)
            {
                Vector3 delta = transform.position - previousPosition; // Calculate the elevator's movement delta
                passengerRigidbody.MovePosition(passengerRigidbody.position + delta); // Apply the delta to the player's Rigidbody
            }else
            {
                // Alternative: Directly update the player's position
                passenger.position += transform.position - previousPosition;
            }

            // Stop moving if the elevator reaches the destination
            if (Vector3.Distance(transform.position, destination.position) < 0.01f)
            {
                isMoving = false;
                Debug.Log("Elevator reached the destination.");

                // Allow the player to move independently again
                if (passengerRigidbody != null)
                {
                    passengerRigidbody.isKinematic = false;
                }

                // Allow the player to move independently again
                ResetPassenger();
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
                MoveToDestination();
            }
            else
            {
                Debug.Log("Not enough keys to activate the elevator.");
            }
        }
        else
        {
            Debug.Log("Elevator is already activated.");
            MoveToDestination();
        }

    }

    private void MoveToDestination()
    {
        if (destination != null)
        {
            Debug.Log("Elevator is moving to the destination.");
            isMoving = true;

            // Ensure the passenger remains in sync
            if (passengerRigidbody != null)
            {
                passengerRigidbody.isKinematic = true; // Disable physics during elevator movement
            }

        }
        else
        {
            Debug.LogError("Destination not set for the elevator.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the elevator.");
            passenger = other.transform; // Set the passenger reference

            // Get the player's Rigidbody
            if (passenger.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                passengerRigidbody = rb;
                // Only make the Rigidbody kinematic if the elevator starts moving
                if (isMoving)
                {
                    passengerRigidbody.isKinematic = true;
                }
            }
            else
            {
                Debug.LogWarning("Player does not have a Rigidbody.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the elevator.");
            passenger = other.transform; // Set the passenger reference

            // Get the player's Rigidbody
            if (passenger.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                passengerRigidbody = rb;
            }
            else
            {
                Debug.LogWarning("Player does not have a Rigidbody.");
            }
        }
    }
    private void ResetPassenger()
    {
        if (passengerRigidbody != null)
        {
            passengerRigidbody.isKinematic = false; // Re-enable physics
        }

        // Clear references
        passenger = null;
        passengerRigidbody = null;
    }

}