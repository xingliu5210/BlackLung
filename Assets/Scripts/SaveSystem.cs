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
    private static string savePath => Application.persistentDataPath + "/savegame.json";
    // Start is called before the first frame update
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

        Debug.Log("Game Saved: " + savePath);
    }

    public static bool LoadGame(Transform player, Transform ally)
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("No save file found!");
            return false;
        }

        string json = File.ReadAllText(savePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        player.position = new Vector3(data.playerX, data.playerY, data.playerZ);
        ally.position = new Vector3(data.allyX, data.allyY, data.allyZ);

        Debug.Log("Game Loaded!");
        return true;
    }
}
