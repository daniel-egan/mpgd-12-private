using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehavior : MonoBehaviour
{
    public Animator npcAnimator;
    public GameObject player;
    public float rotationSpeed = 5f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameObject.CompareTag("EnemySquid"))
            {
                Vector3 directionToPlayer = player.transform.position - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }

            // Trigger the Attack animation
            npcAnimator.SetTrigger("Attack");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            npcAnimator.SetTrigger("Swim");
        }
    }

 
}

