using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDetect : MonoBehaviour
{
    bool danger;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Light"))
        {
            danger = true;

            GetComponentInParent<Bat>().Flee(true, other.gameObject.transform.position);
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
