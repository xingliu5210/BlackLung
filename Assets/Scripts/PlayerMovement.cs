using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float climbSpeed;
    protected Rigidbody body;
    protected Animator anim;
    protected PlayerControls controls;
    protected bool grounded;

    protected float moveInput;

    // Climbing
    protected float vertical;     // Vertical input for climbing
    protected bool isLadder;      // Indicates if the player is near a ladder
    protected bool isClimbing;    // Indicates if the player is currently climbing
    private float upwardGravityScale = 1.5f;
    private float downwardGravityScale = 7f;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

         // Ensure Rigidbody is found
        if (body == null)
        {
            Debug.LogError("Rigidbody component is missing on the Player GameObject.");
        }        
    }

    protected virtual void Update()
    {
        // Check if the player is climbing
        if (isClimbing)
        {
            OnClimb(vertical); // Calls climbing behavior when on a ladder
        }
        else
        {
            // If not climbing, apply horizontal movement
            if(moveInput != 0)
            {
                OnMove(moveInput);
            }
            

            if (!grounded)
            {
                if (body.velocity.y > 0) // Ascending
                {
                    body.velocity += Vector3.up * Physics.gravity.y * (upwardGravityScale - 1) * Time.deltaTime;
                }
                else if (body.velocity.y < 0) // Descending
                {
                    body.velocity += Vector3.up * Physics.gravity.y * (downwardGravityScale - 1) * Time.deltaTime;
                }
            }
        }

    }

    // Method to set moveInput, called by CharacterSwitcher
    public void SetMoveInput(float direction)
    {
        moveInput = direction;
    }


    /// <summary>
    /// Called when the player moves
    /// </summary>
    /// <param name="direction"></param>
    public void OnMove(float direction)
    {
        // moveInput = direction;
        //Debug.Log("Move Input: " + direction);

        // Set horizontal velocity based on move input
        body.velocity = new Vector3(moveInput * speed, body.velocity.y, body.velocity.z);

        // Flip player direction based on movement input
        if (direction > 0.01f)
        {
            transform.localScale = Vector3.one;
        }
        else if (direction < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    /// <summary>
    /// Function for climbing behavior
    /// </summary>
    private void OnClimb(float verticalInput)
    {
        Debug.Log("Climbing with input: " + verticalInput);
        // Move the player vertically along the ladder
        body.velocity = new Vector3(body.velocity.x, verticalInput * climbSpeed, body.velocity.z);
        body.useGravity = false; // Disable gravity while climbing
    }

    /// <summary>
    /// Function for player jumping.
    /// </summary>
    public void OnJump()
    {
        // trigger jump if character is grounded. Removed redundant Jump method.
        if (grounded && body != null)
        {
            body.velocity = new Vector3(body.velocity.x, jumpForce, body.velocity.z);
            grounded = false;
        }
        else if (body == null)
        {
            Debug.LogError("Rigidbody not found on PlayerMovement script.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
            isClimbing = false; // Stop climbing when grounded
            body.useGravity = true; // Re-enable gravity
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Ladder"))
        {
            isLadder = true; // Enable ladder climbing
            Debug.Log("Entered ladder trigger. isLadder set to true.");
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Ladder"))
        {
            isLadder = false;   // Disable climbing when exiting the ladder
            isClimbing = false;
            body.useGravity = true; // Re-enable gravity when leaving the ladder
            Debug.Log("Exited ladder trigger. isLadder set to false.");
        }
    }

    #region Character Specific Functions

    // Method to set climb input, called by CharacterSwitcher or derived class
    public virtual void SetClimbInput(float input)
    {
        vertical = input;
        
        // Start climbing if on a ladder and receiving vertical input
        isClimbing = isLadder && vertical != 0;

        Debug.Log("SetClimbInput called. vertical: " + vertical + ", isLadder: " + isLadder + ", isClimbing: " + isClimbing);
    }

    public virtual void BarkWhip()
    {       
    }

    #endregion
}
