using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalactite : MonoBehaviour
{
    [SerializeField] private bool resetBool;
    [SerializeField] private float damage;
    [SerializeField] private Sensor sensor;
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private bool isFalling;
    [SerializeField] private bool isGrounded;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        isFalling = false;
        isGrounded = false;
        startPosition = transform.position;
        rb = GetComponent<Rigidbody>();
        sensor.OnPlayerEnter += ActivateStalactite;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFalling)
        {
            Debug.Log("Fall");
            rb.useGravity = true;
            if(isGrounded)
            {
                isFalling = false;
            }
        }
    }

    private void ActivateStalactite()
    {
        isFalling = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //check when stalactite touches the ground layer
        if (collision.gameObject.layer == 8)
        {
            isGrounded = true;
            Break();
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
        isFalling = false;
        isGrounded = false;

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
