using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target; // The target to follow
    [SerializeField] private Vector3 offset = new Vector3(0, 2, 0); // Offset from the target
    [SerializeField] private float followSpeed = 5f; // Speed of the camera's movement

    private Camera activeCamera; // The currently active camera

    private void LateUpdate()
    {
        if (target == null) return;

        // Smoothly move the camera to follow the target
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z) + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget; // Dynamically update the target to follow
    }
}
