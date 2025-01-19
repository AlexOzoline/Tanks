using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TankController : MonoBehaviour
{
    public float speed = 100f;        // Movement speed
    public float rotationSpeed = 100f; // Rotation speed

    private Rigidbody rb;

    void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Get input for movement and rotation
        float move = Input.GetAxis("Vertical");  // Up/Down arrow keys or W/S keys
        float rotate = Input.GetAxis("Horizontal"); // Left/Right arrow keys or A/D keys

        // Calculate forward movement
        Vector3 movement = transform.forward * move * speed * Time.fixedDeltaTime;

        // Apply movement using Rigidbody's velocity (will respect physics collisions)
        rb.linearVelocity = new Vector3(movement.x, 0, movement.z); // Lock Y velocity to 0

        // Calculate rotation
        float rotation = rotate * rotationSpeed * Time.fixedDeltaTime;
        Quaternion turn = Quaternion.Euler(0f, rotation, 0f);

        // Apply rotation using Rigidbody
        rb.MoveRotation(rb.rotation * turn);

        // Enforce constraints: Lock X/Z rotation and Y position
        rb.position = new Vector3(rb.position.x, 0f, rb.position.z); // Lock Y position
        rb.rotation = Quaternion.Euler(0f, rb.rotation.eulerAngles.y, 0f); // Lock X/Z rotation
    }


    // Optional: Stop movement when hitting obstacles
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            rb.linearVelocity = Vector3.zero; // Stop the tank from moving
            Debug.Log("Tank hit an obstacle!");
        }
    }
}

