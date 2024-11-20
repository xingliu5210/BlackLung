using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : MonoBehaviour
{
    public Light Lightsource1;
    public Light Lightsource2;

    [SerializeField] private float maxBrightness;
    [SerializeField] private float fuelPerFrame;

    private void FixedUpdate()
    {
        //Light reduction overtime
        if (Lightsource1.intensity > 0)
        {
            Lightsource1.intensity -= fuelPerFrame;
            Lightsource2.intensity -= fuelPerFrame;
        }
        else if (Lightsource1.intensity < 0)
        { 
            Lightsource1.intensity = 0;
            Lightsource2.intensity = 0;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            //Reset light intensity with fuel pickup
            Lightsource1.intensity = maxBrightness;
            Lightsource2.intensity = maxBrightness;

            Destroy(other.gameObject);
        }
    }
}
