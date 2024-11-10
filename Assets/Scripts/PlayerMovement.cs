using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    private Rigidbody body;
    private Animator anim;
    private PlayerControls controls;
    private bool grounded;

    private float moveInput;

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

    private void Update()
    {
        // Continuously call OnMove with the current value of moveInput
        OnMove(moveInput);
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
        Debug.Log("Move Input: " + direction);

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
        }
    }
    
}
