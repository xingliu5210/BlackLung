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

            if (TryRayForNavMesh(origin, Vector3.up, out Vector3 spawnPoint))
            {
                validSpawnPoints.Add(spawnPoint);
                continue;
            }

            if (TryRayForNavMesh(origin, Vector3.down, out spawnPoint))
            {
                validSpawnPoints.Add(spawnPoint);
            }
        }

        if (validSpawnPoints.Count == 0)
        {
            Debug.Log("Bo can't be safely spawned");
        }
        else if (validSpawnPoints.Count == 1)
        {
            SpawnBo(validSpawnPoints[0]);
        }
        else
        {
            // Pick the point closer to Bo's current position
            Vector3 closest = validSpawnPoints[0];
            float minDistance = Vector3.Distance(bo.transform.position, closest);

            foreach (var point in validSpawnPoints)
            {
                float dist = Vector3.Distance(bo.transform.position, point);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    closest = point;
                }
            }

            SpawnBo(closest);
        }
    }

    private bool TryRayForNavMesh(Vector3 origin, Vector3 direction, out Vector3 spawnPoint)
    {
        spawnPoint = Vector3.zero;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, raycastDistance))
        {
            if (NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, navmeshSampleRadius, NavMesh.AllAreas) &&
                NavMesh.SamplePosition(transform.position, out NavMeshHit amosNavHit, navmeshSampleRadius, NavMesh.AllAreas))
            {
                if (Vector3.Distance(navHit.position, amosNavHit.position) < 15f)
                {
                    spawnPoint = navHit.position + Vector3.up * spawnYOffset;
                    return true;
                }
            }
        }

        return false;
    }

    private void SpawnBo(Vector3 position)
    {
        bo.transform.position = position;
        bo.GetComponent<Rigidbody>().velocity = Vector3.zero;
        amosControls.boFollow = true;
        Debug.Log("Whistle is successful. Bo spawned at: " + position);
    }
}
