using UnityEngine;

public class TankAI : MonoBehaviour
{
    private enum TankState { Patrol, Chase, Attack, SeekCover }

    public Transform player;
    public float chaseRange = 100f;
    public float attackRange = 10f;
    public float fireRate = 1.5f;
    public float movementSpeed = 5f;
    public float rotationSpeed = 60f; // Degrees per second
    public float directionChangeInterval = 3f;
    public float maxTurnDuration = 1.5f; // Time spent turning
    public float coverCheckRadius = 15f; // Distance to look for cover

    private Rigidbody rb;
    private float lastShotTime;
    private float nextDirectionChangeTime;
    private bool movingForward = true;
    private bool isTurning = false;
    private float turnEndTime;
    private float targetTurnAngle;
    private float turnDirection; // -1 = left, 1 = right

    public GameObject bulletPrefab;
    public Transform cannonShootPoint;

    private Vector3 coverTarget; // Target position for cover

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        nextDirectionChangeTime = Time.time + directionChangeInterval;

        cannonShootPoint = transform.Find("tanktopfixed/CannonShootPoint");
    }

    void Update()
    {
        if (CanAttack()) AttackPlayer();
        else if (CanSeekCover()) SeekCover();
        else RandomMovement();
    }

    // Random Movement Logic
    void RandomMovement()
    {
        if (Time.time > nextDirectionChangeTime)
        {
            StartTurning();
            movingForward = Random.value > 0.1f; // xx% chance to move backward instead
            nextDirectionChangeTime = Time.time + directionChangeInterval;
        }

        if (isTurning)
        {
            PerformTurning();
        }

        MoveTank();
    }

    void MoveTank()
    {
        float moveDirection = movingForward ? 1f : -1f;
        rb.linearVelocity = transform.forward * moveDirection * movementSpeed;
    }

    void StartTurning()
    {
        isTurning = true;
        turnEndTime = Time.time + Random.Range(0.5f, maxTurnDuration); // Random turn duration
        turnDirection = Random.value > 0.5f ? 1f : -1f; // Random left or right
        targetTurnAngle = turnDirection * rotationSpeed;
    }

    void PerformTurning()
    {
        if (Time.time < turnEndTime)
        {
            transform.Rotate(Vector3.up, targetTurnAngle * Time.deltaTime);
        }
        else
        {
            isTurning = false;
        }
    }


    // ➤ Attack Logic
    void AttackPlayer()
    {
        if (Time.time > lastShotTime + fireRate)
        {
            Vector3 shootDirection = (player.position - cannonShootPoint.position).normalized;
            Vector3 bounceDirection;

            
            if (CanAttack()) 
            {
                Shoot(); // Take a normal shot
            }

            lastShotTime = Time.time;
        }
    }

    bool CanAttack()
    {
        if (Vector3.Distance(transform.position, player.position) > attackRange) return false;

        RaycastHit hit;
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        if (Physics.Raycast(transform.position + Vector3.up * 1.5f, directionToPlayer, out hit, attackRange))
        {
            return hit.transform == player;
        }
        return false;
    }

    void Shoot()
    {
        Debug.Log("Tank Shoots!");
        // Shoot a regular shot
        Instantiate(bulletPrefab, cannonShootPoint.position, cannonShootPoint.rotation);

    }

    

    // Seek Cover Logic
    void SeekCover()
    {
        Vector3 bestCoverPosition = FindCover();
        if (bestCoverPosition != Vector3.zero)
        {
            MoveToCoverOppositePlayer(bestCoverPosition);
        }
        else
        {
            RandomMovement(); // If no cover found, continue random movement
        }
    }

    bool CanSeekCover()
    {
        return Vector3.Distance(transform.position, player.position) < chaseRange;
    }

    Vector3 FindCover()
    {
        // Look for objects within a given range (you can use layer masks to filter obstacles)
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, coverCheckRadius);
        foreach (var collider in hitColliders)
        {
            // Check if the object can act as cover (just an example condition)
            if (collider.CompareTag("Obstacle")) // Assuming obstacles are tagged "Obstacle"
            {
                return collider.transform.position; // Find nearest obstacle cover
            }
        }
        return Vector3.zero; // No cover found
    }

    void MoveToCoverOppositePlayer(Vector3 coverPosition)
    {
        // Calculate the direction from the cover to the player
        Vector3 directionToPlayer = (player.position - coverPosition).normalized;

        // Now we need to move to the opposite side of the cover
        // Calculate a point on the opposite side of the cover by moving away from the player
        Vector3 coverEscapePosition = coverPosition - directionToPlayer * 5f; // Move 5 units away from the player

        // Move towards the escape position
        Vector3 directionToMove = (coverEscapePosition - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToMove);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        rb.linearVelocity = transform.forward * movementSpeed;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            movingForward = !movingForward;
            MoveTank();
        }
    }
}
