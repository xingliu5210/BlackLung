using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonGas : MonoBehaviour
{
    [SerializeField] private AmosControls amos;
    [SerializeField] private TripLine leftSensor;
    [SerializeField] private TripLine rightSensor;
    private bool inRange = false;

    private float leftBoundary;
    private float rightBoundary;

    // Start is called before the first frame update
    void Start()
    {
        leftSensor.OnPlayerPass += CheckPlayerLocation;
        rightSensor.OnPlayerPass += CheckPlayerLocation;

        leftBoundary = leftSensor.transform.position.x;
        rightBoundary = rightSensor.transform.position.x ;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CheckPlayerLocation()
    {
        //if out of bounds
        if (amos.transform.position.x <= leftBoundary || amos.transform.position.x >= rightBoundary)
        {
            Debug.Log("OUT");
            return;
        }
    }
}
