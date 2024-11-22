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

    private float maxIntensity;
    [SerializeField] private float fuelUsagePercentPerSecond;
    private float fuelUsage;

    private float currentFuelPercent = 1f;

    [SerializeField] private float refuelPercent;
    private float refuelAmount;

    private float updateRate = 50f;

    private void Start()
    {
        //Set max brightness
        maxIntensity = lanternLight.intensity;

        //Translate fuel usage from percentage and seconds
        fuelUsage = maxIntensity * (fuelUsagePercentPerSecond / 100f / updateRate);

        //Translate refuel from percentage
        refuelAmount = maxIntensity * (refuelPercent / 100f);
    }
    private void FixedUpdate()
    {
        //Fuel burn per frame 
        if (lanternLight.intensity > 0)
        { lanternLight.intensity -= fuelUsage; }

        //Limit to brightness minimum value
        if (lanternLight.intensity < 0)
        { lanternLight.intensity = 0; }

        //Measure fuel percentage
        if (powerOn)
        currentFuelPercent = lanternLight.intensity / maxIntensity;

        //Collision radius and Color value changes with its brightness output
        lightCol.radius = Mathf.Lerp(0, 10, currentFuelPercent);
        currentColor = Color.Lerp(endColor, startColor, currentFuelPercent);

        //material.color = currentColor;
        material.SetColor("_EmissionColor", currentColor);
    }

    public void PowerToggle(Collider col)
    {
        powerOn = !powerOn;

        if (!powerOn)
        { 
            col.enabled = false;
            lanternLight.intensity = 0;
        }
        else
        { 
            col.enabled = true;
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
        lanternLight.intensity += refuelAmount;

        //Limit to brightness maximum value
        if (lanternLight.intensity > maxIntensity)
        { lanternLight.intensity = maxIntensity; }
    }
}
