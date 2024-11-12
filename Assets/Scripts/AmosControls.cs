using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmosControls : PlayerMovement
{
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
}
