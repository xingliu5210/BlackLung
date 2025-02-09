using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData
{
    public float playerX, playerY, playerZ;
    public float allyX, allyY, allyZ;
}
public class SaveSystem : MonoBehaviour
{
    public GameObject player;
    public GameObject bo;
    private Vector3 playerStartPos = new Vector3 (61f, 0, 0);
    private Vector3 boStartPos = new Vector3 (77.2f, 1.4f, 0);
    private static string savePath => Application.persistentDataPath + "/savegame.json";
    private static SaveSystem instance;
    // Start is called before the first frame update
    private void Awake()
    {
        // Ensure only one SaveSystem exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // ✅ Persist SaveSystem across scenes
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }

        if (player == null || bo == null)
        {
            Debug.LogError("Player or Bo is not assigned in SaveSystem!");
            return;
        }
        // Initialize start positions when the game begins
        // playerStartPos = player.transform.position;
        // boStartPos = bo.transform.position;
    }
    public static void SaveGame(Transform player, Transform ally)
    {
        SaveData data = new SaveData
        {
            playerX = player.position.x,
            playerY = player.position.y,
            playerZ = player.position.z,
            allyX = ally.position.x,
            allyY = ally.position.y,
            allyZ = ally.position.z
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        PlayerPrefs.SetInt("HasSave", 1); // Track that a save exists
        PlayerPrefs.Save();

        Debug.Log("Game Saved: " + savePath);
    }

    public static bool LoadGame(Transform playerTransform, Transform allyTransform)
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("No save file found!");
            return false;
        }

        string json = File.ReadAllText(savePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        playerTransform.position = new Vector3(data.playerX, data.playerY, data.playerZ);
        allyTransform.position = new Vector3(data.allyX, data.allyY, data.allyZ);

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

        // Reset additional game state variables if needed
        PlayerPrefs.DeleteAll(); // Ensure no leftover settings impact the game
        DeleteSave();

        Debug.Log("Game has been reset.");
    }
}
