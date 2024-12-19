//// Followed the tutorial below for this code
/// https://www.youtube.com/watch?v=-Iwsz4gdgyQ  
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

    // Inistialises the NavMeshAgent and Enemy Aiming class from Aim.cs
    void Start()
    {
        agent = GetComponent < NavMeshAgent>();
        player = GameObject.Find("Player");
        AimAndShoot = GetComponent<EnemyAiming>();
    }
    
    // Detects if player is either within the sight of the Enemey NPC or not
    // If so will chase them
    // If close enough will attack
    // If far away will just move around aimlessly
    void Update()
    {
        playerInSight = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInAttack = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (!playerInSight && !playerInAttack) goForPatrol();
        if (playerInSight && !playerInAttack) Chase();
        if (playerInSight && playerInAttack) Attack();
    }

    // The animation is set to swim and move around the tank
    // A new destination for the NPC is set continosuly for the NPC to go to
    void goForPatrol()
    {
        npcAnimator.SetTrigger("Swim");
        if (!walkpointSet) SearchForDest();
        if(walkpointSet) agent.SetDestination(destPoint);
        if(Vector3.Distance(transform.position, destPoint)<10) walkpointSet = false;
    }

    // Sets a new destination for the NPC
    void SearchForDest()
    {
        npcAnimator.SetTrigger("Swim");
        float z = Random.Range(-range, range);
        float x = Random.Range(-range, range);
        destPoint = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

        if (Physics.Raycast(destPoint, Vector3.down, groundLayer))
        {
            walkpointSet = true;
        }
    }

    // The NPC will move towards the player when within range
    void Chase()
    {
        npcAnimator.SetTrigger("Swim");
        agent.SetDestination(player.transform.position);
    }
    // The NPC will attack the player when within range
    // It calls the shoot function from the Enemy Aiming class in Aim.cs
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
