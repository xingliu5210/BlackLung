using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickaxe : MonoBehaviour
{
    private bool mining;
    int frame = 0;
    [SerializeField] private int miningFrames;

    public GameObject rock;

    private void FixedUpdate()
    {
        if (mining && rock != null && GetComponent<PlayerMovement>().grounded)
        {
            frame++;
            GetComponent<PlayerMovement>().OnMine(true);

            if (frame >= miningFrames)
            {
                Destroy(rock);
                frame = 0;
                GetComponent<PlayerMovement>().OnMine(false);
                mining = false;
            }
        }
        else
        {
            frame = 0;
            GetComponent<PlayerMovement>().OnMine(false);
            mining = false;
        }
    }

    public void Mining()
    {

        mining = true;
    }
}
