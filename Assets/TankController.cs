using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TankController : MonoBehaviour
{
    public float speed = 10f;        // Movement speed (adjusted for realistic values)
    public float rotationSpeed = 100f; // Rotation speed
    public GameObject bulletPrefab;   // Bullet prefab reference
    public Transform cannonShootPoint; // The point from where bullets will be fired (cannon's muzzle)
    public float bulletSpeed = 20f;  // Adjust this value to control bullet speed
    public float shootForce = 500f;   // The force to apply to the bullet when shooting

    private Rigidbody rb;
    private Vector3 targetVelocity;  // To store the desired velocity for smooth movement
    private Vector3 currentVelocity; // The current velocity to smoothly transition

    void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();

        // Freeze position on Y and rotations on X/Z
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
    }

    void FixedUpdate()  // Handle movement logic in FixedUpdate for smooth physics-based movement
    {
        // Get input for movement and rotation
        float move = Input.GetAxis("Vertical");  // Up/Down arrow keys or W/S keys
        float rotate = Input.GetAxis("Horizontal"); // Left/Right arrow keys or A/D keys

        // Calculate target movement (forward/backward)
        Vector3 movement = transform.forward * move * speed;

        // Smoothly accelerate or decelerate the current velocity toward the target velocity
        currentVelocity = Vector3.MoveTowards(currentVelocity, movement, speed * Time.fixedDeltaTime);

        // Apply the movement with MovePosition to avoid stuttering
        rb.MovePosition(transform.position + currentVelocity * Time.fixedDeltaTime);

        // Calculate rotation and apply it using MoveRotation
        float rotation = rotate * rotationSpeed * Time.fixedDeltaTime;
        Quaternion turn = Quaternion.Euler(0f, rotation, 0f);
        rb.MoveRotation(rb.rotation * turn);

        // Enforce constraints: Lock X/Z rotation and Y position
        rb.position = new Vector3(rb.position.x, 0f, rb.position.z); // Keep Y position at 0
        rb.rotation = Quaternion.Euler(0f, rb.rotation.eulerAngles.y, 0f); // Lock X/Z rotation
    }

    void Update()  // Use Update for input detection (shooting in this case)
    {
        // Handle shooting
        if (Input.GetButtonDown("Fire1")) // Default: Mouse button 0 (Left-click) or Spacebar
        {
            Shoot();
            Debug.Log("Shooting");
        }
    }

    void Shoot()
    {
        // Instantiate the bullet at the cannon shoot point
        GameObject bullet = Instantiate(bulletPrefab, cannonShootPoint.position, cannonShootPoint.rotation);

        // Apply a 90-degree rotation to the bullet on the X-axis
        bullet.transform.Rotate(90, 0, 0);  // Rotate 90 degrees on the X-axis

        // Get the Rigidbody of the bullet
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        // Apply velocity to the bullet in the direction the cannon is pointing
        if (bulletRb != null)
        {
            bulletRb.linearVelocity = cannonShootPoint.forward * bulletSpeed; // Apply correct velocity
        }
    }

    // Detect collisions with bullets
    void OnCollisionEnter(Collision collision)
    {
        // Check if the object collided with the tank is a bullet
        Debug.Log("Something is colliding..." + collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Bullet")) // Ensure the bullet prefab has this tag
        {
            // Destroy the tank upon collision
            Destroy(gameObject); // Destroy the tank object
            Debug.Log("Tank was hit by a bullet and destroyed!");
        }
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            rb.linearVelocity = Vector3.zero; // Stop the tank from moving
            Debug.Log("Tank hit an obstacle!");
        }
    }

}
