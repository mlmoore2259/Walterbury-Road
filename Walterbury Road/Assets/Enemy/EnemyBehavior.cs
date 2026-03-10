using UnityEngine;
using UnityEngine.AdaptivePerformance;
using UnityEngine.AI;
using System.Collections;

public class EnemyBehavior : MonoBehaviour
{
    // Object/Component variables
    private NavMeshAgent agent;
    public GameObject player;
    private Coroutine roamCoroutine;

    // Boolean flags
    private bool isRoaming = false;
    private bool awareOfPlayer = false;

    // Scaling variables
    private float viewDistance = 10f;
    private float viewAngle = 45f;
    private float chaseSpeed = 4f;
    private float roamSpeed = 2f;
    private float roamRadius = 50f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        EnemyMovement();
    }

    private void EnemyMovement()
    {
        // Check that the enemy knows of players existence
        if (awareOfPlayer)
        {
            isRoaming = true;
        }

        // Can see the player and will chase them
        if (CanSeePlayer())
        {
            awareOfPlayer = true;
            ChasePlayer();
        }

        // Roaming
        else if (isRoaming )
        {
            Roam();
        }
    }

    private void ChasePlayer()
    {
        agent.isStopped = false;
        agent.speed = chaseSpeed;
        agent.SetDestination(player.transform.position);
    }

    private void Roam()
    {
        agent.speed = roamSpeed;
        if (isRoaming && roamCoroutine == null)
        {
            roamCoroutine = StartCoroutine(RoamRoutine(10f));
        }
    }

    private bool CanSeePlayer()
    {
        if (player == null)
        {
            Debug.LogWarning("EnemyBehavior: player Transform is not assigned.");
            return false;
        }

        Vector3 toPlayer = player.transform.position - transform.position;
        float sqrDistance = toPlayer.sqrMagnitude;

        // Check distance first
        if (sqrDistance > viewDistance * viewDistance)
            return false;

        Vector3 playerDirection = toPlayer.normalized;
        float playerAngle = Vector3.Angle(transform.forward, playerDirection);

        // Angle check (use half-angle)
        if (playerAngle > viewAngle / 2f)
            return false;

        // Offset origin and target slightly upward to avoid hitting the enemy's own collider or the ground
        Vector3 origin = transform.position + Vector3.up * 1.0f;
        Vector3 target = player.transform.position + Vector3.up * 1.0f;
        Vector3 dir = (target - origin).normalized;
        float distance = Mathf.Min(viewDistance, (target - origin).magnitude);

        if (Physics.Raycast(origin, dir, out RaycastHit hit, distance))
        {
            Debug.DrawRay(origin, dir * distance, Color.red, 1f);
            // If the first thing hit is the player (or a child of the player), player is visible
            if (hit.transform == player || hit.transform.IsChildOf(player.transform))
                return true;

            // Something else is blocking view
            return false;
        }

        // No hit (should be rare because distance check was done) — treat as visible
        return true;
    }


    public Vector3 GeneratePoint(Vector3 origin, float range)
    {
        const int maxAttempts = 30;
        const float sampleMaxDistance = 20f;
        NavMeshHit hit;
        NavMeshPath path = new NavMeshPath();

        // Ensure we use a position that is actually on the NavMesh as the origin for path calculations.
        Vector3 navOrigin = origin;
        NavMeshHit originHit;
        // try a small radius first, then expand
        if (NavMesh.SamplePosition(origin, out originHit, 1f, NavMesh.AllAreas))
        {
            navOrigin = originHit.position;
        }
        else if (agent != null && NavMesh.SamplePosition(agent.transform.position, out originHit, 1f, NavMesh.AllAreas))
        {
            // fallback to the agent's position if the MonoBehaviour transform isn't on the NavMesh
            navOrigin = originHit.position;
        }
        else if (NavMesh.SamplePosition(origin, out originHit, sampleMaxDistance, NavMesh.AllAreas))
        {
            // last-resort: try a larger radius
            navOrigin = originHit.position;
        }

        for (int i = 0; i < maxAttempts; i++)
        {
            // Pick random point around the navOrigin
            Vector3 randomOffset = Random.insideUnitSphere * range;
            randomOffset.y = 0f;
            Vector3 candidate = navOrigin + randomOffset;

            if (NavMesh.SamplePosition(candidate, out hit, sampleMaxDistance, NavMesh.AllAreas))
            {
                // Calculate path starting from navOrigin (which is on the NavMesh)
                if (NavMesh.CalculatePath(navOrigin, hit.position, NavMesh.AllAreas, path) && path.status == NavMeshPathStatus.PathComplete)
                {
                    return hit.position;
                }
            }
        }

        // If we couldn't find a valid roaming point, return the navOrigin (on-NavMesh fallback)
        return navOrigin;
    }


    IEnumerator RoamRoutine(float waitTime)
    {
        while (isRoaming)
        {
            Vector3 newDest = GeneratePoint(transform.position, roamRadius);
            // Only set destination if it's meaningfully different
            if ((newDest - agent.destination).sqrMagnitude > 0.01f)
            {
                agent.SetDestination(newDest);
                Debug.Log("New Destination Set: " + newDest);
            }
            else
            {
                Debug.Log("GeneratePoint returned fallback origin; retrying next tick.");
            }
            yield return new WaitForSeconds(waitTime);
        }
        roamCoroutine = null;
    }
}
