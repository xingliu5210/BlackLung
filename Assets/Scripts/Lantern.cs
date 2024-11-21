using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : MonoBehaviour
{
    //private bool PowerOn = false;

    public Light Lightsource1;
    public Light Lightsource2;

    private Color startColor = new Color(1.0f, 0.76f, 0.0f); //Lanterns original bright glow #FFC200
    private Color endColor = Color.black; //Lantern gets dark as its light diminishes
    private Color currentColor;

    [SerializeField] private Material material;

    private float maxBrightness;
    [SerializeField] private float fuelUsagePercentPerSecond;
    private float fuelUsage;

    private float currentFuelPercent;

    [SerializeField] private float refuelPercent;
    private float refuelAmount;

    private float updateRate = 50f;

    private void Start()
    {
        maxBrightness = Lightsource1.intensity;

        //Translate from percentage
        fuelUsage = maxBrightness * (fuelUsagePercentPerSecond / 100 / updateRate);
        refuelAmount = maxBrightness * (refuelPercent / 100);
    }
    private void FixedUpdate()
    {
        //Brightness maximum value
        if (Lightsource1.intensity > maxBrightness)
        { 
            Lightsource1.intensity = maxBrightness;
            Lightsource2.intensity = maxBrightness;
        }

        //Fuel burn per frame
        if (Lightsource1.intensity > 0)
        {
            Lightsource1.intensity -= fuelUsage;
            Lightsource2.intensity -= fuelUsage;
        }
        else if (Lightsource1.intensity < 0)
        { 
            Lightsource1.intensity = 0;
            Lightsource2.intensity = 0;
        }
        else
        {
            //PowerOn = false;
        }

        //The lamps color value changes with its brightness output
        currentFuelPercent = Lightsource1.intensity / maxBrightness;
        currentColor = Color.Lerp(endColor, startColor, currentFuelPercent);

        //material.color = currentColor;
        material.SetColor("_EmissionColor", currentColor);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Water depletes fuel
        if (other.CompareTag("Water"))
        {
            Lightsource1.intensity = 0;
            Lightsource2.intensity = 0;
        }

        //Remove this below once Refuel() function works with inventory
        if (other.CompareTag("Pickup"))
        {
            Refuel();
            Destroy(other.gameObject);
        }
    }

    //Add 20% fuel to lantern
    public void Refuel()
    {
        Lightsource1.intensity += refuelAmount;
        Lightsource2.intensity += refuelAmount;

        if (Lightsource1.intensity > maxBrightness)
        {
            Lightsource1.intensity = maxBrightness;
            Lightsource2.intensity = maxBrightness;
        }
    }
}
