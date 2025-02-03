using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Transform player;  // Reference to the player's transform
    public float followDistance = 10f;  // Distance at which the enemy starts following the player
    private NavMeshAgent navMeshAgent;

    void Start()
    {
        // Get the NavMeshAgent component attached to the enemy
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent not found on Enemy object. Please attach it.");
        }
    }

    void Update()
    {
        // Check if the NavMeshAgent exists and is on a valid NavMesh
        if (navMeshAgent == null || !navMeshAgent.isOnNavMesh) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= followDistance)
        {
            // Enemy starts following the player
            navMeshAgent.SetDestination(player.position);
        }
        else
        {
            // Stop following if too far
            navMeshAgent.ResetPath();
        }
    }
}
