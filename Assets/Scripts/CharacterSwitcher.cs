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

    // References for the two character objects
    [SerializeField] private PlayerMovement amos;
    [SerializeField] private PlayerMovement bo;

    // Used to set who is currently receiving the inputs.
    private PlayerMovement controlledCharacter;

    // Start is called before the first frame update
    private void Awake()
    {
        playerControls = new PlayerControls();
        // Call bind method on start.        
        Debug.Log("Hello");
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

        Debug.Log("Bind Called");
        playerControls.Player.Swap.started += ctx => { SwapCharacters();  };

        // Separate bindings for the movement starting and ending.
        playerControls.Player.Move.started += ctx => { MovementInputStarted(ctx); };
        playerControls.Player.Move.canceled += ctx => { MovementInputEnded(); };
        
        playerControls.Player.BarkWhip.started += ctx => { Debug.Log("Bark Whip"); };
        playerControls.Player.Flare.started += ctx => { Debug.Log("Flare"); };
        playerControls.Player.Lantern.started += ctx => { Debug.Log("Lantern"); };
        playerControls.Player.Interact.started += ctx => { Debug.Log("Interact"); };
    }

    private void OnEnable()
    {
        Debug.Log("Enable");
        playerControls = new PlayerControls();
        playerControls.Player.Enable();
    }

    private void OnDisable()
    {
        Debug.Log("Disable");
        playerControls.Player.Disable();
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
        // Value from the input
        float inputValue = ctx.ReadValue<float>();
        // Pass Input context object to character.
        Debug.Log("Move input started.");
    }

    /// <summary>
    /// Ends movement for currently controlled character.
    /// </summary>
    private void MovementInputEnded()
    {
        // Stop movement for character
        Debug.Log("Move input ended.");
    }

    /// <summary>
    /// Calls the jump method for the currently controlled character.
    /// </summary>
    private void JumpEvent()
    {
        //controlledCharacter.Jump();
        Debug.Log("Jump.");
    }

    #endregion

    // Update is called once per frame
    void Update()
    {
        
    }
}
