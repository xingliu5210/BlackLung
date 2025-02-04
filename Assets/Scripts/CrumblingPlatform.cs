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

    [SerializeField] private float resetTime;
    private float resetCountdown;

    [SerializeField] ParticleSystem dustParticles;
    private float defaultParticleRate;
    [SerializeField] private float crumblingParticleRate = 5f;

    // Start is called before the first frame update
    void Start()
    {
        // Set crumbleCountdown as the time it takes to crumble away.
        crumbleCountdown = crumbleTime;
        resetCountdown = resetTime;

        defaultParticleRate = dustParticles.emission.rateOverTime.constant;
    }

    // Update is called once per frame
    void Update()
    {
        if(crumbling)
        {
            Debug.Log("Plat - crumbling");
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

        // If platform is inactive, start reset timer to reset it.
        if(!platform.activeSelf)
        {
            resetCountdown -= Time.deltaTime;

            if(resetCountdown <= 0 )
            {

                // Reset alpha channel
                Color color = cubeRenderer.material.color;
                color.a = 1;
                cubeRenderer.material.color = color;


                ParticleSystem.EmissionModule module = dustParticles.emission;
                module.rateOverTime = defaultParticleRate;

                resetCountdown = resetTime;
                platform.SetActive(true);
            }
        }
        
    }


    /// <summary>
    /// To be called when the platform needs to reset.
    /// </summary>
    public void Reset()
    {
        platform.SetActive(true);
    }


    /// <summary>
    /// Called when a collider collides with the platform.
    /// </summary>
    /// <param name="other">Other object's collider</param>
    private void OnTriggerEnter(Collider other)
    {
        // Only need to check when the platform is not already crumbling and when it is still active.
        if (!crumbling && platform.activeSelf)
        {
            Debug.Log("Collision with: " + other.gameObject);
            if (other.gameObject.CompareTag("Player"))
            {
                crumbling = true;
                ParticleSystem.EmissionModule module = dustParticles.emission;
                module.rateOverTime = crumblingParticleRate;
                
            }
        }
    }
}
