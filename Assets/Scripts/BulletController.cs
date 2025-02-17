using UnityEngine;
using Unity.Netcode;

public class BulletController : MonoBehaviour
{
    public float bulletSpeed = 200f; // Speed of the bullet
    public int maxBounces = 1; // Maximum number of bounces before the bullet is destroyed
    private int bounceCount = 0; // Current number of bounces

    private Rigidbody rb;

    void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();

        rb.linearDamping = 0f;
        rb.angularDamping = 0f;

        // Lock the Y position of the bullet
        //rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;

        // Set the initial velocity of the bullet
        rb.linearVelocity = transform.forward * bulletSpeed;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the bullet collides with an obstacle
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            // Increment the bounce count
            bounceCount++;

            // If the bullet has bounced too many times, destroy it
            if (bounceCount > maxBounces)
            {
                Destroy(gameObject);
                return;
            }
            // Get the collision normal
            Vector3 collisionNormal = collision.contacts[0].normal;

            // Calculate the reflected velocity using the collision normal
            Vector3 reflectedVelocity = Vector3.Reflect(transform.forward, collisionNormal);
            // Remove any vertical component from the reflected velocity
            //reflectedVelocity.y = 0f;

            // Normalize the reflected velocity and apply the bullet speed
            reflectedVelocity = reflectedVelocity.normalized * bulletSpeed;

            // Apply the reflected velocity to the bullet
            rb.linearVelocity = reflectedVelocity;

            // Rotate the bullet to face the new direction
            transform.forward = reflectedVelocity.normalized;
        }
        else if (collision.gameObject.CompareTag("Tank") || collision.gameObject.CompareTag("Bullet"))
        {
            // Handle collision with the tank (if needed)
            NetworkObject.Destroy(gameObject); // Destroy the bullet on hitting the tank
        }
    }
}