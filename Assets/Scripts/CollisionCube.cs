using UnityEngine;

public class TankCollision : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            Debug.Log("Tank hit the border!");
            // Add behavior here (e.g., stop movement, bounce, etc.)
        }
        else
        {
            Debug.Log("Tank collided with something unknown");
        }
    }
}
