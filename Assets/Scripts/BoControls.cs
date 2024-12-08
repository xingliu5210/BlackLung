using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoControls : PlayerMovement
{
    /// <summary>
    /// Implements Bo's bark function.
    /// </summary>
    public override void BarkWhip()
    {
        base.BarkWhip();
    }

    /// <summary>
    /// Visualizes the ground detection box and other debug information specific to Bo.
    /// </summary>
    protected void OnDrawGizmos()
    {
        // Draw the box used for ground detection
        Gizmos.color = grounded ? Color.green : Color.red;

        Vector3 boxCenter = transform.position - transform.up * groundCheckDistance;
        Gizmos.DrawWireCube(boxCenter, groundCheckSize);
    }

    protected override void Update()
    {
        base.Update();
    }
}
