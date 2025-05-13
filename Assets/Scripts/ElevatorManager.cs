using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorManager : MonoBehaviour
{
    [SerializeField] private Elevator_generator generator;
    [SerializeField] private List<Collider> interactionColliders;
    [SerializeField] private List<Transform> onBoardPositions;
    [SerializeField] private List<Transform> offLoadPositions;

    public void AddInteractionCollider(Collider collider)
    {
        interactionColliders.Add(collider);
    }

    public void ResetInteractionColliderList()
    {
        if (interactionColliders.Count > 0)
        {
            for (int i = interactionColliders.Count - 1; i >= 0; i--)
            {
                Destroy(interactionColliders[i].gameObject);
                interactionColliders.RemoveAt(i);
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
