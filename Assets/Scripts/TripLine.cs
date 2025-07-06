using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripLine : MonoBehaviour
{
    public Action OnPlayerPass;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerPass.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerPass.Invoke();
        }
    }
}
