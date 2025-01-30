using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class KillBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth health = other.gameObject.GetComponent<PlayerHealth>();
        if(health != null)
        {
            health.Damage(health.health);
        }
    }
}
