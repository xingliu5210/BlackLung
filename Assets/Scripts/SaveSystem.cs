using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class InventoryItem
{
    public string itemName;
    public int quantity;

    public InventoryItem(string name, int qty)
    {
        itemName = name;
        quantity = qty;
    }
}

[System.Serializable]
public class SaveData
{
    public float playerX, playerY, playerZ;
    public float allyX, allyY, allyZ;

    public float lanternFuelPercent;

    // Store elevator activation status using a serializable format
    public List<ElevatorState> elevatorStates = new List<ElevatorState>();
    public List<string> collectedKeys = new List<string>();
    public List<InventoryItem> inventoryItems = new List<InventoryItem>();
    // public Dictionary<string, int> savedInventoryItems;

    // Store tutorial states
    public List<TutorialState> tutorialTriggers = new List<TutorialState>();

}

[System.Serializable]
public class ElevatorState
{
    public string elevatorName;
    public bool isActivated;

    public ElevatorState(string name, bool activated)
    {
        elevatorName = name;
        isActivated = activated;
    }
}

[System.Serializable]
public class TutorialState
{
    public string tutorialName;
    public bool hasBeenTriggered;

    public TutorialState(string name, bool triggered)
    {
        tutorialName = name;
        hasBeenTriggered = triggered;
    }
}

public class SaveSystem : MonoBehaviour
{
    public GameObject player;
    public GameObject bo;
    private Lantern lantern;
    private Vector3 playerStartPos = new Vector3 (61f, 0, 0);
    private Vector3 boStartPos = new Vector3 (77.2f, 1.4f, 0);
    private static string savePath => Application.persistentDataPath + "/savegame.json";
    private static SaveSystem instance;
    private static HashSet<string> collectedKeysList = new HashSet<string>();

    // Start is called before the first frame update
    private void Awake()
    {
        // Ensure only one SaveSystem exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // ✅ Persist SaveSystem across scenes
            Debug.Log("SaveSystem initialized and set to persist.");
        }
        else
        {
            Debug.LogWarning("Duplicate SaveSystem detected and destroyed.");
            Destroy(gameObject); // Prevent duplicates
            return;
        }

        // if (player == null) player = GameObject.FindGameObjectWithTag("Player");
        // if (bo == null) bo = GameObject.FindGameObjectWithTag("Dog");

