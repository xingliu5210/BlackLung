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
        controls = new PlayerControls();

         // Ensure Rigidbody is found
        if (body == null)
        {
            Debug.LogError("Rigidbody component is missing on the Player GameObject.");
        }

        // Bind the jump and move actions to their respective methods
        controls.Player.Jump.started += ctx => OnJump();
        controls.Player.Move.performed += ctx => OnMove(ctx.ReadValue<float>());
        controls.Player.Move.canceled += ctx => OnMove(0f);
        
    }

    private void OnMove(float direction)
    {
        moveInput = direction;
        Debug.Log("Move Input: " + moveInput);

        // Set horizontal velocity based on move input
        body.velocity = new Vector3(moveInput * speed, body.velocity.y, body.velocity.z);

        // Flip player direction based on movement input
        if (moveInput > 0.01f)
        {
            transform.localScale = Vector3.one;
        }
        else if (moveInput < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void OnJump()
    {
            if (grounded && body != null)
        {
            Jump();
        }
        else if (body == null)
        {
            Debug.LogError("Rigidbody not found on PlayerMovement script.");
        }
    }
        public void Jump()
    {
        body.velocity = new Vector3(body.velocity.x, jumpForce, body.velocity.z);
        grounded = false;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
        }
    }


}
