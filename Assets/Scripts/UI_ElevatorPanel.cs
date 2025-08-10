using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ElevatorPanel : MonoBehaviour
{
    [SerializeField] private ElevatorManager manager;
    [SerializeField] private ElevatorButton elevatorButtonPrefab;
    [SerializeField] private GridLayoutGroup buttonGrid;
    [SerializeField] private int levels;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PopulatePanel(int elevatorNum)
    {
        levels = elevatorNum;
        for (int i = 0; i < elevatorNum; i++)
        {
            ElevatorButton button = Instantiate(elevatorButtonPrefab);
            button.transform.parent = buttonGrid.transform;
            button.ButtonSetUp(i, this);
        }
    }

    //buttonID is the level the elevator is meant to travel to 
    public void TryToGoToLevel(int buttonID)
    {
        manager.TryToGoTo(buttonID);
    }
}
