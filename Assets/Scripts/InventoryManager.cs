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
}
