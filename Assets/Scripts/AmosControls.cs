using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

public class AmosControls : PlayerMovement
{    
    [SerializeField] private GameObject bo;

    [Header("Whip Variables")]
    [SerializeField] private WhipHookChecker whipHookChecker;
    [SerializeField] private float pullForce = 15;
    public bool hooking = false;
    private int hookingCounter = 0;

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

    //Used to have Bo follow Amos
    public bool boFollow = false;
    public bool whistleLearned = false;
    public bool whistleRestricted = false;

    private Transform currentLadder;


    /// <summary>
    /// Implentation for Amos' climb.
    /// </summary>
    /// <param name="input">Value of climb input.</param>
    /// 
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        
        if (hooking == true)
        {
            hookingCounter++;
            GetComponent<FallDamage>().FallStartSet();

            if (hookingCounter > 10)
            {
                Debug.Log("COUNTER RESET");
                hooking = false;
                hookingCounter = 0;
            }
        }

        if (grounded && !isClimbing && !hooking)
        {
            bo.GetComponent<BoControls>().amosback = false;
        }
    }
    public override void SetClimbInput(float input)
    {

        base.SetClimbInput(input);
        climbInput = input;

        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        string animName = "";
        if (anim.GetCurrentAnimatorClipInfo(0).Length > 0){
            animName = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        }
        
        
        float normalizedTime = stateInfo.normalizedTime;
        Debug.Log(animName);

        if (animName != "Amos_Climb_01"){
            normalizedTime = 0;
        }

        // if climbing is enabled and input is non-zero, engable climbing
        if (isLadder && climbInput != 0 && !attachedToLadder)
        {

            Transform nearest = getNearestLoc();
            isClimbing = true; // Start Climbing when the input is received
            bo.GetComponent<BoControls>().amosback = true;
            anim.SetTrigger("climbing");
            anim.SetBool("Climbing", true);
            transform.SetPositionAndRotation(nearest.position + new Vector3(0, 0.25f, 0), transform.rotation);
            transform.Find("Amos_Rig").SetPositionAndRotation(transform.Find("Amos_Rig").position, currentLadder.rotation);
            if (climbInput > 0) anim.CrossFade("Climbing_Up", 0.1f, 0, normalizedTime);
            else anim.CrossFade("Climbing_Down", 0.1f, 0, normalizedTime);
            
            anim.speed = 1.5f;
            attachedToLadder = true;
        }
        else if (isLadder && climbInput != 0 && attachedToLadder){
            anim.speed = 1.5f;
            if (climbInput > 0) anim.CrossFade("Climbing_Up", 0.1f, 0, normalizedTime);
            else anim.CrossFade("Climbing_Down", 0.1f, 0, normalizedTime);
        }
        else if (isLadder && climbInput == 0 && attachedToLadder)
        {
            isClimbing = false; // Stop climbing is there's no input or not on a ladder.
            anim.speed = 0;
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

            // Animation for whiphook
            anim.SetTrigger("hook");

            hooking = true;
            bo.GetComponent<BoControls>().amosback = true;
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

    protected override void Jumping()
    {
        //Jumping animation
        anim.SetTrigger("jump");

        if (attachedToLadder){
            transform.Find("Amos_Rig").SetLocalPositionAndRotation(new Vector3(-0.04f, -0.83f, 0), Quaternion.Euler(0, 90, 0));
            anim.SetBool("Climbing", false);
            anim.speed = 1;
            attachedToLadder = false;
        }
    }

    protected override void Update()
    {
        base.Update();

        // Move Animations
        anim.SetFloat("moveSpd", Mathf.Abs(body.velocity.x));

        // Landing animation
        if (grounded == false) anim.SetBool("grounded", false);
        else anim.SetBool("grounded", true);

        // If the rope is visible, update player side. Once countdown is complete, clear the positions and reset counter.
        if (ropeVisible)
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

    public WhipHookChecker GetWhipHookChecker()
    {
        return whipHookChecker;
    }

    public override void OnWhistle()
    {
        base.OnWhistle();

        if (!whistleLearned || whistleRestricted) return;

        if (boFollow)
        {
            boFollow = false;
            Debug.Log("Bo, stay");
        }
        else
        {
            boFollow = true;
            Debug.Log("Amos calls Bo");
        }
    }

    public void StartFollow()
    {
        boFollow = true;
    }

    private Transform getNearestLoc(){
        if (currentLadder != null){

            float minDist = Mathf.Infinity;
            Vector3 currentPos = transform.position;

            Transform nearest = null;

            foreach (Transform child in currentLadder)
            {
                float dist = Vector3.Distance(child.transform.position, currentPos);

                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = child;
                }
            }

            return nearest;
        }

        return null;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Ladder"))
        {
            currentLadder = collider.transform;
            isLadder = true; // Enable ladder climbing
            Debug.Log("Entered ladder trigger. isLadder set to true.");
        }

        if (collider.CompareTag("noWhistle"))
        {
            whistleRestricted = true;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Ladder"))
        {
            isLadder = false;   // Disable climbing when exiting the ladder
            isClimbing = false;
            body.useGravity = true; // Re-enable gravity when leaving the ladder
            Debug.Log("Exited ladder trigger. isLadder set to false.");
        }

        if (collider.CompareTag("noWhistle"))
        {
            whistleRestricted = false;
        }
    }
    private void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("noWhistle"))
        {
            boFollow = false;
        }
    }
}



