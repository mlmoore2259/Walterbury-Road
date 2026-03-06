using UnityEngine;
using UnityEngine.AdaptivePerformance;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    // Object/Component variables
    private NavMeshAgent agent;
    public Transform player;

    // Scaling variables
    private float viewDistance = 10f;
    private float viewAngle = 45f;
    private float speed = 1.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ChasePlayer()
    {
        if (CanSeePlayer())
        {
            agent.SetDestination(player.position);
        }
    }

    private bool CanSeePlayer()
    {
        Vector3 playerDirection = (player.position - transform.position).normalized;
        float playerAngle = Vector3.Angle(transform.forward, playerDirection);

        if (playerAngle < viewAngle / 2f)
        {
            if (!Physics.Linecast(transform.position, player.position))
            {
                return true;
            }
        }
        return false;
    }
}
