using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AmosControls : PlayerMovement
{
    [Header("Whip Variables")]
    [SerializeField] private WhipHookChecker whipHookChecker;
    [SerializeField] private float pullForce = 15;
    [SerializeField] private float whipAttackRadius;
    [SerializeField] private int whipDamage;

    // How long to display the rope on screen when pulling Amos
    [SerializeField] private float ropeVisibleTime = 0.5f;

    [Header("Object References")]
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Lantern lantern;

    // Used to track if the rope is currently visible
    private bool ropeVisible = false;

    // Used to countdown to remove the rope
    private float ropeVisibleCountdown = 0.5f;

    private float climbInput;

    /// <summary>
    /// Implentation for Amos' climb.
    /// </summary>
    /// <param name="input">Value of climb input.</param>
    public override void SetClimbInput(float input)
    {
        base.SetClimbInput(input);
        climbInput = input;

        // if climbing is enabled and input is non-zero, engable climbing
        if (isLadder && climbInput != 0)
        {
            isClimbing = true; // Start Climbing when the input is received
        }
        else
        {
            isClimbing = false; // Stop climbing is there's no input or not on a ladder.
        }
    }

    /// <summary>
    /// Implementatio for Amos' Whip ability.
    /// </summary>
    public override void BarkWhip()
    {
        base.BarkWhip();

        // Check if there is a hook in range.
        WhipHook targetHook = whipHookChecker.GetClosestHook(transform.position);
        if (targetHook != null)
        {
            // Draw line from player to hook using Linerenderer
            Vector3[] points = new Vector3[2];
            points[0] = transform.position;
            points[1] = targetHook.transform.position;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPositions(points);
            ropeVisible = true;
            //Debug.Log(points[0] + " " + points[1]);

            // Determine the direction to launch Amos
            Vector3 direction = Vector3.Normalize(targetHook.transform.position - transform.position);
            // Remove the Z part of the Vector so it doesn't move the player off the platform.
            direction = new Vector3(direction.x, direction.y, 0);

            // Addforce to launch player toward hook. Double force on y to combat gravity
            body.AddForce(new Vector2(direction.x * pullForce, direction.y * pullForce * 2));
        }
        else
        {
            //RaycastHit hit;
            //bool enemyhit = Physics.SphereCast(transform.position, whipAttackRadius, transform.forward, out hit, 0.1f);
            //Debug.Log("Whip Spherecast " + enemyhit);

            Collider[] results = Physics.OverlapSphere(transform.position, whipAttackRadius);

            if (results.Length > 0)
            {
                foreach (Collider r in results)
                {
                    if (r.gameObject.CompareTag("Enemy"))
                    {
                        Debug.Log("Enemy Hit: " + r.gameObject);
                        r.gameObject.GetComponent<CreatureFear>().TakeDamage(whipDamage);
                    }
                }
            }


        }
    }

    // /// <summary>
    // /// Debug for sphere and ray casts.
    // /// </summary>
    // private void OnDrawGizmos()
    // {
    //     // Debug for whip radius.
    //     Gizmos.DrawWireSphere(transform.position, whipAttackRadius);
    // }

    /// <summary>
    /// Calls the power toggle function on the lantern script, toggling it on or off.
    /// </summary>
    public override void ToggleLantern()
    {
        base.ToggleLantern();
        lantern.PowerToggle();
    }

    protected override void Update()
    {
        base.Update();

        // If the rope is visible, update player side. Once countdown is complete, clear the positions and reset counter.
        if(ropeVisible)
        {
            lineRenderer.SetPosition(0, transform.position);
            ropeVisibleCountdown -= Time.deltaTime;
            if(ropeVisibleCountdown <= 0)
            {
                lineRenderer.positionCount = 0;
                ropeVisible = false;
                ropeVisibleCountdown = ropeVisibleTime;
            }
        }
    }

    protected void OnDrawGizmos()
    {
        // Draw the box used for ground detection
        Gizmos.color = grounded ? Color.green : Color.red;

        Vector3 boxCenter = transform.position + Vector3.down * groundCheckDistance;
        Gizmos.DrawWireCube(boxCenter, groundCheckSize);

        // Debug for whip radius (specific to AmosControls)
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, whipAttackRadius);
    }

}



