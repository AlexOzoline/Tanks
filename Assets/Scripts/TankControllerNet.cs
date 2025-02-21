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
        Debug.Log($"Tank {NetworkObjectId} - IsOwner: {IsOwner}");
        rb = GetComponent<Rigidbody>();
        if (cannonShootPoint == null)
        {
            cannonShootPoint = transform.Find("tanktopfixed/CannonShootPoint");
        }

        if (IsOwner) 
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;

        }
    }

    void FixedUpdate()
    {
        if (!IsOwner) return;

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
            Debug.Log($"Tank {NetworkObjectId} attempting to shoot.");
            ShootServerRpc();
        }
    }

    [ServerRpc]
    void ShootServerRpc(ServerRpcParams rpcParams = default)
    {
        if (bulletPrefab == null || cannonShootPoint == null)
        {
            Debug.LogError("BulletPrefab or CannonShootPoint is not set!");
            return;
        }

        Debug.Log($"Tank {NetworkObjectId} shooting on the server.");
        
        GameObject bullet = Instantiate(bulletPrefab, cannonShootPoint.position, cannonShootPoint.rotation);
        bullet.GetComponent<NetworkObject>().Spawn();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && IsServer)
        {
            NetworkObject.Destroy(gameObject);
        }
    }
}
