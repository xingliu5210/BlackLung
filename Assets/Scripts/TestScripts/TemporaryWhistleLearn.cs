using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryWhistleLearn : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<AmosControls>().whistleLearned = true;
            Debug.Log("Whistle Learned!");
            Destroy(gameObject);
        }
    }
}
