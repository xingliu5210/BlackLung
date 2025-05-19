using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorManager : MonoBehaviour
{
    [SerializeField] private PlayerMovement amos;
    [SerializeField] private PlayerMovement bo; 
    [SerializeField] private Elevator_generator generator;
    [SerializeField] private List<InteractionZone> interactionZones;
    [SerializeField] private List<Transform> onBoardPositions;
    [SerializeField] private List<Transform> offLoadPositions;
    [SerializeField] private int currentLocation;


    private bool onElevator = false;
    [SerializeField] private bool isOnALevel;
    private float verticalInput;
    private void Start()
    {
        currentLocation = 0;
    }

    private void Update()
    {
        if (onElevator)
        {
            verticalInput = amos.GetVerticalInput();
            if(verticalInput != 0)
            {
                generator.MoveElevator(verticalInput);
            }
        }
    }
    public void InteractWithElevator(int playerLocation)
    {
        if(playerLocation != currentLocation)
        {
            //move elevator to current location
        }

        if(playerLocation == currentLocation)
        {
            //board the elevator
        }

        BoardElevator();
    }

    public void InteractWithElevator(InteractionZoneType type)
    {
        if (type == InteractionZoneType.OnElevator)
        {
            GetOffElevator();
        }
    }

    private void GetOffElevator()
    {
        
        isOnALevel = generator.withinUnboardRange;
        if (!generator.withinUnboardRange)
        {
            return;
        }
        int elevatorPosition = generator.GetElevatorPosition();
        amos.gameObject.transform.SetParent(null);
        amos.gameObject.transform.position = offLoadPositions[elevatorPosition].position;
        currentLocation = elevatorPosition;
        Debug.Log("GET OFF ELEVATOR: " + elevatorPosition);
        onElevator = false;
    }

    private void BoardElevator()
    {
        Debug.Log("BOARDING ELEVATOR");
        GameObject parent = generator.GetElevatorBox();
        amos.gameObject.transform.SetParent(parent.transform);
        amos.gameObject.transform.position = onBoardPositions[currentLocation].position;
        onElevator = true;
    }

    public void AddInteractionCollider(InteractionZone zone)
    {
        interactionZones.Add(zone);
        zone.SetLevelNumber(interactionZones.Count - 1);
        zone.SetManager(this);
    }

    public void ResetInteractionColliderList()
    {
        if (interactionZones.Count > 0)
        {
            for (int i = interactionZones.Count - 1; i >= 0; i--)
            {
                Destroy(interactionZones[i].gameObject);
                interactionZones.RemoveAt(i);
            }
        }
    }

    public void AddOnBoardPosition(Transform position)
    {
        onBoardPositions.Add(position);
    }

    public void AddOffLoadPosition(Transform position)
    {
        offLoadPositions.Add(position);
    }

    public void ResetPositionLists()
    {
        if(onBoardPositions.Count > 0)
        {
            for(int i = onBoardPositions.Count - 1; i >= 0; i--)
            {
                Destroy(onBoardPositions[i].gameObject);
                onBoardPositions.RemoveAt(i);
            }
        }

        if (offLoadPositions.Count > 0)
        {
            for (int i = offLoadPositions.Count - 1; i >= 0; i--)
            {
                Destroy(offLoadPositions[i].gameObject);
                offLoadPositions.RemoveAt(i);
            }
        }
    }
}
