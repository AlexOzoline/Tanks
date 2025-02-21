using UnityEngine;
using UnityEngine.AI;

public class PathCalculationExample : MonoBehaviour
{
    public Transform target;
    private NavMeshPath path;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
    }

    void Update()
    {
        if (target != null)
        {
            // Calculate the path from agent's position to target
            if (NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path))
            {
                Debug.Log("Path calculated successfully! Number of corners: " + path.corners.Length);
                
                // Draw the path in the Scene view
                for (int i = 0; i < path.corners.Length - 1; i++)
                {
                    Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
                }
            }
        }
    }
}