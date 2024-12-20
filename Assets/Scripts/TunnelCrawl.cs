using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelCrawl : MonoBehaviour
{
    public int tunnelCooldown;
    private int counter = 0;

    bool inCooldown = false;

    // Public getter for inCooldown
    public bool IsInCooldown => inCooldown;

    private void FixedUpdate()
    {
        if (inCooldown == true)
        { Cooldown();}
    }

    void Cooldown()
    {
        counter++;

        if (counter >= tunnelCooldown)
        {
            counter = 0;
            inCooldown = false;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Tunnel") && !inCooldown)
        {
            Debug.Log("TUNNEL!");
            transform.position = col.GetComponent<Tunnel>().PairedTunnel.transform.position;
            inCooldown = true;
        }
    }
}
