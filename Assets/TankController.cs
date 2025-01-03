using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TankController : MonoBehaviour
{
    public float speed = 5f;        // Movement speed
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

        // Apply movement using Rigidbody
        rb.MovePosition(rb.position + movement);

        // Calculate rotation
        float rotation = rotate * rotationSpeed * Time.fixedDeltaTime;
        Quaternion turn = Quaternion.Euler(0f, rotation, 0f);

        // Apply rotation using Rigidbody
        rb.MoveRotation(rb.rotation * turn);
    }
}

