using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // private int keyCount = 0; // Number of keys collected
    private float FuelAmount = 0.0f; // The amount of fuel
    
    public void AddKey()
    {
        InventoryManager.Instance.AddItem("Key");
    }

    public int GetKeyCount()
    {
        return InventoryManager.Instance.GetItemCount("Key");
    }

    public void UseKey()
    {
        if (InventoryManager.Instance.HasItem("Key"))
        {
            InventoryManager.Instance.RemoveItem("Key");
            Debug.Log("Key used.");
        }
        else
        {
            Debug.Log("No keys available.");
        }
    }

    public void Addfuel()
    {
        GetComponentInChildren<Lantern>().Refuel(20); // Add Fuel to inventory

        //FuelAmount++;
        //Debug.Log("Fuel collected: " + FuelAmount);
    }
    /*
    public void UseFuel()
    {
        if (FuelAmount > 0)
        {
            FuelAmount--;
            Debug.Log("Fuel used. Remaining fuels: " + FuelAmount);
            GetComponent<Lantern>().Refuel(20);
        }
        else
        {
            Debug.LogWarning("No keys to use!");
        }
    }

    public float GetFuelAmount()
    {
        return FuelAmount;
    }*/
}
