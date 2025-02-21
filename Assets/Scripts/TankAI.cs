using UnityEngine;

public class TankAI : MonoBehaviour
{
    private enum TankState { Patrol, Chase, Attack }

    public Transform player;
    public float chaseRange = 20f;
    public float attackRange = 10f;
    public float fireRate = 1.5f;
    public float movementSpeed = 5f;
    public float rotationSpeed = 60f; // Degrees per second
    public float directionChangeInterval = 3f;
    public float maxTurnDuration = 1.5f; // Time spent turning

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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        nextDirectionChangeTime = Time.time + directionChangeInterval;

        cannonShootPoint = transform.Find("tanktopfixed/CannonShootPoint"); 
    }

    void Update()
    {
        if (CanAttack()) AttackPlayer();
        else if (CanChase()) ChasePlayer();
        else RandomMovement();
    }

    // ➤ Random Movement Logic
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

    // ➤ Chase Logic (Turns towards player gradually)
    void ChasePlayer()
    {
        Vector3 toPlayer = (player.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(toPlayer);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        rb.linearVelocity = transform.forward * movementSpeed;
    }

    bool CanChase()
    {
        return Vector3.Distance(transform.position, player.position) < chaseRange;
    }

    // ➤ Attack Logic
    void AttackPlayer()
    {

        if (Time.time > lastShotTime + fireRate)
        {
            Shoot();
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
        Instantiate(bulletPrefab, cannonShootPoint.position, cannonShootPoint.rotation);
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
