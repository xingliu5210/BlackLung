using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalactite : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float fallDelay;
    [SerializeField] private float forceMultiplier;
    [SerializeField] private float damage;
    [SerializeField] private Sensor sensor;
    [SerializeField] private Vector3 startPosition;
    private bool isActivated;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody>();
        sensor.OnPlayerEnter += WarnPlayer;
    }

    private void WarnPlayer(bool inRange)
    {
        animator.SetBool("InWarningRange", inRange);
        if(inRange)
        {
            Invoke(nameof(ActivateStalactite), fallDelay);
        }
    }

    private void ActivateStalactite()
    {
        AudioManager.Instance.PlayStalactitesFall();
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
