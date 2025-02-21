using UnityEngine;

public class TankTurretController : MonoBehaviour
{
    public Camera mainCamera;

    void Update()
    {
        // Raycast from the mouse position
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        // Cast a ray to the ground plane
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 targetPosition = hit.point;
            targetPosition.y = transform.position.y; // Keep the turret level

            // Rotate toward the target position
            Vector3 direction = (targetPosition - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
        }
    }
}
