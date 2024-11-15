using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatureFear : MonoBehaviour
{
    [SerializeField] private float fear;
    [SerializeField] private float maxFear;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            fear -= 25;

            //Restrict Fear to maximum/minimum
            if (fear > maxFear)
            { fear = maxFear; }

            if (fear < 0)
            { fear = 0; }

            Debug.Log("Creature has " + fear + "/" + maxFear + "fear points");

            if (fear <= 0)
            {
                Debug.Log("Creature has been scared off!");
                fear = 100;
            }
        }
    }
}
