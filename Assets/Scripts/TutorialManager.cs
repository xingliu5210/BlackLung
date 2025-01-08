using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] popUps;
    private int currentPopUpIndex = -1; // Currently active popup index
    private bool leftArrowPressed = false;
    private bool rightArrowPressed = false;
    private bool upArrowPressed = false;
    private bool downArrowPressed = false;
    private bool switchtoBo = false;
    private float batTutorialTimer = 4f; // Timer for HandleBatTutorial
    private float stalactiteTutorialTimer = 4f; // Timer for HandleStalactiteTutorial
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
        // playerMovement.JumpForce = 0;

        // Ensure all popups are inactive at the start
        HideAllPopUps();
    }

    private void HideAllPopUps()
    {
        foreach (var popUp in popUps)
        {
            popUp.SetActive(false);
        }
    }

    public void TriggerPopup(int popUpIndex)
    {
        if (popUpIndex < 0 || popUpIndex >= popUps.Length)
        {
            Debug.LogError($"Invalid popup index {popUpIndex}");
            return;
        }

        // Hide the current popup and show the triggered one
        HideAllPopUps();
        currentPopUpIndex = popUpIndex;
        popUps[currentPopUpIndex].SetActive(true);

        Debug.Log($"Popup {popUpIndex} triggered.");
    }

    // Update is called once per frame
    void Update()
    {
        // Handle specific popup logic based on the current popup index
        switch (currentPopUpIndex)
        {
            case 0: // Movement tutorial
                HandleMovementTutorial();
                break;
            case 1: // Switching characters tutorial
                HandleCharacterSwitchTutorial();
                break;
            case 2: // Interaction tutorial
                HandleInteractionTutorial();
                break;
            case 3: // Tunnel interaction tutorial
                HandleTunnelInteractionTutorial();
                break;
            case 4: // Jumping tutorial
                HandleJumpingTutorial();
                break;
            case 5: // Whip Hook interaction tutorial
                HandleWhipHookTutorial();
                break;
            case 6: // Tab key tutorial
                HandleMinecartTutorial();
                break;
            case 7: // Lantern tutorial
                HandleLanternTutorial();
                break;
            case 8: // Fetch tutorial
                HandleFetchTutorial();
                break;
            case 9: // Ladder tutorial
                HandleLadderTutorial();
                break;
            case 10: // Checkpoint tutorial
                HandleCheckpointTutorial();
                break;
            case 11: // Bat tutorial
                HandleBatTutorial();
                break;
                case 12: // Stalactite tutorial
                HandleStalactiteTutorial();
                break;
            default:
                break;
        }
    }

    private void HandleMovementTutorial()
    {
        if (Input.GetKeyDown(KeyCode.A)) leftArrowPressed = true;
        if (Input.GetKeyDown(KeyCode.D)) rightArrowPressed = true;

        if (leftArrowPressed && rightArrowPressed)
        {
            // Disable the current popup
            popUps[currentPopUpIndex].SetActive(false);
            currentPopUpIndex = -1; // Disable current popup
            ResetKeyPressStates();
            Debug.Log("Movement tutorial completed.");
        }
    }

    private void HandleCharacterSwitchTutorial()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!switchtoBo)
            {
                switchtoBo = true;
            }
            else
            {
                // Disable the current popup
                popUps[currentPopUpIndex].SetActive(false);
                currentPopUpIndex = -1; // Disable current popup
                Debug.Log("Character switch tutorial completed.");
            }
        }
    }

    private void HandleInteractionTutorial()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            interaction.Interact();
            if (CheckIfItemPickedUp())
            {
                // Disable the current popup
                popUps[currentPopUpIndex].SetActive(false);
                currentPopUpIndex = -1; // Disable current popup
                Debug.Log("Interaction tutorial completed.");
            }
        }
    }

    private void HandleTunnelInteractionTutorial()
    {
        PlayerMovement controlledCharacter = characterSwitcher.GetControlledCharacter();
        TunnelCrawl boTunnelCrawl = controlledCharacter?.GetComponent<TunnelCrawl>();

        if (boTunnelCrawl != null && boTunnelCrawl.IsInCooldown)
        {
            // Disable the current popup
            popUps[currentPopUpIndex].SetActive(false);
            currentPopUpIndex = -1; // Disable current popup
            Debug.Log("Tunnel interaction tutorial completed.");
        }
    }

    private void HandleJumpingTutorial()
    {
        // playerMovement.JumpForce = originalJumpForce;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Disable the current popup
            popUps[currentPopUpIndex].SetActive(false);
            currentPopUpIndex = -1; // Disable current popup
            Debug.Log("Jumping tutorial completed.");
        }
    }

    private void HandleWhipHookTutorial()
    {
        if (Input.GetKeyDown(KeyCode.J) || Input.GetMouseButtonDown(0))
        {
            if (CheckWhipHookInteraction())
            {
                // Disable the current popup
                popUps[currentPopUpIndex].SetActive(false);
                currentPopUpIndex = -1; // Disable current popup
                Debug.Log("Whip hook interaction tutorial completed.");
            }
        }
    }

    private void HandleMinecartTutorial()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Disable the current popup
            popUps[currentPopUpIndex].SetActive(false);
            currentPopUpIndex = -1; // Disable current popup
            Debug.Log("Tab key tutorial completed.");
        }
    }

    private void HandleLanternTutorial()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            // Disable the current popup
            popUps[currentPopUpIndex].SetActive(false);
            currentPopUpIndex = -1; // Disable current popup
            Debug.Log("Lantern tutorial completed.");
        }
    }

    private void HandleFetchTutorial()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Disable the current popup
            popUps[currentPopUpIndex].SetActive(false);
            currentPopUpIndex = -1; // Disable current popup
            Debug.Log("Fetch tutorial completed.");
        }
    }

    private void HandleLadderTutorial()
    {
        if (Input.GetKeyDown(KeyCode.W)) upArrowPressed = true;
        if (Input.GetKeyDown(KeyCode.S)) downArrowPressed = true;

        if (upArrowPressed && downArrowPressed)
        {
            // Disable the current popup
            popUps[currentPopUpIndex].SetActive(false);
            currentPopUpIndex = -1; // Disable current popup
            ResetKeyPressStates();
            Debug.Log("Ladder tutorial completed.");
        }
    }

    private void HandleCheckpointTutorial()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Disable the current popup
            popUps[currentPopUpIndex].SetActive(false);
            currentPopUpIndex = -1; // Disable current popup
            Debug.Log("Checkpoint tutorial completed.");
        }
    }

    private void HandleBatTutorial()
    {
        batTutorialTimer -= Time.deltaTime;
        if (batTutorialTimer <= 0f)
        {
            // Disable the current popup
            popUps[currentPopUpIndex].SetActive(false);
            currentPopUpIndex = -1; // Disable current popup
            Debug.Log("Bat tutorial completed.");
        }
    }

    private void HandleStalactiteTutorial()
    {
        stalactiteTutorialTimer -= Time.deltaTime;
        if (stalactiteTutorialTimer <= 0f)
        {
            // Disable the current popup
            popUps[currentPopUpIndex].SetActive(false);
            currentPopUpIndex = -1; // Disable current popup
            Debug.Log("Stalactite tutorial completed.");
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
