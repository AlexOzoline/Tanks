using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TankMovement : MonoBehaviour
{
    public float speed = 10f;         // Movement speed
    public float rotationSpeed = 100f; // Rotation speed

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic; // Prevents passing through walls
    }

    void FixedUpdate()
    {
        float moveInput = Input.GetAxis("Vertical");
        float turnInput = Input.GetAxis("Horizontal");

        // Move the tank forward/backward
        rb.linearVelocity = transform.forward * moveInput * speed;

        // Rotate only if there's input
        if (turnInput != 0)
        {
            rb.angularVelocity = new Vector3(0, turnInput * rotationSpeed, 0);
        }
        else
        {
            rb.angularVelocity = Vector3.zero; // Prevent unwanted rotation
        }
    }
}
