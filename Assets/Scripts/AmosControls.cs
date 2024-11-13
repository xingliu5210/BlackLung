using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmosControls : PlayerMovement
{
    private float climbInput;
    [SerializeField] private WhipHookChecker whipHookChecker;
    [SerializeField] private float pullForce = 15;

    [SerializeField] private LineRenderer lineRenderer;

    // How long to display the rope on screen when pulling Amos
    [SerializeField] private float ropeVisibleTime = 0.5f;

    // Used to track if the rope is currently visible
    private bool ropeVisible = false;

    // Used to countdown to remove the rope
    private float ropeVisibleCountdown = 0.5f;

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
            lineRenderer.SetPositions(points);
            ropeVisible = true;
            //Debug.Log(points[0] + " " + points[1]);

            // Determine the direction to launch Amos
            Vector3 direction = Vector3.Normalize(targetHook.transform.position - transform.position);
            // Remove the Z part of the Vector so it doesn't move the player off the platform.
            direction = new Vector3(direction.x, direction.y, 0);
            // Addforce to launch player toward hook
            body.AddForce(direction * pullForce);
        }
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

}



