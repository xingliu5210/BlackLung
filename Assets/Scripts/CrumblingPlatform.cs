using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CrumblingPlatform : MonoBehaviour
{
    [SerializeField] private MeshRenderer cubeRenderer;
    [SerializeField] private GameObject platform;
    private bool crumbling;

    [SerializeField] private float crumbleTime;
    private float crumbleCountdown;

    // Start is called before the first frame update
    void Start()
    {
        // Set crumbleCountdown as the time it takes to crumble away.
        crumbleCountdown = crumbleTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(crumbling)
        {
            // Reduce countdown
            crumbleCountdown -= Time.deltaTime;

            // Making a temp variable to change the material's alpha channel. 
            Color color = cubeRenderer.material.color;

            // Reducing alpha channel by Delta time divided by the time it takes to disappear so that it becomes transparent at the same time the crumbling is complete.
            // This will be replaced by an animation of some sort
            color.a -= Time.deltaTime / crumbleTime;
            cubeRenderer.material.color = color;

            // Once crumbleCountdown reaches 0, disable game object and reset variables.
            Debug.Log(cubeRenderer.material.color);
            if (crumbleCountdown <= 0 )
            {
                crumbling = false;
                crumbleCountdown = crumbleTime;
                platform.SetActive(false);
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only need to check when the platform is not already crumbling and when it is still active.
        if (!crumbling && platform.activeSelf)
        {
            Debug.Log("Collision with: " + other.gameObject);
            if (other.gameObject.CompareTag("Player"))
            {

               crumbling = true;
            }
        }
    }
}
