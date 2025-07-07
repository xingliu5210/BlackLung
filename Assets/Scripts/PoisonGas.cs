using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoisonGas : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float damagePerIncrement = 1f;
    [SerializeField] float sec1DamageFactor = 1f;
    [SerializeField] float sec2DamageFactor = 2f;
    [SerializeField] float centreDamageFactor = 3f;
    [SerializeField] float damageCooldown = 2f;

    [Header("Assets required")]
    [SerializeField] private AmosControls amos;
    [SerializeField] private TripLine leftSensor;
    [SerializeField] private TripLine rightSensor;
    [SerializeField] private Transform centrePoint;
    private bool inRange = false;

    [Header("TESTING")]
    [SerializeField] private float leftBoundary;
    [SerializeField] private float rightBoundary;

    [SerializeField] private float playerPosition;
    [SerializeField] private float centre;
    //right side
    [SerializeField] private float rDistance;
    [SerializeField] private float rSec1; //section farthest from centre & weakest damage
    [SerializeField] private float rSec2;
    //left side
    [SerializeField] private float lDistance;
    [SerializeField] private float lSec1; //section farthest from centre 
    [SerializeField] private float lSec2; 

    [SerializeField] private float damage;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        leftSensor.OnPlayerPass += CheckPlayerLocation;
        rightSensor.OnPlayerPass += CheckPlayerLocation;

        leftBoundary = leftSensor.transform.position.x;
        rightBoundary = rightSensor.transform.position.x ;

        timer = damageCooldown;

        centre = centrePoint.transform.position.x;
        //Calculate sections from of centre to right boundary.
        rDistance = rightBoundary - centre;
        rSec1 = rDistance * 0.3f + centre;
        rSec2 = rDistance * 0.1f + centre;

        //Calculate sections from left boundary to centre;
        lDistance = centre - leftBoundary;
        lSec1 = centre - lDistance * 0.3f;
        lSec2 = centre - lDistance * 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (inRange)
        {
            playerPosition = amos.transform.position.x;
            if (playerPosition > rSec1 || playerPosition < lSec1)
            {
                Debug.Log("DAMAGE 1");
                damage = damagePerIncrement * sec1DamageFactor;
            } else if ((playerPosition <= rSec1 && playerPosition > rSec2) || (playerPosition >= lSec1 && playerPosition < lSec2))
            {
                Debug.Log("DAMAGE 2");
                damage = damagePerIncrement * sec2DamageFactor;
            } else if ((playerPosition <= rSec2 && playerPosition > centre) || (playerPosition >= lSec2 && playerPosition < centre))
            {
                Debug.Log("Damage CENTRE");
                damage = damagePerIncrement * centreDamageFactor;
            }

            
            if(timer <= 0) {
                amos.GetComponent<PlayerHealth>().Damage(damage);
                timer = damageCooldown;
            } else
            {
                timer -= Time.deltaTime;
            }
        }
    }

    private void CheckPlayerLocation()
    {
        //if out of bounds
        if (amos.transform.position.x <= leftBoundary || amos.transform.position.x >= rightBoundary)
        {
            Debug.Log("OUT");
            inRange = false;
            return;
        } else
        {
            Debug.Log("IN");
            inRange = true;
        }
    }
}
