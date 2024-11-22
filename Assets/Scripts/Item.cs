using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Item : MonoBehaviour
{
    public enum InteractionType { None,PickUp,Examine, Key, Fuel}
    public InteractionType type;
    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
        gameObject.layer = 10;
    }

    public void Interact(PlayerInventory playerInventory)
    {
        switch(type)
        {
            case InteractionType.PickUp:
                Debug.Log("Pick Up");
                break;
            case InteractionType.Examine:
                Debug.Log("Examine");
                break;
            case InteractionType.Key:
                Debug.Log("Picked up a key.");
                playerInventory.AddKey(); // Add key to inventory
                Destroy(gameObject); // Remove the key after picking it up
                break;
            case InteractionType.Fuel:
                Debug.Log("Picked up a fuel.");
                playerInventory.Addfuel(); // Add Fuel to inventory
                Destroy(gameObject); // Remove the Fuel after picking it up
                break;
            default:
                Debug.Log("Null Item");
                break;
        }
    }
}
