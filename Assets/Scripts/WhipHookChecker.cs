using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhipHookChecker : MonoBehaviour
{
    [SerializeField] private CapsuleCollider collider;
    public List<WhipHook> hooksInRange;
    // Start is called before the first frame update

    /// <summary>
    /// Checks to see if overlapped trigger is a WhipHook object.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Hook"))
        {
            hooksInRange.Add(other.gameObject.GetComponent<WhipHook>());
        }
    }

    /// <summary>
    /// Removes WhipHook from array when it leaves the checker's range.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {        
        if (other.gameObject.CompareTag("Hook"))
        {
            hooksInRange.Remove(other.gameObject.GetComponent<WhipHook>());
        }
    }

    /// <summary>
    /// Gets the hook closest to the player.
    /// </summary>
    /// <param name="position">The player's position.</param>
    /// <returns>The closest hook in range.</returns>
    public WhipHook GetClosestHook(Vector3 position)
    {
        WhipHook closestHook = null;
        foreach (var hook in hooksInRange)
        {
            // Check if closestHook has been assigned yet, assigning it the current hook if it is.
            if(closestHook != null)
            {
                // Calculate distance between the player and the current hook, and the between the player and the hook currently flagged as the closest hook.
                float distance = Vector3.Distance(hook.gameObject.transform.position, position);
                float closestHookDistance = Vector3.Distance(closestHook.gameObject.transform.position, position);

                // Compare the two distances, and reassign the closestHook if the current hook being checked is closer.
                if (distance < closestHookDistance)
                {
                    closestHook = hook;
                }
            }
            else
            {
                closestHook = hook;
            }
        }

        return closestHook;
    }
}
