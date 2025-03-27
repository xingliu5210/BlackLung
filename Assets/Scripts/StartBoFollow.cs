using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBoFollow : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<AmosControls>().StartFollow();

            this.gameObject.SetActive(false);
        }
    }
}
