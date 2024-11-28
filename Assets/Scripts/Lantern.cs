using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : MonoBehaviour
{
    private bool powerOn = true;
    [SerializeField] private SphereCollider lightCol;

    [SerializeField] private Light lanternLight;

    private Color startColor = new Color(1f, 0.76f, 0f); //Lanterns original bright glow #FFC200
    private Color endColor = Color.black; //Lantern gets dark as its light diminishes
    private Color currentColor;

    [SerializeField] private Material material;

    private float maxIntensity = 1.8f; //set max brightness
    private float fuelUsagePercentPerSec = 1.2f;
    private float fuelUsagePerSec;

    private float currentFuelPercent = 1f;

    private float updateRate = 50f;

    private void Start()
    {
        //Translate fuel usage from percentage and seconds
        fuelUsagePerSec = maxIntensity * (fuelUsagePercentPerSec / 100f / updateRate);
    }
    private void FixedUpdate()
    {
        //Fuel burn per frame 
        if (lanternLight.intensity > 0)
        { lanternLight.intensity -= fuelUsagePerSec; }

        //Limit to brightness minimum value
        if (lanternLight.intensity < 0)
        { lanternLight.intensity = 0; }

        //Measure fuel percentage
        if (powerOn)
        { currentFuelPercent = lanternLight.intensity / maxIntensity; }

        //Collision radius and Color value changes with its brightness output
        lightCol.radius = Mathf.Lerp(0, 10, currentFuelPercent);
        currentColor = Color.Lerp(endColor, startColor, currentFuelPercent);

        //material.color = currentColor;
        if (powerOn)
        { material.SetColor("_EmissionColor", currentColor); }
    }

    public void PowerToggle()
    {
        //Save fuel by toggling power
        powerOn = !powerOn;

        if (!powerOn)
        {
            lightCol.enabled = false;
            lanternLight.intensity = 0;
            material.SetColor("_EmissionColor", Color.black);
        }
        else
        {
            lightCol.enabled = true;
            lanternLight.intensity = currentFuelPercent * maxIntensity;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Water depletes fuel
        if (other.CompareTag("Water"))
        {
            lanternLight.intensity = 0;
        }
    }

    //Add fuel to lantern
    public void Refuel(float fuelChargePercent)
    {
        //Translate fuelCharge from percentage
        float fuelCharge = maxIntensity * (fuelChargePercent / 100f);

        if (powerOn)
        { lanternLight.intensity += fuelCharge; }
        else
        { currentFuelPercent += fuelChargePercent / 100f; }
        
        //Limit to brightness maximum value
        if (lanternLight.intensity > maxIntensity)
        { lanternLight.intensity = maxIntensity; }
    }
}
