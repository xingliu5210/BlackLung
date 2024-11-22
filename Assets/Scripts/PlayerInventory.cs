using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private int keyCount = 0; // Number of keys collected
    private float FuelAmount = 0.0f; // The amount of fuel
    public void AddKey()
    {
        keyCount++;
        Debug.Log("Keys collected: " + keyCount);
    }

    public void UseKey()
    {
        if (keyCount > 0)
        {
            keyCount--;
            Debug.Log("Key used. Remaining keys: " + keyCount);
        }
        else
        {
            Debug.LogWarning("No keys to use!");
        }
    }

    public int GetKeyCount()
    {
        return keyCount;
    }

    public void Addfuel()
    {
        FuelAmount++;
        Debug.Log("Fuel collected: " + FuelAmount);
    }

    public void UseFuel()
    {
        if (FuelAmount > 0)
        {
            FuelAmount--;
            Debug.Log("Fuel used. Remaining fuels: " + FuelAmount);
        }
        else
        {
            Debug.LogWarning("No keys to use!");
        }
    }

    public float GetFuelAmount()
    {
        return FuelAmount;
    }
}
