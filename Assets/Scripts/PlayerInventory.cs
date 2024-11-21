using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private int keyCount = 0; // Number of keys collected

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
}
