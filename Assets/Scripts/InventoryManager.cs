using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; } // Singleton

    private Dictionary<string, int> inventoryItems = new Dictionary<string, int>();
    private Dictionary<string, int> savedInventoryItems = new Dictionary<string, int>(); 

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
            Debug.LogWarning("[LOAD] No inventory data in save file! Initializing empty inventory.");
            savedInventoryItems = new Dictionary<string, int>(); // ✅ Ensure empty inventory is handled
            inventoryItems = new Dictionary<string, int>();
        }
        else
        {
            savedInventoryItems = new Dictionary<string, int>(savedInventory); // ✅ Load saved inventory
            inventoryItems = new Dictionary<string, int>(savedInventoryItems); // ✅ Reset temp inventory
        }

        Debug.Log("[LOAD] Inventory restored: " + string.Join(", ", inventoryItems));
    }

    // Method to save inventory at checkpoint
    public void SaveInventoryAtCheckpoint()
    {
        savedInventoryItems.Clear();
        savedInventoryItems = new Dictionary<string, int>(inventoryItems);
        Debug.Log("[CHECKPOINT] Inventory saved: " + string.Join(", ", savedInventoryItems));
    }

    // Method to load inventory from the last checkpoint
    public void LoadInventoryFromCheckpoint()
    {
        inventoryItems = new Dictionary<string, int>(savedInventoryItems);
        Debug.Log("[LOAD] Inventory restored from checkpoint: " + string.Join(", ", inventoryItems));
    }
    public Dictionary<string, int> GetSavedInventoryData()
    {
        if (savedInventoryItems == null || savedInventoryItems.Count == 0)
        {
            Debug.LogWarning("[ERROR] GetSavedInventoryData() - No saved inventory found!");
        }
        else
        {
            Debug.Log("[DEBUG] GetSavedInventoryData before saving: " + string.Join(", ", savedInventoryItems));
        }

        return new Dictionary<string, int>(savedInventoryItems);
    }

    public Dictionary<string, int> GetInventoryForSave()
    {
        return new Dictionary<string, int>(inventoryItems); // ✅ Return checkpoint inventory for saving
    }

    public Dictionary<string, int> GetInventoryItems()
    {
        return inventoryItems;
    }
}
