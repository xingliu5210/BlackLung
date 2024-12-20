using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] popUps;
    private int popUpIndex;
    private bool leftArrowPressed = false;
    private bool rightArrowPressed = false;
    private bool switchtoBo = false;

    [SerializeField] private PlayerMovement playerMovement; // Reference to the PlayerMovement script
    private float originalJumpForce; // To store the original jump force
    [SerializeField] private Interaction interaction; // Reference to the Interaction script
    [SerializeField] private CharacterSwitcher characterSwitcher;


    void Start()
    {
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement reference is not set in the TutorialManager!");
            return;
        }
        // Save the original jump force value
        originalJumpForce = playerMovement.JumpForce;

        // Disable jumping by setting jumpForce to 0 initially
        playerMovement.JumpForce = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Show only the active popup
        for (int i = 0; i < popUps.Length; i++)
        {
            popUps[i].SetActive(i == popUpIndex);
        }

        // Handle the first popup logic
        if (popUpIndex == 0) 
        {
            if (Input.GetKeyDown(KeyCode.A)) 
            {
                leftArrowPressed = true;
            }

            if (Input.GetKeyDown(KeyCode.D)) 
            {
                rightArrowPressed = true;
            }

            // If both keys are pressed, move to the next popup
            if (leftArrowPressed && rightArrowPressed) 
            {
                popUpIndex++;
                ResetKeyPressStates();
            }
        }
        else if (popUpIndex == 1)
        {
            // Re-enable jumping by restoring the original jumpForce
            playerMovement.JumpForce = originalJumpForce;

            // Handle the second popup logic
            if (Input.GetKeyDown(KeyCode.Space)) 
            {
                popUpIndex++;
            }
        }
        else if (popUpIndex == 2)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                if(!switchtoBo)
                {
                    // Initial switch to Bo
                    switchtoBo = true;  
                }
                else
                {
                    // If already switched, proceed to next popup
                    popUpIndex++;
                }
            }
        }
        else if (popUpIndex == 3)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                interaction.Interact(); // Call the Interaction method
                if (CheckIfItemPickedUp())
                {
                    popUpIndex++;
                }
            }
        }
        else if (popUpIndex == 4)
        {
            // Handle whip hook interaction (J key or left mouse button)
            if (Input.GetKeyDown(KeyCode.J) || Input.GetMouseButtonDown(0))
            {
                if (CheckWhipHookInteraction()) // Custom function to verify interaction success
                {
                    Debug.Log("Successfully interacted with the whip hook.");
                    popUpIndex++;
                }
                else
                {
                    Debug.Log("Failed to interact with the whip hook. Try again.");
                }
            }
        }
        else if (popUpIndex == 5)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                popUpIndex++;
            }
        }
        else if (popUpIndex == 6)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                popUpIndex++;
            }
        }
        else if (popUpIndex == 7)
        {
            // Tunnel interaction tutorial
            PlayerMovement controlledCharacter = characterSwitcher.GetControlledCharacter();
            TunnelCrawl boTunnelCrawl = controlledCharacter?.GetComponent<TunnelCrawl>();

            if (boTunnelCrawl != null && boTunnelCrawl.IsInCooldown)
            {
                popUpIndex++;
            }
        }
    }

    // Helper method to reset key press states
    private void ResetKeyPressStates()
    {
        leftArrowPressed = false;
        rightArrowPressed = false;
    }

    private bool CheckWhipHookInteraction()
    {
        WhipHookChecker whipHookChecker = null;

        // Check if the playerMovement is AmosControls
        if (playerMovement is AmosControls amosControls)
        {
            // using a getter method
            whipHookChecker = amosControls.GetWhipHookChecker();
        }

        // Dynamically get WhipHookChecker as a fallback
        if (whipHookChecker == null)
        {
            whipHookChecker = playerMovement.GetComponent<WhipHookChecker>();
        }

        if (whipHookChecker != null)
        {
            WhipHook closestHook = whipHookChecker.GetClosestHook(playerMovement.transform.position);
            if (closestHook != null)
            {
                Debug.Log($"Interacted with WhipHook: {closestHook.name}");
                return true; // Successful interaction
            }
            else
            {
                Debug.Log("No hooks in range.");
            }
        }
        else
        {
            Debug.LogError("WhipHookChecker is not assigned to the player.");
        }

        return false; // Interaction failed
    }

    private bool CheckIfItemPickedUp()
    {
        // Use the public method or property to get the interactable
        Collider interactable = interaction.GetCurrentInteractable();
        
        if (interactable != null)
        {
            Item item = interactable.GetComponent<Item>();
            if (item != null)
            {
                // Check if the item is of type PickUp or Fuel
                //if (item.type == Item.InteractionType.PickUp || item.type == Item.InteractionType.Fuel || item.type == Item.InteractionType.Key)
                if (Enum.IsDefined(typeof(Item.InteractionType), item.type))
                {
                    Debug.Log($"Successfully picked up an item of type: {item.type}");
                    return true; // Pickup was successful
                }
            }
        }
        Debug.Log("No item picked up.");
        return false; // Pickup failed
    }
}
