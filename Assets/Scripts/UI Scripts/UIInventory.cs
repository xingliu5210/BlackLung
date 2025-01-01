using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    [SerializeField] private Transform contentArea;
    [SerializeField] private UIInventoryItem uiItemPrefab;
    [SerializeField] private Sprite image; //DELETE

    [SerializeField] private float speed;
    [SerializeField] Vector3 openPosition;
    [SerializeField] Vector3 closePosition;
    //[SerializeField] Vector3 currentPosition; //DELETE

    private bool isRunning;
    [SerializeField] private bool isOpen;
    [SerializeField] private bool isOpening;
    [SerializeField] private bool isClosing;

    private void Start()
    {
        openPosition = transform.position;
        closePosition = openPosition;
        closePosition.y -= 215;
        transform.position = closePosition;

        isOpening = false;
        isClosing = false;
        isOpen = false;
        StartCoroutine(InventoryAnimation());
    }
    // TESTING ONLY
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Tab))
        //{
        //    ToggleInventoryWindow();
        //}
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("Add");
            AddItem(image);
        }
        //currentPosition = transform.position;
    }

    //When item is picked up it will call this function and
    // pass its icon image 
    public void AddItem(Sprite icon)
    {
        UIInventoryItem item = Instantiate(uiItemPrefab, contentArea);
        item.SetIcon(icon);
        
    }

    // Call this function to toggle inventory window view
    public void ToggleInventoryWindow()
    {
        if(isOpen)
        {
            isClosing = true;
            isOpening = false;
            isOpen = false;
        } else if(!isOpen)
        {  
            isOpening = true;
            isClosing = false;
            isOpen = true;
        }
    }

    private IEnumerator InventoryAnimation()
    {
        while(true)
        {
            if (isOpening)
            {
                transform.position = Vector3.Lerp(transform.position, openPosition, speed * Time.deltaTime);
                if (transform.position.y > openPosition.y - 0.5f)
                {
                    isOpening = false;
                }
            }
            if (isClosing)
            {
                transform.position = Vector3.Lerp(transform.position, closePosition, speed * Time.deltaTime);
                if (transform.position.y < closePosition.y + 0.5f)
                {
                    isClosing = false;
                }
            }
            yield return new WaitForSeconds(0.05f);
        }

    }

}
