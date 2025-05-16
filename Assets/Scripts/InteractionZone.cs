using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionZone : MonoBehaviour
{
    public int level;
    [SerializeField] private ElevatorManager manager;
    [SerializeField] private InteractionZoneType type;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLevelNumber(int level)
    {
        this.level = level;
    }

    public void SetManager(ElevatorManager manager)
    {
        this.manager = manager;
    }

    public void InteractWithElevator()
    {
       if (type == InteractionZoneType.OnElevator)
        {
            manager.InteractWithElevator(InteractionZoneType.OnElevator);
        } else if(type == InteractionZoneType.OnBoardElevator)
        {
            manager.InteractWithElevator(level);
        }
    }


}

public enum InteractionZoneType
{
    None,
    OnBoardElevator,
    OnElevator
}
