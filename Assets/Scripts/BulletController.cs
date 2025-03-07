using UnityEngine;
using Unity.Netcode;

public class BulletController : MonoBehaviour
{
    public float bulletSpeed = 200f; // Speed of the bullet
    public int maxBounces = 1; // Maximum number of bounces before the bullet is destroyed
    private int bounceCount = 0; // Current number of bounces

    private Rigidbody rb;
    private ParticlePooler particlePooler; // Reference to ParticlePooler

    public GameObject bulletTrailPrefab; // Particle effect for the trail (you can assign this in the Unity inspector)

    private GameObject trailEffect; // Reference to the trail particle effect

    private float trailDistance = 3f;

    void Start()
    {
        // Get the Rigidbody component
        Debug.Log("bullet");
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = 0f;
        rb.angularDamping = 0f;

        // Set the initial velocity of the bullet
        rb.linearVelocity = transform.forward * bulletSpeed;

        // Get reference to the ParticlePooler
        particlePooler = FindObjectOfType<ParticlePooler>();

        if (particlePooler != null)
        {
            // Get a bullet trail particle effect from the pool and position it at the bullet's location
            trailEffect = particlePooler.GetParticle();
            trailEffect.transform.position = transform.position + (-transform.forward * trailDistance);
            trailEffect.transform.SetParent(transform); // Attach to bullet
            trailEffect.transform.rotation = transform.rotation; // Set the rotation to match the bullet's initial direction
        }
        else
        {
            Debug.LogError("ParticlePooler not found in the scene.");
        }
    }

    void Update()
    {
        // Update trail particle effect's position to follow the bullet
        if (trailEffect != null)
        {
            // Set the trail position relative to the bullet's current forward direction
            trailEffect.transform.position = transform.position + (-transform.forward * trailDistance);
        }
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
                DestroyBullet();
                return;
            }

            // Get the collision normal
            Vector3 collisionNormal = collision.contacts[0].normal;

            // Calculate the reflected velocity using the collision normal
            Vector3 reflectedVelocity = Vector3.Reflect(transform.forward, collisionNormal);
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
            DestroyBullet(); // Destroy the bullet on hitting the tank
        }
    }

    // Function to destroy the bullet and return particle to the pool
    private void DestroyBullet()
    {
        // Return the particle effect to the pool
        if (particlePooler != null && trailEffect != null)
        {
            particlePooler.ReturnParticle(trailEffect); // Return to the pool
        }

        // Destroy the bullet object itself
        NetworkObject.Destroy(gameObject);
    }
}
