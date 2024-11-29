using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDamage : MonoBehaviour
{
    bool skipUpdate;
    bool airborne;

    float fallStart;
    float fallEnd;

    //Damage will scale from 1 to 100 based on height range
    [SerializeField] float minFallHeight; 
    [SerializeField] float maxFallHeight;

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Ground") && airborne)
        {
            airborne = false;
            skipUpdate = true;

            Debug.Log("Player has landed!");
            fallEnd = transform.position.y;

            float fallHeight = fallStart - fallEnd;

            if(fallHeight >= minFallHeight)
            {
                if (fallHeight > maxFallHeight)
                { fallHeight = maxFallHeight; }
                
                float normalizedFallHeight = (fallHeight - minFallHeight) / (maxFallHeight - minFallHeight);

                int damage = Mathf.RoundToInt(Mathf.Lerp(1f, 100f, normalizedFallHeight));

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
            if (airborne == false)
            { 
                fallStart = transform.position.y;
                Debug.Log("Player is airborn!");
            }

            airborne = true;
        }
    
        skipUpdate = false;
    }
}
