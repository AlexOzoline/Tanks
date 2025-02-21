using UnityEngine;

public class AITankTurretController : MonoBehaviour
{
    public Transform player; // Reference to the player's transform

    void Update()
    {
        if (player == null) return; // Ensure player is assigned

        // Get the player's position but keep Y-level constant
        Vector3 targetPosition = player.position;
        targetPosition.y = transform.position.y; 

        // Calculate direction to player
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Rotate turret toward player
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
    }
}
