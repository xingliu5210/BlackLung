using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] popUps;
    private int popUpIndex;
    private bool leftArrowPressed = false;
    private bool rightArrowPressed = false;
    // Update is called once per frame
    // Update is called once per frame
    void Update()
    {
        // Show only the active popup
        for (int i = 0; i < popUps.Length; i++)
        {
            popUps[i].SetActive(i == popUpIndex);
        }

        // Handle the first popup logic
        if (popUpIndex == 0) 
        {
            if (Input.GetKeyDown(KeyCode.A)) 
            {
                leftArrowPressed = true;
            }

            if (Input.GetKeyDown(KeyCode.D)) 
            {
                rightArrowPressed = true;
            }

            // If both keys are pressed, move to the next popup
            if (leftArrowPressed && rightArrowPressed) 
            {
                popUpIndex++;
                ResetKeyPressStates();
            }
        }
        else if (popUpIndex == 1)
        {
            // Handle the second popup logic
            if (Input.GetKeyDown(KeyCode.Space)) 
            {
                popUpIndex++;
            }
        }
    }

    // Helper method to reset key press states
    private void ResetKeyPressStates()
    {
        leftArrowPressed = false;
        rightArrowPressed = false;
    }
}
