using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour
{
    private EnemyAiming AimAndShoot;
    public Animator npcAnimator;
    NavMeshAgent agent;
    GameObject player;

    [SerializeField] LayerMask groundLayer, playerLayer;

    Vector3 destPoint;
    bool walkpointSet;
    [SerializeField] float range;

    [SerializeField] float sightRange, attackRange;
    bool playerInSight, playerInAttack;

    // Custom swim behavior parameters
    [SerializeField] float swimHeightRange = 40f; // Range for vertical movement (up/down)
    [SerializeField] float swimSpeed = 10f; // Speed of vertical swimming

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
        AimAndShoot = GetComponent<EnemyAiming>();
    }

    void Update()
    {
        playerInSight = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInAttack = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (!playerInSight && !playerInAttack) goForPatrol();
        if (playerInSight && !playerInAttack) Chase();
        if (playerInSight && playerInAttack) Attack();
    }

    void goForPatrol()
    {
        npcAnimator.SetTrigger("Swim");
        if (!walkpointSet) SearchForDest();
        if (walkpointSet) agent.SetDestination(destPoint);

        // Apply vertical movement to simulate swimming up/down
        float yPosition = Mathf.PingPong(Time.time * swimSpeed, swimHeightRange) + transform.position.y;
        transform.position = new Vector3(transform.position.x, yPosition, transform.position.z);

        if (Vector3.Distance(transform.position, destPoint) < 10) walkpointSet = false;
    }

    void SearchForDest()
    {
        // Randomize the destination within a specified range
        float z = Random.Range(-range, range);
        float x = Random.Range(-range, range);
        destPoint = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

        if (Physics.Raycast(destPoint, Vector3.down, groundLayer))
        {
            walkpointSet = true;
        }
    }

    void Chase()
    {
        agent.SetDestination(player.transform.position);

        // Optionally, you can add vertical chase logic if the fish needs to chase the player up/down
        float yPosition = Mathf.PingPong(Time.time * swimSpeed, swimHeightRange) + transform.position.y;
        transform.position = new Vector3(transform.position.x, yPosition, transform.position.z);
    }

    void Attack()
    {
        npcAnimator.SetTrigger("Attack");

        if (AimAndShoot != null)
        {
            AimAndShoot.Shoot();
        }
        else
        {
            Debug.LogWarning("AimAndShoot script not found on the same GameObject!");
        }
    }
}
