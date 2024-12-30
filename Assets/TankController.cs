using UnityEngine;

public class TankController : MonoBehaviour
{
    public float speed = 5f;        // Movement speed
    public float rotationSpeed = 100f; // Rotation speed

    void Update()
    {
        // Get input for movement and rotation
        float move = Input.GetAxis("Vertical");  // Up/Down arrow keys or W/S keys
        float rotate = Input.GetAxis("Horizontal"); // Left/Right arrow keys or A/D keys

        // Move the tank forward/backward
        transform.Translate(Vector3.forward * move * speed * Time.deltaTime);

        // Rotate the tank
        transform.Rotate(Vector3.up * rotate * rotationSpeed * Time.deltaTime);
    }
}
