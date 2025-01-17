using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoControls : PlayerMovement
{
    public bool amosback;
    [SerializeField] private GameObject amos;
    [SerializeField] private float minFollowRange;
    [SerializeField] private float teleportRange;
    [SerializeField] private float barkRadius;
    float currentPos;
    float lastPos;
    float frameCount = 0;

    [Header("Audio")]
    [SerializeField] private new FootstepAudioManager footstepAudioManager;

    // Used to track the time between footstep sounds
    private float footstepTimer = 0f;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        lastPos = currentPos;
        currentPos = transform.position.x;

        /*if(control bo)
        {
            boFollow = false;
        }
        */

        // Bo follows Amos when Amos whistles
        float distance = Mathf.Abs(amos.transform.position.x - transform.position.x);

        if (amos.GetComponent<AmosControls>().boFollow && amosback)
        {
            //GetComponent<Collider>().enabled = false;
            //body.isKinematic = false;
            //grounded = true;
        }
        else
        {
            //GetComponent<Collider>().enabled = true;
           // body.isKinematic = true;
        }

        if (amos.GetComponent<AmosControls>().boFollow && (amosback || distance > teleportRange))
        {
            transform.position = amos.transform.position;
            body.velocity = Vector3.zero;
        }
        else if (amos.GetComponent<AmosControls>().boFollow && distance > minFollowRange )
        {
            float direction = 1;

            // Flip player direction based on movement input
            if (amos.transform.position.x > transform.position.x)
            {
                transform.localScale = Vector3.one;
                direction = 1;
            }
            else if (amos.transform.position.x < transform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                direction = -1;
            }

            body.velocity = new Vector3(direction * 10, body.velocity.y, body.velocity.z);


            if (lastPos == currentPos)
            { 
                frameCount ++;
                if (frameCount < 2) return;

                base.OnJump();
                Debug.Log("Jumping");
            }
            else
            {
                frameCount = 0;
            }        
        }
        else if(amos.GetComponent<AmosControls>().boFollow!)
        { frameCount = 0; }
    }

    public void BackpackRide()
    {

    }

    public override void BarkWhip()
    {
        base.BarkWhip();
        Debug.Log("BARK ");
        //onBark.Invoke();
        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, barkRadius);
        if (objectsInRange.Length > 0)
        {
            foreach (Collider obj in objectsInRange)
            {
                if (obj.gameObject.CompareTag("Enemy"))
                {
                    obj.GetComponent<Bat>().FearedFlee(transform.position);
                }
            }
        }


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

        // Trigger footstep sounds 
        if (grounded && Mathf.Abs(body.velocity.x) > 0.1f)
        {
            // If the timer is at zero, immediately play a footstep sound
            if (footstepTimer == 0f)
            {
                footstepAudioManager.PlayBoFootstep();
            }

            // Increment the timer
            footstepTimer += Time.deltaTime;

            // Play footstep at regular intervals
            if (footstepTimer >= footstepAudioManager.footstepInterval)
            {
                footstepAudioManager.PlayBoFootstep();
                footstepTimer = 0f; // Reset timer
            }
        }
        else
        {
            // Reset timer when not moving or grounded
            footstepTimer = 0f;
        }
    }
}
