using UnityEngine;
using UnityEngine.AI;

public class TankAI : MonoBehaviour
{
    private enum TaskStatus { Success, Failure, Running }
    private enum TankState { Patrol, Chase, Attack }

    public Transform[] patrolPoints;  // Waypoints for patrolling
    private int currentPatrolIndex = 0;
    
    public Transform player;
    public float chaseRange = 20f;
    public float attackRange = 10f;
    public float fireRate = 1.5f;  // Time between shots

    private NavMeshAgent agent;
    private float lastShotTime;

    public GameObject bulletPrefab;
    public Transform cannonShootPoint;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent is missing on AI Tank!");
        }
        else
        {
            Debug.Log("NavMeshAgent found. Starting patrol.");
            GoToNextPatrolPoint();
        }
    }

    void Update()
    {
        // Decision making with Behavior Tree logic
        if (CanAttack()) AttackPlayer();
        else if (CanChase()) ChasePlayer();
        else Patrol();
    }

    // ➤ Patrol Logic
    void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance < 1f)
        {
            GoToNextPatrolPoint();
        }
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;
        agent.destination = patrolPoints[currentPatrolIndex].position;
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    // ➤ Chase Logic
    void ChasePlayer()
    {
        agent.SetDestination(player.position);
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
        return Vector3.Distance(transform.position, player.position) < attackRange;
    }

    void Shoot()
    {
        Debug.Log("Tank Shoots!");
        GameObject bullet = Instantiate(bulletPrefab, cannonShootPoint.position, cannonShootPoint.rotation);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
    }
}
