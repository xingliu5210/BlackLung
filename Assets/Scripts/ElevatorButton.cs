using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ElevatorButton : MonoBehaviour
{
    [SerializeField] private UI_ElevatorPanel panel;
    [SerializeField] private Button button;
    [SerializeField] private int buttonID; //Indicates floor it goes to. Level 0 is Ground floor
    [SerializeField] private Image image;

    [SerializeField] private TextMeshProUGUI buttonText;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonSetUp(int buttonID, UI_ElevatorPanel uiPanel)
    {
        panel = uiPanel;
        this.buttonID = buttonID;
        buttonText.text = buttonID.ToString();
    }

    public void OnClick()
    {
        panel.TryToGoToLevel(buttonID);
    }
}
