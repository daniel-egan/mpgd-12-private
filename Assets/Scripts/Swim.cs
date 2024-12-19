using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public float speed = 5f;
    public float neighborRadius = 5f;
    public float separationDistance = 2f;

    private Vector3 velocity;

    void Update()
    {
        Vector3 cohesion = Vector3.zero;
        Vector3 alignment = Vector3.zero;
        Vector3 separation = Vector3.zero;

        List<Transform> neighbors = GetNeighbors();

        if (neighbors.Count > 0)
        {
            foreach (Transform neighbor in neighbors)
            {
                Vector3 toNeighbor = neighbor.position - transform.position;

                // Cohesion: Move towards average position
                cohesion += neighbor.position;

                // Alignment: Match direction
                alignment += neighbor.GetComponent<Fish>().velocity;

                // Separation: Avoid getting too close
                if (toNeighbor.magnitude < separationDistance)
                {
                    separation -= toNeighbor.normalized / toNeighbor.magnitude;
                }
            }

            cohesion /= neighbors.Count;
            cohesion = (cohesion - transform.position).normalized;

            alignment /= neighbors.Count;
            alignment.Normalize();
        }

        // Combine behaviors
        Vector3 flockDirection = (cohesion + alignment + separation).normalized;

        // Update velocity and move
        velocity = Vector3.Lerp(velocity, flockDirection * speed, Time.deltaTime);
        transform.position += velocity * Time.deltaTime;

        // Rotate to face movement direction
        if (velocity.magnitude > 0)
        {
            transform.rotation = Quaternion.LookRotation(velocity);
        }
    }

    private List<Transform> GetNeighbors()
    {
        List<Transform> neighbors = new List<Transform>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, neighborRadius);

        foreach (Collider collider in colliders)
        {
            if (collider != GetComponent<Collider>())
            {
                neighbors.Add(collider.transform);
            }
        }

        return neighbors;
    }
}


