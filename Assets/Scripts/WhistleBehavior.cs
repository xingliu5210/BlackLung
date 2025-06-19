using UnityEngine;
using UnityEngine.AI;

public class WhistleBehavior : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera amosCamera;     // Assign Camera_Amos
    [SerializeField] private GameObject bo;         // Assign Bo object
    [SerializeField] private float raycastDistance = 50f;
    [SerializeField] private float navmeshSampleRadius = 2f;

    private CharacterSwitcher characterSwitcher;
    private AmosControls amosControls;

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
            // Only check visibility if Amos is the currently controlled character
            if (characterSwitcher.GetControlledCharacter() == amosControls)
            {
                CheckBoVisibility();
                TryWhistleSpawn();
            }
        }
    }

    private void CheckBoVisibility()
    {
        Renderer boRenderer = bo.GetComponentInChildren<Renderer>();
        if (boRenderer == null)
        {
            Debug.LogWarning("Bo has no Renderer attached.");
            return;
        }

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(amosCamera);
        bool isVisible = GeometryUtility.TestPlanesAABB(planes, boRenderer.bounds);

        Debug.Log(isVisible ? "Bo in Screen" : "Bo is off screen");
    }

    private void TryWhistleSpawn()
    {
        float camZDistance = Mathf.Abs(amosCamera.transform.position.z - transform.position.z);

        // Viewport points slightly outside left and right
        Vector3[] viewportPoints = new Vector3[]
        {
            new Vector3(-0.1f, 0.5f, camZDistance),  // left
            new Vector3(1.1f, 0.5f, camZDistance),   // right
        };

        foreach (var vp in viewportPoints)
        {
            Vector3 worldPoint = amosCamera.ViewportToWorldPoint(vp);

            if (CheckRayHitNavMesh(worldPoint, Vector3.up) ||
                CheckRayHitNavMesh(worldPoint, Vector3.down))
            {
                Debug.Log("Bo can be spawned");
                return;
            }
        }

        Debug.Log("Bo can't be safely spawned");
    }

    private bool CheckRayHitNavMesh(Vector3 origin, Vector3 direction)
    {
        if (Physics.Raycast(origin, direction, out RaycastHit hit, raycastDistance))
        {
            if (NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, navmeshSampleRadius, NavMesh.AllAreas) &&
                NavMesh.SamplePosition(transform.position, out NavMeshHit amosNavHit, navmeshSampleRadius, NavMesh.AllAreas))
            {
                // Compare area masks OR closeness in position
                if (Vector3.Distance(navHit.position, amosNavHit.position) < 10f)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
