using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalactite : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem ceilingDustParticles;
    [SerializeField] private float fallDelay;
    [SerializeField] private float forceMultiplier;
    [SerializeField] private float damage;
    [SerializeField] private Sensor warningSensor;
    [SerializeField] private Sensor fallSensor;
    [SerializeField] private Vector3 startPosition;
    private bool isActivated;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody>();
        if(warningSensor != null)
        {
            warningSensor.OnPlayerEnter += WarnPlayer;
        }
        fallSensor.OnPlayerEnter += DelayActivation;
    } 

    private void WarnPlayer(bool inRange)
    {
        Debug.Log("Stal: warning - " + inRange);
        
        if (inRange)
        {
            Invoke(nameof(PlayWarningAnimation), Random.Range(0f, 0.1f));

        } 
    }

    private void PlayWarningAnimation()
    {
        ceilingDustParticles.Play();
        animator.SetBool("InWarningRange", true);
    } 

    private void DelayActivation(bool inRange)
    {
        Debug.Log("Stal: delay");
        if (inRange)
        {
            Invoke(nameof(ActivateStalactite), fallDelay);
        }
    }

    private void ActivateStalactite()
    {
        //animator.SetTrigger("warningTrigger");
        //animator.SetBool("InWarningRange", true);
        isActivated = true;
        rb.useGravity = true;
        rb.AddForce(Vector3.down * forceMultiplier, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //check when stalactite touches the ground layer
        if (collision.gameObject.layer == 8)
        {
            if (isActivated)
            {
                Break();
            }
        }
        //if collide with player, damage player
        if (collision.gameObject.CompareTag("Player"))
        {
            HitPlayer(collision);
        }
    }

    private void HitPlayer(Collision collision)
    {
        collision.gameObject.GetComponent<PlayerHealth>().Damage(damage);
        Break();
    }

    private void Break()
    {
        // Insert animation of stalactite breaking
        gameObject.SetActive(false);
    }

    //Resets stalactite to starting state
    private void Reset()
    {
        isActivated = false;

        //Reset physics
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        //Reset to starting position
        transform.position = startPosition;
        transform.rotation = Quaternion.identity;
        gameObject.SetActive(true);
    }
}
