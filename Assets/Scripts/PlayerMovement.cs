using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Variables")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float climbSpeed;
    [SerializeField] protected LayerMask groundLayer; // LayerMask for ground detection
    [SerializeField] protected Vector3 groundCheckSize = new Vector3(0.9f, 0.1f, 0.9f); // Box size for ground check
    [SerializeField] protected float groundCheckDistance = 0.1f; // Distance below the player to check for ground


    protected Rigidbody body;
    protected Animator anim;
    protected PlayerControls controls;
    public bool grounded;

    protected float moveInput;

    // Climbing
    protected float vertical;     // Vertical input for climbing
    protected bool isLadder;      // Indicates if the player is near a ladder
    protected bool isClimbing;    // Indicates if the player is currently climbing
    private float upwardGravityScale = 1.5f;
    private float downwardGravityScale = 7f;

    public bool inAction = false;
    private bool isMounting = false;
    public GameObject mount;
    private int transitionTimer = 0;

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
        // Update grounded state using BoxCast
        GroundCheck();

        // Check if the player is climbing
        if (isClimbing)
        {
            OnClimb(vertical); // Calls climbing behavior when on a ladder
        }
        else
        {
            // If not climbing, apply horizontal movement
            if(moveInput != 0 && !grounded && !inAction)
            {
                OnMove(moveInput);
            }
            else if(grounded && !inAction)
            {
                OnMove(moveInput);
            }
            
            if (!grounded)
            {
                if (body.velocity.y > 0) // Ascending
                {
                    body.velocity += Vector3.up * Physics.gravity.y * (upwardGravityScale - 1) * Time.deltaTime;
                }
                else // Descending
                {
                    body.velocity += Vector3.up * Physics.gravity.y * (downwardGravityScale - 1) * Time.deltaTime;
                }
            }
        }

        // Check if player is mounted
        if (isMounting && mount != null)
        {
            transform.position = new Vector3(mount.transform.position.x, mount.transform.position.y + 1.6f, mount.transform.position.z);// + mountOffset;

            transitionTimer++;
            //insert screen darkening here or insert below into a function in Mount
            if (transitionTimer >= 50)
            {
                GameObject PairedCart = mount.GetComponent<Minecart>().PairedCart;

                transform.position = new Vector3(PairedCart.transform.position.x, PairedCart.transform.position.y + 1.6f, PairedCart.transform.position.z);
                OnMount();
            }
        }
    }

    private void GroundCheck()
    {
        // Perform a BoxCast downward to check for ground
        Vector3 boxCenter = transform.position - transform.up * groundCheckDistance;

         // Perform CheckBox
        grounded = Physics.CheckBox(
            boxCenter,
            groundCheckSize / 2,
            Quaternion.identity,
            groundLayer
        );

        // // Debugging
        // if (grounded)
        // {
        //     Debug.Log("Grounded: True");
        // }
        // else
        // {
        //     Debug.Log("Grounded: False");
        // }
        // Debug.Log($"Box Center: {boxCenter}, Size: {groundCheckSize}, Ground Layer: {groundLayer}");

    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = grounded ? Color.green : Color.yellow;

    //     Vector3 boxCenter = transform.position +Vector3.down * groundCheckDistance;
    //     Gizmos.DrawWireCube(boxCenter, groundCheckSize);

    // }

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
        if (grounded && body != null && !inAction)
        {
            body.velocity = new Vector3(body.velocity.x, jumpForce, body.velocity.z);
            grounded = false;
            Jumping();
        }
        else if (body == null)
        {
            Debug.LogError("Rigidbody not found on PlayerMovement script.");
        }
        else
        {
            Debug.LogError("currently is not grounded");
        }
    }

    protected virtual void Jumping()
    {
        //animation trigger
    }

    public void OnMount()
    {
        isMounting = !isMounting;

        if (isMounting)
        {
            body.velocity = Vector3.zero;
            inAction = true;
        }
        else
        {
            inAction = false;
            Debug.Log("Dismount");
        }
    }

    public void OnMine(bool mining)
    {
        if (mining)
        {
            body.velocity = Vector3.zero;
            inAction = true;
        }
        else
        {
            inAction = false;
        }
    }

    public void Freeze()
    {
        body.velocity = Vector3.zero;
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

    public virtual void ToggleLantern()
    {
    }

    public float JumpForce // Expose jumpForce as a public property
    {
        get { return jumpForce; }
        set { jumpForce = value; }
    }

    #endregion
}
