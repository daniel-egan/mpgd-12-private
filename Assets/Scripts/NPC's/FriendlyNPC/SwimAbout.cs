//// Followed the tutorial below for this code
/// https://www.youtube.com/watch?v=-Iwsz4gdgyQ  and CHATGPT
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SwimAbout : MonoBehaviour
{
    NavMeshAgent agent;

    [SerializeField] LayerMask groundLayer;

    Vector3 destPoint;
    bool walkpointSet;
    [SerializeField] float range;


    // Inistialises the NavMeshAgent
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
       goForPatrol();
    }

    // The animation is set to swim and move around the tank
    // A new destination for the NPC is set continosuly for the NPC to go to
    // NPC models are shifted 90 degrees sp
    // Rotate the model to face its movement direction with an added 90-degree Y-axis rotation
    // Smoothly interpolate to the target rotation
    void goForPatrol()
    {
        if (!walkpointSet) SearchForDest();

        if (walkpointSet)
        {
            agent.SetDestination(destPoint);
            Vector3 direction = (agent.velocity != Vector3.zero) ? agent.velocity.normalized : Vector3.zero;
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, targetRotation.eulerAngles.y - 90, 0), Time.deltaTime * 5f);
            }
        }

        if (Vector3.Distance(transform.position, destPoint) < 10) walkpointSet = false;
    }



    // Sets a new destination for the NPC
    void SearchForDest()
    {
        float z = Random.Range(-range, range);
        float x = Random.Range(-range, range);
        destPoint = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

        if (Physics.Raycast(destPoint, Vector3.down, groundLayer))
        {
            walkpointSet = true;
        }
    }

}
