using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDamage : MonoBehaviour
{
    private bool skipUpdate;
    public bool airborne;

    private float fallStart;
    private float fallEnd;

    //Damage will scale from 1 to 100 based on height range
    [SerializeField] private float minFallHeight; 
    [SerializeField] private float maxFallHeight;

    public void FallStartSet()
    {
        fallStart = transform.position.y;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Ground") && airborne)
        {
            airborne = false;
            skipUpdate = true;

            Debug.Log("Player has landed!");
            fallEnd = transform.position.y;

            float fallHeight = fallStart - fallEnd;

            if(fallHeight >= minFallHeight) // Determines if Damage should be taken
            {
                if (fallHeight > maxFallHeight) // Damage scaling cap
                { fallHeight = maxFallHeight; }
                
                float damageScale = (fallHeight - minFallHeight) / (maxFallHeight - minFallHeight); // Damage scaler 0 - 1

                int damage = Mathf.RoundToInt(Mathf.Lerp(1f, 100f, damageScale)); // Translate (0 - 1)float to (0 - 100)int

                Debug.Log("Player has taken " + damage + " fall damage.");

                GetComponent<PlayerHealth>().Damage(damage);
            }

        }

    }

    private void OnCollisionStay(Collision col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            airborne = false;
            skipUpdate = true;
        }
    }

    private void FixedUpdate()

    {
        if (!skipUpdate)
        {
            if (airborne == false || GetComponent<PlayerMovement>().isClimbing) //|| GetComponent<AmosControls>().hooking)
            { 
                fallStart = transform.position.y;
                Debug.Log("Player is airborn!");
            }

            airborne = true;
        }
    
        skipUpdate = false;
    }
}
