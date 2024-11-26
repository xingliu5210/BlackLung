using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float moveSpeed = 5f;

    float distanceToTarget;
    float distanceFromLight;

    bool followPlayer = true;


    public void Flee(bool fleeing, Vector3 lightPos)
    {
        //Avoid light
        if(fleeing)
        {
            followPlayer = false;

            Vector3 direction = (lightPos - transform.position).normalized;
            direction.z = 0; //lock axis
            transform.position -= direction * moveSpeed;
        }

        else followPlayer = true;
    }

    void FixedUpdate()
    {
        // Chase player once in range
        distanceToTarget = Vector3.Distance(transform.position, player.position);

        if (distanceToTarget <= detectionRange && followPlayer)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.z = 0; //lock axis
            transform.position += direction * moveSpeed;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Light"))
        {
            Debug.Log("Bat is in light range!");
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Light"))
        {
            GetComponent<CreatureFear>().TakeDamage(1);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Light"))
        {
            Debug.Log("Bat escaped the light.");
        }
    }

    // Detection range visualizer
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
