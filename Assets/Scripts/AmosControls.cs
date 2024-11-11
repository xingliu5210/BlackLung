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
    }
}
