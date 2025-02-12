using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(Rigidbody))]
public class TankControllerNet : NetworkBehaviour
{
    public float speed = 10f;
    public float rotationSpeed = 100f;
    public GameObject bulletPrefab;
    public Transform cannonShootPoint;

    private Rigidbody rb;

    public override void OnNetworkSpawn()
    {   
        Debug.Log("Am I the owner?" + IsOwner);
        if (IsOwner) 
        {
            rb = GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;

            if (cannonShootPoint == null)
            {
                cannonShootPoint = transform.Find("tanktopfixed/CannonShootPoint"); // Adjust to the correct child object name
            }
        }
    }

    void FixedUpdate()
    {
        if (!IsOwner) 
        {
            return;
        }

        float moveInput = 0f;
        float turnInput = 0f;

        if (Input.GetKey(KeyCode.W)) moveInput = 1;
        if (Input.GetKey(KeyCode.S)) moveInput = -1;
        if (Input.GetKey(KeyCode.A)) turnInput = -1;
        if (Input.GetKey(KeyCode.D)) turnInput = 1;

        rb.linearVelocity = transform.forward * moveInput * speed;
        rb.angularVelocity = new Vector3(0, turnInput * rotationSpeed, 0);
    }

    void Update()
    {
        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            ShootServerRpc();
        }
    }

    [ServerRpc]
    void ShootServerRpc()
    {
        GameObject bullet = Instantiate(bulletPrefab, cannonShootPoint.position, cannonShootPoint.rotation);
        bullet.GetComponent<NetworkObject>().Spawn(true);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && IsServer)
        {
            NetworkObject.Destroy(gameObject);
        }
    }
}
