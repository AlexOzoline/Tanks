using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class TankAI : MonoBehaviour
{
    public Transform player;  // Assign player Transform
    public float speed = 10f; // Movement speed
    public float rotationSpeed = 100f; // Rotation speed
    public float stoppingDistance = 2f; // Stop near player
    public float pathUpdateInterval = 0.5f; // How often to update path

    private Rigidbody rb;
    private NavMeshPath path;
    private int currentPathIndex;
    private float nextPathUpdateTime;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic; 

        path = new NavMeshPath();
    }

    void Update()
    {
        if (player == null) return;

        // Update path at intervals
        if (Time.time >= nextPathUpdateTime)
        {
            if (NavMesh.CalculatePath(transform.position, player.position, NavMesh.AllAreas, path))
            {
                currentPathIndex = 0;
                nextPathUpdateTime = Time.time + pathUpdateInterval;
            }
        }
    }

    void FixedUpdate()
    {
        if (path.corners.Length > 1 && currentPathIndex < path.corners.Length)
        {
            Vector3 targetPosition = path.corners[currentPathIndex];
            MoveTowards(targetPosition);

            // If close to the current waypoint, move to the next
            if (Vector3.Distance(transform.position, targetPosition) < 1f)
            {
                currentPathIndex++;
            }
        }
    }

    void MoveTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0; // Keep movement flat

        // Move forward if not too close to the player
        if (Vector3.Distance(transform.position, player.position) > stoppingDistance)
        {
            rb.linearVelocity = transform.forward * speed;
        }
        else
        {
            rb.linearVelocity = Vector3.zero; // Stop when close
        }

        // Rotate toward the waypoint
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
    }
}
