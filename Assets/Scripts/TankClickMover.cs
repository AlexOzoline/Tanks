using UnityEngine;
using UnityEngine.AI; // Required for pathfinding

[RequireComponent(typeof(NavMeshAgent))]
public class TankClickMover : MonoBehaviour
{
    public GameObject bulletPrefab;   // Bullet prefab reference
    public Transform cannonShootPoint; // Where bullets are fired from

    private NavMeshAgent agent; // Handles movement

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        HandleClickMovement(); // Click-to-move functionality
        HandleShooting();      // Shooting controls
    }

    void HandleClickMovement()
    {
        // Check for left mouse button click
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Move the tank to the clicked location
                agent.SetDestination(hit.point);
            }
        }
    }

    void HandleShooting()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, cannonShootPoint.position, cannonShootPoint.rotation);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
    }
}
