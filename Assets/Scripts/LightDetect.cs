using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDetect : MonoBehaviour
{
    bool danger;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Light") || other.CompareTag("Checkpoint"))
        {
            danger = true;

            GetComponentInParent<Bat>().Flee(true, other.gameObject.transform.position);

            Debug.Log("fleeing");
        }
    }

    private void FixedUpdate()
    {
        if (!danger)
        {
            GetComponentInParent<Bat>().Flee(false, Vector3.zero);
        }

        danger = false;
    }
}
