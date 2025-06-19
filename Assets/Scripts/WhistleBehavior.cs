using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WhistleBehavior : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera amosCamera;     // Assign Camera_Amos
    [SerializeField] private GameObject bo;         // Assign Bo object
    [SerializeField] private float raycastDistance = 50f;
    [SerializeField] private float navmeshSampleRadius = 2f;
    [SerializeField] private float spawnYOffset = 1f;

    private CharacterSwitcher characterSwitcher;
    private AmosControls amosControls;
    private bool boIsVisible = true;

    private void Start()
    {
        characterSwitcher = FindObjectOfType<CharacterSwitcher>();
        amosControls = GetComponent<AmosControls>();

        if (amosCamera == null || bo == null || characterSwitcher == null || amosControls == null)
        {
            Debug.LogError("Missing references in WhistleBehavior.");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                // Only check if Amos is the controlled character
                if (characterSwitcher.GetControlledCharacter() == amosControls)
                {
                    CheckBoVisibility();

                    if (!boIsVisible)
                    {
                        TryWhistleSpawn(); // Only try to spawn Bo if he's off-screen
                    }
                }
            }
        }
    }

    private void CheckBoVisibility()
    {
        Renderer boRenderer = bo.GetComponentInChildren<Renderer>();
        if (boRenderer == null)
        {
            Debug.LogWarning("Bo has no Renderer attached.");
            boIsVisible = false;
            return;
        }

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(amosCamera);
        boIsVisible = GeometryUtility.TestPlanesAABB(planes, boRenderer.bounds);

        Debug.Log(boIsVisible ? "Bo in Screen" : "Bo is off screen");
    }

    private void TryWhistleSpawn()
    {
        float camZDistance = Mathf.Abs(amosCamera.transform.position.z - transform.position.z);
        // float safeY = amosCamera.transform.position.y; // A good Y height to raycast from

        // Viewport points slightly outside left and right
        Vector3[] viewportPoints = new Vector3[]
        {
            new Vector3(-0.2f, 0.5f, camZDistance),  // left
            new Vector3(1.2f, 0.5f, camZDistance),   // right
        };

        List<Vector3> validSpawnPoints = new List<Vector3>();

        foreach (var vp in viewportPoints)
        {
            Vector3 origin = amosCamera.ViewportToWorldPoint(vp);
            // origin.y = safeY; // override to consistent cast height

            // Try upward and downward rays
            if (TryRayForNavMesh(origin, Vector3.up)) return;
            if (TryRayForNavMesh(origin, Vector3.down)) return;
        }

        Debug.Log("Bo can't be safely spawned");
    }

    private bool TryRayForNavMesh(Vector3 origin, Vector3 direction)
    {
        if (Physics.Raycast(origin, direction, out RaycastHit hit, raycastDistance))
        {
            // Check if hit point is on the NavMesh
            if (NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, navmeshSampleRadius, NavMesh.AllAreas) &&
                NavMesh.SamplePosition(transform.position, out NavMeshHit amosNavHit, navmeshSampleRadius, NavMesh.AllAreas))
            {
                if (Vector3.Distance(navHit.position, amosNavHit.position) < 13f)
                {
                    // Safe spawn point found — spawn Bo slightly above the surface
                    bo.transform.position = navHit.position + Vector3.up * spawnYOffset;
                    bo.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    // Enable following
                    amosControls.boFollow = true;

                    Debug.Log("Whistle is successful. Bo spawned at: " + bo.transform.position);
                    return true;
                }
            }
        }

        return false;
    }
}
