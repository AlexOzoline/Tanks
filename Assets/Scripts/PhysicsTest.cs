using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsTest : MonoBehaviour
{
    public float speed = 10f;          // Speed factor for movement
    public float rotationSpeed = 50f;  // Speed for rotation
    public GameObject bulletPrefab;    // Bullet prefab reference
    public Transform cannonShootPoint; // Bullet spawn point
    public float bulletSpeed = 200f;   // Bullet speed

    private Rigidbody rb;

    void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();

        // Freeze rotation on X/Z and position on Y to keep the tank upright
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
    }

    void FixedUpdate()
    {
        // Get input for movement and rotation
        float move = Input.GetAxis("Vertical");  // Up/Down arrow keys or W/S keys
        float rotate = Input.GetAxis("Horizontal"); // Left/Right arrow keys or A/D keys

        // Apply movement instantly using VelocityChange
        Vector3 movement = transform.forward * move * speed;
        rb.AddForce(movement, ForceMode.VelocityChange);

        // Apply instant rotation using AngularVelocity
        rb.angularVelocity = new Vector3(0f, rotate * rotationSpeed * Mathf.Deg2Rad, 0f);
    }

    void Update()
    {
        // Handle shooting
        if (Input.GetButtonDown("Fire1")) // Default: Mouse button 0 (Left-click) or Spacebar
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Instantiate the bullet at the cannon shoot point
        GameObject bullet = Instantiate(bulletPrefab, cannonShootPoint.position, cannonShootPoint.rotation);

    }
}
