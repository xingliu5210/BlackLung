using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.PlayerSettings;

public class CharacterSwitcher : MonoBehaviour
{
    // Reference to input action mapping object.
    private PlayerControls playerControls;

    // Used to set who is currently receiving the inputs.
    private PlayerMovement controlledCharacter;

    // Used to store value from movement input.
    private float movementValue;

    // References for the two character objects
    [Tooltip("Reference to Amos' movement script")]
    [SerializeField] private PlayerMovement amos;

    [Tooltip("Reference to Bo's movement script.")]
    [SerializeField] private PlayerMovement bo;

    // Start is called before the first frame update
    private void Awake()
    {
        playerControls = new PlayerControls();

        // Set amos as the currently controlled character.
        controlledCharacter = amos;
    }

    private void Start()
    {
        BindControlEvents();
    }

    #region Input events and binding method

    /// <summary>
    /// Method for setting up all input event bindings.
    /// </summary>
    private void BindControlEvents()
    {

        playerControls.Player.Jump.started += ctx => JumpEvent();

        //Debug.Log("Bind Called");
        playerControls.Player.Swap.performed += ctx => { SwapCharacters();  };

        // Separate bindings for the movement starting and ending.
        playerControls.Player.Move.performed += ctx => MovementInputStarted(ctx);
        playerControls.Player.Move.canceled += ctx => MovementInputEnded();

        playerControls.Player.Climb.performed += ctx => { ClimbStarted(ctx); };
        playerControls.Player.Climb.canceled += ctx => { ClimbEnded(); } ;

        playerControls.Player.Lantern.started += ctx => { LanternToggleInput(); };

        playerControls.Player.BarkWhip.started += ctx => { BarkWhipInput(); };

        playerControls.Player.Flare.started += ctx => { Debug.Log("Flare"); };
        playerControls.Player.Interact.started += ctx => { Debug.Log("Interact"); };

        // Bind interaction logic
        playerControls.Player.Interact.started += ctx => InteractEvent();
    }

    /// <summary>
    /// Routes input for the Lantern input to the character script.
    /// </summary>
    private void LanternToggleInput()
    {
        controlledCharacter.ToggleLantern();
    }

    /// <summary>
    /// Routes input for the Lantern input to the character script.
    /// </summary>
    private void BarkWhipInput()
    {
        controlledCharacter.BarkWhip();
    }

    private void OnEnable()
    {
        Debug.Log("Enable");
        playerControls = new PlayerControls();
        playerControls.Player.Enable();
    }

    private void OnDisable()
    {
       //Debug.Log("Disable");
        playerControls.Player.Disable();
    }

    /// <summary>
    /// Sets climb input for controlled character, passing the input value.
    /// </summary>
    /// <param name="ctx">Input context object.</param>
    private void ClimbStarted(InputAction.CallbackContext ctx)
    {
        controlledCharacter.SetClimbInput(ctx.ReadValue<float>());
        Debug.Log(ctx.ReadValue<float>());
    }

    /// <summary>
    /// End of climb input.
    /// </summary>
    private void ClimbEnded()
    {
        controlledCharacter.SetClimbInput(0);
    }


    /// <summary>
    /// Swaps the currently controlled character to the character currently not being controlled.
    /// </summary>
    private void SwapCharacters()
    {
        // If the controlledCharacter is Amos, assign bo as the controlledCharacter, otherwise assign Amos.
        controlledCharacter = controlledCharacter == amos ? bo : amos;
        Debug.Log("Character swap.");
    }

    /// <summary>
    /// Sets movement for currently controlled character, passing the input value.
    /// </summary>
    /// <param name="ctx">Input context object.</param>
    private void MovementInputStarted(InputAction.CallbackContext ctx)
    {
        //Debug.Log(ctx.ReadValue<Vector2>());
        // Value from the input
        movementValue = ctx.ReadValue<Vector2>().x;

        // Setting input to a value of 1 or -1 as Gamepad stick values 'ramp up' to 1 (ex. 0.31 instead of 1)
        /*movementValue = movementValue > 0 ? 1 : -1;
        movementValue = movementValue > 0 ? 1 : movementValue < 0 ? -1 : 0;
        controlledCharacter.OnMove(movementValue);
        Debug.Log("MovementInputStarted called. Movement input value: " + movementValue);
        */
        controlledCharacter.SetMoveInput(movementValue);
        //Debug.Log("Movement started with value: " + movementValue);
    }

    /// <summary>
    /// Ends movement for the currently controlled character.
    /// </summary>
    private void MovementInputEnded()
    {
        controlledCharacter.SetMoveInput(0f); // Stop movement by setting input to zero
        //Debug.Log("Movement input ended.");
    }


    /// <summary>
    /// Calls the jump method for the currently controlled character.
    /// </summary>
    private void JumpEvent()
    {
        controlledCharacter.OnJump();
        //Debug.Log("Jump.");
    }

    /// <summary>
    /// Calls the Interact method for the currently controlled character.
    /// </summary>
    private void InteractEvent()
    {
        if (controlledCharacter != null)
        {
            // Get the Interaction component on the controlled character and call Interact
            Interaction interaction = controlledCharacter.GetComponent<Interaction>();
            if (interaction != null)
            {
                interaction.Interact();
            }
            else
            {
                Debug.LogWarning("No Interaction component found on controlled character.");
            }
        }
    }

    #endregion

    // Update is called once per frame
    void Update()
    {
        
    }
}
