using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TankController : MonoBehaviour
{
    public float speed = 10f;        // Movement speed (adjusted for realistic values)
    public float rotationSpeed = 100f; // Rotation speed
    public GameObject bulletPrefab;   // Bullet prefab reference
    public Transform cannonShootPoint; // The point from where bullets will be fired (cannon's muzzle)
    private Rigidbody rb;
    public float playerNumber;
    

    void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();

        // Freeze position on Y and rotations on X/Z
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
    }

    void FixedUpdate()
    {
        float moveInput = 0f;
        float turnInput = 0f;

        // Player 1 (WASD)
        if (playerNumber == 1)
        {
            if (Input.GetKey(KeyCode.W)) moveInput = 1;
            if (Input.GetKey(KeyCode.S)) moveInput = -1;
            if (Input.GetKey(KeyCode.A)) turnInput = -1;
            if (Input.GetKey(KeyCode.D)) turnInput = 1;
        }
        // Player 2 (Arrow Keys)
        else
        {
            if (Input.GetKey(KeyCode.UpArrow)) moveInput = 1;
            if (Input.GetKey(KeyCode.DownArrow)) moveInput = -1;
            if (Input.GetKey(KeyCode.LeftArrow)) turnInput = -1;
            if (Input.GetKey(KeyCode.RightArrow)) turnInput = 1;
        }

        // Apply movement
        rb.linearVelocity = transform.forward * moveInput * speed;
        rb.angularVelocity = new Vector3(0, turnInput * rotationSpeed, 0);
    }

    void Update()  // Use Update for input detection (shooting in this case)
    {
    // Player 1 Shooting (Spacebar)
    if (playerNumber == 1 && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
    {
        Shoot();
    }
    // Player 2 Shooting (Enter / Numpad Enter)
    else if (playerNumber == 2 && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
    {
        Shoot();
    }
    }

    void Shoot()
    {
        // Instantiate the bullet at the cannon shoot point
        GameObject bullet = Instantiate(bulletPrefab, cannonShootPoint.position, cannonShootPoint.rotation);

    }

    // Detect collisions with obstacles
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            // Destroy the Tank
            Destroy(gameObject);
        }
    }

}