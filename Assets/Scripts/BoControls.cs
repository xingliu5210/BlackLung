using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;

public class BoControls : PlayerMovement
{
    [SerializeField] AK.Wwise.Event boBarkEvent;
    public bool amosback;
    public bool isMovementPaused = false;
    [SerializeField] private GameObject amos;
    [SerializeField] private float minFollowRange;
    [SerializeField] private float teleportRange;
    [SerializeField] private float barkRadius;
    float currentPos;
    float lastPos;
    float frameCount = 0;

    protected override void FixedUpdate()
    {
        if (isMovementPaused) return;
        
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
        PlayBarkSound();
        // Trigger the dog barking
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

    protected override void Jumping()
    {
        //Jumping animation
        anim.SetTrigger("jump");
    }


    protected override void Update()
    {
        base.Update();

        // // Move Animations
        // anim.SetFloat("moveSpd", Mathf.Abs(body.velocity.x));

        // // Landing animation
        // if (grounded == false) anim.SetBool("grounded", false);
        // else anim.SetBool("grounded", true);

        float effectiveVelocityX = Mathf.Abs(body.velocity.x);

        // If Bo is following and being teleported or moved directly, simulate animation speed
        if (amos.GetComponent<AmosControls>().boFollow && effectiveVelocityX == 0)
        {
            float distanceToAmos = Mathf.Abs(amos.transform.position.x - transform.position.x);
            if (distanceToAmos > minFollowRange)
            {
                anim.SetFloat("moveSpd", 6f); // matches "walk/run" in blend tree
            }
            else
            {
                anim.SetFloat("moveSpd", 0f);
            }
        }
        else
        {
            anim.SetFloat("moveSpd", effectiveVelocityX);
        }

        // Landing animation
        anim.SetBool("grounded", grounded);
    }

    private void PlayBarkSound()
    {
        AkSoundEngine.PostEvent(boBarkEvent.Id, this.gameObject);
    }
}