        // if (player == null || bo == null)
        // {
        //     Debug.LogError("Player or Bo is not assigned in SaveSystem!");
        //     return;
        // }
        // Initialize start positions when the game begins
        // playerStartPos = player.transform.position;
        // boStartPos = bo.transform.position;
    }

    private void Start()
    {
        StartCoroutine(FindPlayerAndBo());
    }
    private IEnumerator FindPlayerAndBo()
    {
        yield return new WaitForSeconds(0.5f); // Small delay to allow objects to load

        player = GameObject.FindGameObjectWithTag("Player");
        bo = GameObject.FindGameObjectWithTag("Dog");

        if (player == null || bo == null)
        {
            Debug.LogError("SaveSystem: Player or Bo is still null after attempting to find them.");
        }
        else
        {
            Debug.Log($"SaveSystem: Player and Bo successfully assigned - Player = {player.name}, Bo = {bo.name}");
        }

        // Find Lantern inside Player
        lantern = player.GetComponentInChildren<Lantern>();

        if (lantern == null)
        {
            Debug.LogWarning("Lantern not found! Fuel will not be saved.");
        }
    }
    public static void SaveGame(Transform player, Transform ally)
    {
        Debug.Log("[DEBUG] Calling SaveInventoryAtCheckpoint()");  
        InventoryManager.Instance.SaveInventoryAtCheckpoint();
        Dictionary<string, int> inventoryDictionary;
        inventoryDictionary = InventoryManager.Instance.GetSavedInventoryData();
        // ✅ Debugging: Print inventory before saving
        Debug.Log("[DEBUG] InventoryDictionary before saving: " + 
                (inventoryDictionary.Count > 0 ? string.Join(", ", inventoryDictionary) : "EMPTY"));

        List<InventoryItem> inventoryList = new List<InventoryItem>();
        foreach (var item in inventoryDictionary)
        {
            inventoryList.Add(new InventoryItem(item.Key, item.Value));
        }

        Debug.Log("[DEBUG] Inventory List before saving: " +
              (inventoryList.Count > 0 ? string.Join(", ", inventoryList) : "EMPTY"));

         // Collect tutorial trigger states
        List<TutorialState> tutorialStates = new List<TutorialState>();
        TutorialTrigger[] tutorialTriggers = Resources.FindObjectsOfTypeAll<TutorialTrigger>(); // Finds both active and inactive objects

        foreach (TutorialTrigger trigger in tutorialTriggers)
        {
            string tutorialKey = "Tutorial_" + trigger.popUpIndex;
            bool hasBeenTriggered = !trigger.gameObject.activeSelf;
            tutorialStates.Add(new TutorialState(tutorialKey, hasBeenTriggered));
        }

        SaveData data = new SaveData
        {
            playerX = player.position.x,
            playerY = player.position.y,
            playerZ = player.position.z,
            allyX = ally.position.x,
            allyY = ally.position.y,
            allyZ = ally.position.z,
            lanternFuelPercent = instance.lantern != null ? instance.lantern.GetfuelPercent() : 1.0f,
            elevatorStates = GetElevatorStates(),
            collectedKeys = new List<string>(collectedKeysList),
            inventoryItems = inventoryList,
            tutorialTriggers = tutorialStates
            // savedInventoryItems = InventoryManager.Instance.GetSavedInventoryData()
            // inventoryItems = InventoryManager.Instance.GetInventoryItems()
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        PlayerPrefs.SetInt("HasSave", 1); // Track that a save exists
        PlayerPrefs.Save();

        Debug.Log("Game Saved: " + savePath);
    }

    public static bool LoadGame(Transform playerTransform, Transform allyTransform)
    {
        // InventoryManager.Instance.GetInventoryItems().Clear();
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("No save file found!");
            return false;
        }

        string json = File.ReadAllText(savePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        // ** Re-find Player and Bo after scene changes**
        instance.player = GameObject.FindGameObjectWithTag("Player");
        instance.bo = GameObject.FindGameObjectWithTag("Dog");

        if (instance.player == null || instance.bo == null)
        {
            Debug.LogError("Player or Bo not found after loading scene!");
            return false;
        }

        // ✅ Restore Player and Bo positions
        instance.player.transform.position = new Vector3(data.playerX, data.playerY, data.playerZ);
        instance.bo.transform.position = new Vector3(data.allyX, data.allyY, data.allyZ);

        // ** Reassign Lantern after scene change**
        instance.lantern = instance.player.GetComponentInChildren<Lantern>();

        if (instance.lantern != null)
        {
            instance.lantern.SetFuelPercent(data.lanternFuelPercent); // ✅ Restore fuel level
        }
        else
        {
            Debug.LogWarning("Lantern not found! Fuel level not restored.");
        }

        RestoreElevatorStates(data.elevatorStates);

        // Convert List to Dictionary
        Dictionary<string, int> loadedInventory = new Dictionary<string, int>();
        foreach (var item in data.inventoryItems)
        {
            loadedInventory[item.itemName] = item.quantity;
        }

        InventoryManager.Instance.LoadInventoryData(loadedInventory);

        RestoreCollectedKeys(data.collectedKeys);
        RestoreTutorialTriggers(data.tutorialTriggers);

        RespawnTemporaryObjects();

        Debug.Log("Game Loaded!");
        return true;
    }

    public static void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            PlayerPrefs.DeleteKey("HasSave"); // Remove save flag
            PlayerPrefs.Save();
            Debug.Log("Save data deleted. Starting fresh.");
        }
    }

    public void ResetGame()
    {
        Debug.Log("Resetting game to initial state...");

        if (player == null || bo == null)
        {
            Debug.LogError("Player or Bo reference is null! Ensure they are assigned before calling ResetGame.");
            return;
        }

        // Reset player and Bo positions
        player.transform.position = playerStartPos;
        bo.transform.position = boStartPos;

        // Reset all tutorial triggers
        ResetTutorialTriggers();

        // Reset additional game state variables if needed
        PlayerPrefs.DeleteAll(); // Ensure no leftover settings impact the game
        DeleteSave();

        Debug.Log("Game has been reset.");
    }
    // **Helper Functions for Elevator State**
    private static List<ElevatorState> GetElevatorStates()
    {
        List<ElevatorState> elevatorStates = new List<ElevatorState>();
        Elevator[] elevators = GameObject.FindObjectsOfType<Elevator>();

        foreach (Elevator elevator in elevators)
        {
            elevatorStates.Add(new ElevatorState(elevator.gameObject.name, elevator.IsActivated));
        }

        return elevatorStates;
    }

    private static void RestoreElevatorStates(List<ElevatorState> elevatorStates)
    {
        Elevator[] elevators = GameObject.FindObjectsOfType<Elevator>();

       foreach (Elevator elevator in elevators)
        {
            foreach (ElevatorState state in elevatorStates)
            {
                if (elevator.gameObject.name == state.elevatorName)
                {
                    elevator.SetActivated(state.isActivated);
                }
            }
        }
    }
    private static void RestoreCollectedKeys(List<string> collectedKeys)
    {
        collectedKeysList.Clear();

        if (collectedKeys == null)
        {
            Debug.LogWarning("No collected keys found in save file!");
            return;
        }

        foreach (string keyName in collectedKeys)
        {
            collectedKeysList.Add(keyName); // Re-add keys to memory
        }

        Item[] keys = GameObject.FindObjectsOfType<Item>();

        foreach (Item key in keys)
        {
            if (key.type == Item.InteractionType.Key && collectedKeysList.Contains(key.gameObject.name))
            {
                key.gameObject.SetActive(false); // ✅ Disable picked-up keys
                Debug.Log("Restored: Key " + key.gameObject.name + " is disabled.");
            }
            else
            {
                key.gameObject.SetActive(true);
            }
        }
    }

    private static void RestoreTutorialTriggers(List<TutorialState> tutorialTriggers)
    {
         // Restore tutorial triggers in the level
        TutorialTrigger[] allTriggers = Resources.FindObjectsOfTypeAll<TutorialTrigger>(); // Get all, even disabled ones

        foreach (TutorialTrigger trigger in allTriggers)
        {
            string tutorialKey = "Tutorial_" + trigger.popUpIndex;

            // Find matching saved state
            TutorialState savedState = tutorialTriggers.Find(state => state.tutorialName == tutorialKey);

             if (savedState != null)
            {
                // Restore state: Disable if it was triggered, otherwise enable it
                trigger.gameObject.SetActive(!savedState.hasBeenTriggered);
            }
            else
            {
                // If no saved state exists, default to enabling it
                trigger.gameObject.SetActive(true);
            }
        }

        Debug.Log("[LOAD] Tutorial triggers restored successfully.");
    }

    // **Respawning Temporary Objects**
    private static void RespawnTemporaryObjects()
    {
        string[] respawnTags = { "Enemy", "Stalactite", "Fuel", "Platform" };

        foreach (string tag in respawnTags)
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);

            foreach (GameObject obj in objects)
            {
                obj.SetActive(true);
            }
        }
    }

    // Store the name of the picked-up key
    public static void RegisterKeyPickup(string keyName)
    {
        if (!collectedKeysList.Contains(keyName))
        {
            collectedKeysList.Add(keyName);
        }
    }

    // New method to reset tutorial triggers
    private void ResetTutorialTriggers()
    {
        foreach (TutorialTrigger trigger in FindObjectsOfType<TutorialTrigger>())
        {
            trigger.gameObject.SetActive(true); // Ensure all triggers are enabled again
        }

        PlayerPrefs.DeleteAll(); // Clear any previous save states
        PlayerPrefs.Save();
        Debug.Log("Tutorial popups have been reset.");
    }
}

