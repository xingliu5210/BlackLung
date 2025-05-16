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
    private bool snapToLadder = false;
    private bool movingDown = false;
    private bool isTouchingGround = false;

    // Used to countdown to remove the rope
    private float ropeVisibleCountdown = 0.5f;

    private float climbInput;

    //Used to have Bo follow Amos
    public bool boFollow = false;
    public bool whistleLearned = false;
    public bool whistleRestricted = false;

    //Used for elevator
    public bool isOnElevator = false;

    // Rig Offsets
    private Vector3 offsetTest = new Vector3(-0.2f,-0.4f,0);
    private float footOffset = .16f;
    private float handOffet = -.16f;

    private Transform currentLadder;
    private Vector3 amosCenter = Vector3.zero;

    protected override void Awake()
    {
        base.Awake();
        Transform amos = transform.Find("Amos_Rig");

        SkinnedMeshRenderer[] renderers = amos.GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach (var smr in renderers)
        {
            amosCenter += smr.bounds.center;
        }

        amosCenter = amos.InverseTransformPoint(amosCenter / renderers.Length);

    }

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

        if (grounded && !attachedToLadder && !hooking)
        {
            bo.GetComponent<BoControls>().amosback = false;
        }
    }
    public override void SetClimbInput(float input)
    {
        if (isResting){
            base.SetClimbInput(input);
            climbInput = input;
        }
        else{
            base.SetClimbInput(0);
            climbInput = 0;
        }
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        string animName = "";
        if (anim.GetCurrentAnimatorClipInfo(0).Length > 0){
            animName = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        }
        
        float normalizedTime = stateInfo.normalizedTime;

        if (animName != "Amos_Climb_01"){
            normalizedTime = 0;
        }

        // if climbing is enabled and input is non-zero, engable climbing
        if (isLadder && climbInput != 0 && !attachedToLadder)
        {

            isClimbing = true; // Start Climbing when the input is received
            bo.GetComponent<BoControls>().amosback = true;
            anim.SetTrigger("climbing");
            anim.SetBool("Climbing", true);
            
            snapToLadder = true;

            if (climbInput > 0){
                anim.CrossFade("Climbing Up", 0.1f, 0, normalizedTime - (int)normalizedTime);
                movingDown = false;
            }
            else {
                anim.CrossFade("Climbing Down", 0.1f, 0, normalizedTime - (int)normalizedTime);
                movingDown = true;
            }
            
            anim.speed = 1.5f;
            attachedToLadder = true;
        }
        else if (isLadder && climbInput != 0 && attachedToLadder){
            anim.speed = 1.5f;
            if (climbInput > 0){
                anim.CrossFade("Climbing Up", 0.1f, 0, normalizedTime - (int)normalizedTime);
                movingDown = false;
            }
            else {
                anim.CrossFade("Climbing Down", 0.1f, 0, normalizedTime - (int)normalizedTime);
                movingDown = true;
            }
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
            transform.Find("Amos_Rig").SetLocalPositionAndRotation(new Vector3(-0.04f, -1, 0), Quaternion.Euler(0, 90, 0));
            anim.SetBool("Climbing", false);
            anim.speed = 1;
            attachedToLadder = false;
            body.useGravity = true;
            isResting = true;
        }
    }

    protected override void Update()
    {
        base.Update();

        // Move Animations
        anim.SetFloat("moveSpd", Mathf.Abs(body.velocity.x));
        

        if (snapToLadder){
            moveToNearestLoc(1);
        }
        
        if (attachedToLadder){

            if (climbInput == 0){
                var normalizedTime = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
                normalizedTime -= (int)normalizedTime;
                var tolerance = 0.01f;
                if (Mathf.Abs(normalizedTime - 0.5f) > tolerance && Mathf.Abs(normalizedTime - 1) > tolerance){
                    anim.speed = 1.5f;
                    if (movingDown && !isTouchingGround) base.SetClimbInput(-1);
                    else if (!movingDown && !isTouchingGround) base.SetClimbInput(1);
                    else base.SetClimbInput(0);
                    isResting = false;
                }
                else if (Mathf.Abs(normalizedTime - 0.5f) < tolerance || Mathf.Abs(normalizedTime - 1) < tolerance){
                    anim.speed = 0;
                    base.SetClimbInput(0);
                    isResting = true;
                }
            }
        }

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

            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

            foreach (Transform child in currentLadder)
            {
                float dist = Vector3.Distance(child.transform.position, currentPos);

                var condition = false;
                //Output the name of the starting clip
                if (stateInfo.IsName("Climbing Down")){
                    condition = child.transform.position.y < currentPos.y;
                }
                else{
                    condition = true;
                }

                if (condition){
                    if (dist < minDist)
                    {
                        minDist = dist;
                        nearest = child;
                    }

                }
            }

            return nearest;
        }

        return null;
    }

    private void moveToNearestLoc( float speed){

        Transform nearest = getNearestLoc();
        Transform amos = transform.Find("Amos_Rig");
        var offset = new Vector3(0, 0.5f, 0);
        var newPosition = nearest.position;
            
        transform.position = Vector3.Lerp(transform.position, newPosition, speed);

        amos.position = newPosition - currentLadder.rotation * (amosCenter + offsetTest);
        amos.rotation = currentLadder.rotation;

        if (Vector3.Distance(transform.position, newPosition) < 0.001f){
            snapToLadder = false;
        }
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
            // Re-enable gravity when leaving the ladder
            Debug.Log("Exited ladder trigger. isLadder set to false.");
        }

        if (collider.CompareTag("noWhistle"))
        {
            whistleRestricted = false;
        }
    }
    private void OnTriggerStay(Collider collider)
    {
        if (collider.CompareTag("noWhistle"))
        {
            boFollow = false;
        }
    }

    private void OnCollisionEnter(Collision collider){

        if (collider.transform.tag == "Ground"){
            if (attachedToLadder){
                var dis = Vector3.Normalize(collider.transform.position - transform.position).y;
                
                if ((dis > 0 && climbInput > 0) || (dis < 0 && climbInput < 0)){
                    anim.speed = 0;
                    base.SetClimbInput(0);
                    body.velocity = new Vector3(0, 0, 0);
                }
                else{
                    isTouchingGround = true;
                }
            }

        }
    }

    private void OnCollisionStay(Collision collider){

        if (collider.transform.tag == "Ground"){
            if (attachedToLadder){
                var dis = Vector3.Normalize(collider.transform.position - transform.position).y;

                if ((dis > 0 && climbInput > 0) || (dis < 0 && climbInput < 0)){
                    anim.speed = 0;
                    base.SetClimbInput(0);
                    body.velocity = new Vector3(0, 0, 0);
                }
                else{
                    isTouchingGround = true;
                }
            }

        }
    }

    private void OnCollisionExit(Collision collider){

        if (collider.transform.tag == "Ground"){
            if (attachedToLadder){
                isTouchingGround = false;
            }

        }

    }

    private void OnAnimatorIK(int layerIndex){


        if (anim) {

            // Foot IK Snapping
            anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, anim.GetFloat("LeftFootIKWeight"));
            anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, anim.GetFloat("LeftFootIKWeight"));

            anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, anim.GetFloat("RightFootIKWeight"));
            anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, anim.GetFloat("RightFootIKWeight"));

            RaycastHit hit;
            LayerMask mask = LayerMask.GetMask("Ground", "Ladder");
            Ray rayLeft = new Ray(anim.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.down, transform.Find("Amos_Rig").rotation * Vector3.down);

            Ray rayRight = new Ray(anim.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.down, transform.Find("Amos_Rig").rotation * Vector3.down);

            if (Physics.Raycast(rayLeft, out hit, groundCheckDistance + 2f, mask)){
                if ((hit.transform.tag == "Ground" && !attachedToLadder) || (hit.transform.tag == "Locator" && attachedToLadder)){
                    
                    Vector3 footPosition = hit.point;
                    footPosition += (new Vector3(0, groundCheckDistance, 0));
                    anim.SetIKPosition(AvatarIKGoal.LeftFoot, footPosition + new Vector3(0, footOffset, 0));
                    anim.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(transform.forward, hit.normal));
                }

            }

            if (Physics.Raycast(rayRight, out hit, groundCheckDistance + 2f, mask)){
                if ((hit.transform.tag == "Ground" && !attachedToLadder) || (hit.transform.tag == "Locator" && attachedToLadder)){
                    
                    Vector3 footPosition = hit.point;
                    footPosition += (new Vector3(0, groundCheckDistance, 0));
                    anim.SetIKPosition(AvatarIKGoal.RightFoot, footPosition + new Vector3(0, footOffset, 0));
                    anim.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(transform.forward, hit.normal));

                }

            }


            // Hand IK Snapping

            anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, anim.GetFloat("LeftHandIKWeight"));
            anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, anim.GetFloat("LeftHandIKWeight"));

            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, anim.GetFloat("RightHandIKWeight"));
            anim.SetIKRotationWeight(AvatarIKGoal.RightHand, anim.GetFloat("RightHandIKWeight"));

            rayLeft = new Ray(anim.GetIKPosition(AvatarIKGoal.LeftHand) + Vector3.down, transform.Find("Amos_Rig").rotation * Vector3.up);

            rayRight = new Ray(anim.GetIKPosition(AvatarIKGoal.RightHand) + Vector3.down, transform.Find("Amos_Rig").rotation * Vector3.up);

            if (Physics.Raycast(rayLeft, out hit, .98f + 2f, mask))
            {
                if ((hit.transform.tag == "Locator" && attachedToLadder))
                {
                    Vector3 handPosition = hit.point;
                    handPosition += (new Vector3(0, .98f, 0));
                    anim.SetIKPosition(AvatarIKGoal.LeftHand, handPosition + new Vector3(0, handOffet, 0));
                }

            }

            if (Physics.Raycast(rayRight, out hit, .98f + 2f, mask))
            {
                if ((hit.transform.tag == "Locator" && attachedToLadder))
                {
                    Vector3 handPosition = hit.point;
                    handPosition += (new Vector3(0, .98f, 0));
                    anim.SetIKPosition(AvatarIKGoal.RightHand, handPosition + new Vector3(0, handOffet, 0));

                }

            }

        }

    }

}



