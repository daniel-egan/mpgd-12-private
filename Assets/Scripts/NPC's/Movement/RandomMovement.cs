using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomMovement : MonoBehaviour
{
    NavMeshAgent agent;

    [SerializeField] LayerMask groundLayer;

    Vector3 destPoint;
    bool walkpointSet;
    [SerializeField] float range;

    [SerializeField] Transform player; // Reference to the player's Transform
    [SerializeField] float detectionRadius = 10f; // Zone radius to detect the player
    [SerializeField] float fleeDistance = 15f; // Distance to flee from the player

    bool isFleeing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        CheckPlayerDistance();
        if (!isFleeing)
        {
            goForPatrol();
        }
    }

    void CheckPlayerDistance()
    {
        if (player == null) return;

        // Calculate the distance to the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Check if the player is within the detection radius
        if (distanceToPlayer < detectionRadius)
        {
            Debug.Log("Player detected within zone. Fleeing...");
            FleeFromPlayer();
        }
    }

    void FleeFromPlayer()
    {
        isFleeing = true;

        // Calculate a direction vector away from the player
        Vector3 directionAway = (transform.position - player.position).normalized;
        Vector3 fleeDestination = transform.position + directionAway * fleeDistance;

        // Validate the flee destination
        NavMeshHit hit;
        if (NavMesh.SamplePosition(fleeDestination, out hit, range, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            Debug.Log($"Fleeing to position: {hit.position}");
        }
        else
        {
            Debug.LogWarning("Could not find a valid flee position!");
        }

        // Stop fleeing after some time
        StartCoroutine(StopFleeing());
    }

    IEnumerator StopFleeing()
    {
        yield return new WaitForSeconds(3f); // Flee for 3 seconds
        isFleeing = false;
        Debug.Log("Stopped fleeing. Resuming patrol.");
    }

    void goForPatrol()
    {
        if (!walkpointSet) SearchForDest();
        if (walkpointSet) agent.SetDestination(destPoint);
        if (Vector3.Distance(transform.position, destPoint) < 10) walkpointSet = false;
    }

    void SearchForDest()
    {
        float z = Random.Range(-range, range);
        float x = Random.Range(-range, range);
        float y = Random.Range(-range, range);
        destPoint = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
        if (Physics.Raycast(destPoint, Vector3.down, groundLayer))
        {
            walkpointSet = true;
        }
    }
}
