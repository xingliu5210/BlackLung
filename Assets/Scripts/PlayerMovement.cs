using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.Playables;
using UnityEngine.Animations;

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
    //protected 
    public bool isClimbing;    // Indicates if the player is currently climbing
    public bool attachedToLadder = false;
    public bool isResting = true;
    private float upwardGravityScale = 1.5f;
    private float downwardGravityScale = 7f;
    

    public bool inAction = false;
    private bool isMounting = false;
    public GameObject mount;
    private int transitionTimer = 0;

    protected virtual void Awake()
    {
        body = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

         // Ensure Rigidbody is found
        if (body == null)
        {
            Debug.LogError("Rigidbody component is missing on the Player GameObject.");
        }
    }

    private void Start()
    {
        if (SaveSystem.LoadGame(transform, GameObject.FindGameObjectWithTag("Dog")?.transform))
        {
            Debug.Log("Game Loaded at startup.");
        }
    }

    protected virtual void FixedUpdate()
    { 
    }

    protected virtual void Update()
    {
        // Update grounded state using BoxCast
        GroundCheck();

        // Check if the player is climbing
        if (isClimbing && attachedToLadder)
        {
            OnClimb(vertical); // Calls climbing behavior when on a ladder
        }
        else if (!isClimbing && attachedToLadder)
        {
            if (body.velocity.magnitude != 0)
                {
                    body.velocity = new Vector3(0,0,0);
                }
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
            
            if (!grounded && !attachedToLadder)
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

        var v = new Vector3();
        if (attachedToLadder == false) v = new Vector3(moveInput * speed, body.velocity.y, body.velocity.z);
        else v = new Vector3(0, 0, 0);

        var currVel = body.velocity;

        body.velocity = Vector3.SmoothDamp(body.velocity, v, ref currVel, 0.05f); ;

        // Flip player direction based on movement input
        if (direction > 0.01f && !attachedToLadder)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (direction < -0.01f && !attachedToLadder)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

    }

    /// <summary>
    /// Function for climbing behavior
    /// </summary>
    private void OnClimb(float verticalInput)
    {
        body.velocity = transform.Find("Amos_Rig").rotation * new Vector3(0, 0.592f * anim.speed * verticalInput * 1.45f, 0);
        body.useGravity = false; // Disable gravity while climbing
    }

    /// <summary>
    /// Function for player jumping.
    /// </summary>
    public void OnJump()
    {
        // trigger jump if character is grounded. Removed redundant Jump method.
        if ((grounded || attachedToLadder) && body != null && !inAction)
        {
            Jumping();
            body.velocity = new Vector3(body.velocity.x, jumpForce, body.velocity.z);
            grounded = false;
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

    #region Character Specific Functions

    // Method to set climb input, called by CharacterSwitcher or derived class
    public virtual void SetClimbInput(float input)
    {
        vertical = input;
        
        // Start climbing if on a ladder and receiving vertical input
        isClimbing = attachedToLadder && vertical != 0;
    }

    public virtual void BarkWhip()
    {       
    }

    public virtual void ToggleLantern()
    {
    }

    public virtual void OnWhistle()
    {
    }

    public float JumpForce // Expose jumpForce as a public property
    {
        get { return jumpForce; }
        set { jumpForce = value; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            Debug.Log("Entered ladder.");
            isLadder = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            Debug.Log("Exited ladder.");
            isLadder = false;
            isClimbing = false;
            body.useGravity = true; // Re-enable gravity
        }
    }

    public float GetVerticalInput()
    {
        return vertical;
    }

    #endregion
}
