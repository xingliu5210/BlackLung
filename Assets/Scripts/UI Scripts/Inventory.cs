using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] Vector3 openPosition;
    [SerializeField] Vector3 closePosition;
    [SerializeField] Vector3 currentPosition;
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
    // TODO temporary call for toggling inventory window
    // need to connect with control map call
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventoryWindow();
        }
    }

    public void ToggleInventoryWindow()
    {
        if(isOpen)
        {
            isOpen = false;
            isClosing = true;
            isOpening = false;
        } else if(!isOpen)
        {
            isOpen = true;  
            isOpening = true;
            isClosing = false;
        }
    }

    private IEnumerator InventoryAnimation()
    {
        while(true)
        {
            Debug.Log("start");
            if (isOpening)
            {
                Debug.Log("open coroutine");
                transform.position = Vector3.Lerp(transform.position, openPosition, speed * Time.deltaTime);
                if (transform.position.y > openPosition.y - 0.5f)
                {
                    isOpening = false;
                }
            }
            if (isClosing)
            {
                Debug.Log("clse coroutine");
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
