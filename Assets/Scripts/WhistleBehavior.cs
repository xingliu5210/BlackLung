using UnityEngine;

public class WhistleBehavior : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera amosCamera;     // Assign Camera_Amos
    [SerializeField] private GameObject bo;         // Assign Bo object

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
}
