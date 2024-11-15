using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] private float damage;

    private void OnTriggerEnter(Collider other)
    {
        //Checks if collides with player. Then, reduces health by damage set.
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealth>().health -= damage;

            if (damage < 0)
            { Debug.Log("Player has recovered " + -damage + " health"); }
            else
            { Debug.Log("Player has taken " + damage + " damage"); }

            other.gameObject.GetComponent<PlayerHealth>().Healthbar();
        }
    }
}
