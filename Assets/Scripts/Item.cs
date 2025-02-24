using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;


[RequireComponent(typeof(BoxCollider))]
public class Item : MonoBehaviour
{
    [SerializeField] AK.Wwise.Event lanternPickUpFuelEvent;
    [SerializeField] AK.Wwise.Event elevatorPickUpKeyEvent;
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
                PlayPickUpKeySound();
                playerInventory.AddKey(); // Add key to inventory

                // **Mark this key as collected before destroying**
                SaveSystem.RegisterKeyPickup(gameObject.name);

                gameObject.SetActive(false); // **Deactivate instead of Destroy**
                break;
            case InteractionType.Fuel:
                Debug.Log("Picked up a fuel. +20%");
                PlayPickUpFuelSound();
                playerInventory.Addfuel(); // Add Fuel to inventory
                Destroy(gameObject); // Remove the Fuel after picking it up
                break;
            default:
                Debug.Log("Null Item");
                break;
        }
    }
    public void PlayPickUpFuelSound()
    {
        AkSoundEngine.PostEvent(lanternPickUpFuelEvent.Id, this.gameObject);
    }

    public void PlayPickUpKeySound()
    {
        AkSoundEngine.PostEvent(elevatorPickUpKeyEvent.Id, this.gameObject);
    }
}
