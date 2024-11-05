using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSwitcher : MonoBehaviour
{
    // Reference to input action mapping object.
    private PlayerControls playerControls;

    // References for the two character objects
    [SerializeField] private PlayerMovement amos;
    [SerializeField] private PlayerMovement bo;

    // Used to set who is currently receiving the inputs.
    private PlayerMovement controlledCharacter;

    // Start is called before the first frame update
    void Awake()
    {
        // Call bind method on start.
        BindControlEvents();

        // Set amos as the currently controlled character.
        controlledCharacter = amos;
    }

    #region Input events and binding method

    /// <summary>
    /// Method for setting up all input event bindings.
    /// </summary>
    private void BindControlEvents()
    {
        playerControls = new PlayerControls();

        playerControls.Player.Swap.started += ctx => { SwapCharacters();  };

        // Separate bindings for the movement starting and ending.
        playerControls.Player.Move.started += ctx => { MovementInputStarted(ctx); };
        playerControls.Player.Move.canceled += ctx => { MovementInputEnded(); };

        playerControls.Player.Jump.started += ctx => { JumpEvent(); };
    }

    /// <summary>
    /// Swaps the currently controlled character to the character currently not being controlled.
    /// </summary>
    private void SwapCharacters()
    {
        // If the controlledCharacter is Amos, assign bo as the controlledCharacter, otherwise assign Amos.
        controlledCharacter = controlledCharacter == amos ? bo : amos;
    }

    /// <summary>
    /// Sets movement for currently controlled character, passing the input value.
    /// </summary>
    /// <param name="ctx">Input context object.</param>
    private void MovementInputStarted(InputAction.CallbackContext ctx)
    {
        // Value from the input
        float inputValue = ctx.ReadValue<float>();
        // Pass Input context object to character.
    }

    /// <summary>
    /// Ends movement for currently controlled character.
    /// </summary>
    private void MovementInputEnded()
    {
        // Stop movement for character
    }

    /// <summary>
    /// Calls the jump method for the currently controlled character.
    /// </summary>
    private void JumpEvent()
    {
        controlledCharacter.Jump();
    }

    #endregion

    // Update is called once per frame
    void Update()
    {
        
    }
}
