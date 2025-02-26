using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; } // Singleton

    private Dictionary<string, int> inventoryItems = new Dictionary<string, int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Ensure persistence across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(string item, int quantity = 1)
    {
        if (inventoryItems.ContainsKey(item))
        {
            inventoryItems[item] += quantity;
        }
        else
        {
            inventoryItems[item] = quantity;
        }
        Debug.Log($"Added {quantity} {item}(s) to inventory. Total: {inventoryItems[item]}");
    }

    public bool HasItem(string item, int quantity = 1)
    {
        return inventoryItems.ContainsKey(item) && inventoryItems[item] >= quantity;
    }

    public void RemoveItem(string item, int quantity = 1)
    {
        if (HasItem(item, quantity))
        {
            inventoryItems[item] -= quantity;

            if (inventoryItems[item] <= 0)
            {
                inventoryItems.Remove(item); // Remove from dictionary if count reaches 0
            }
            Debug.Log($"Removed {quantity} {item}(s) from inventory.");
        }
        else
        {
            Debug.Log($"Not enough {item}(s) to remove.");
        }
    }

    public int GetItemCount(string item)
    {
        return inventoryItems.ContainsKey(item) ? inventoryItems[item] : 0;
    }

    // method to return inventory data for saving
    public Dictionary<string, int> SaveInventoryData()
    {
        if (inventoryItems == null || inventoryItems.Count == 0)
        {
            Debug.LogWarning("SaveInventoryData: No inventory items to save!");
            return new Dictionary<string, int>(); // ✅ Ensure empty inventory doesn't cause issues
        }
        
        Debug.Log("Saving Inventory: " + string.Join(", ", inventoryItems));
        return new Dictionary<string, int>(inventoryItems);
    }

    // method to restore inventory from save file
    public void LoadInventoryData(Dictionary<string, int> savedInventory)
    {
        if (savedInventory == null)
        {
            Debug.LogWarning("Saved inventory is null! Initializing new inventory.");
            inventoryItems = new Dictionary<string, int>(); // Initialize an empty dictionary
        }
        else
        {
            inventoryItems = new Dictionary<string, int>(savedInventory);
            Debug.Log("Inventory Loaded: " + string.Join(", ", inventoryItems));
        }
    }
}
