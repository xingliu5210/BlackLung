using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDebug : MonoBehaviour
{
    private void Update()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, GetComponent<BoxCollider>().size / 2, Quaternion.identity);
        foreach (var collider in colliders)
        {
            if (collider.gameObject != gameObject) // Exclude the elevator itself
            {
                Debug.Log($"Overlapping with: {collider.name}, Tag: {collider.tag}, Layer: {LayerMask.LayerToName(collider.gameObject.layer)}");
            }
        }
    }
}
