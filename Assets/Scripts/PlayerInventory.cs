using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private int keyCount = 0; // Number of keys collected

    public void AddKey()
    {
        keyCount++;
        Debug.Log("Keys collected: " + keyCount);
    }

    public int GetKeyCount()
    {
        return keyCount;
    }
}
