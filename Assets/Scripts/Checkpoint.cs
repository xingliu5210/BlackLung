using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Vector3 savedPosition;

    // Start is called before the first frame update
    private void Start()
    {
        savedPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            GetComponent<PlayerHealth>().health += 100;
            GetComponent<PlayerHealth>().Healthbar();

            savedPosition = other.transform.position;

            Debug.Log("Checkpoint set to " + savedPosition);
        }
    }

    public void Respawn()
    { 
        transform.position = savedPosition;
        GetComponent<PlayerHealth>().health += 100;
        GetComponent<PlayerHealth>().Healthbar();
    }

}
