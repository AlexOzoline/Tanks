using UnityEngine;

public class TankCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("BorderCube"))
        {
            Debug.Log("Tank hit the border!");
            // Add behavior here (e.g., stop movement, bounce, etc.)
        }
        else:
        {
            console.Log("Tank collided with something unknown");
        }
    }
}
