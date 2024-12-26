using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLookAtPlayer : MonoBehaviour
{
    public GameObject player;
    void Start()
    {
        
    }
    void Update()
    {
        Vector3 directionToPlayer = player.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        Quaternion offsetRotation = Quaternion.Euler(0, 90, 0);
        transform.rotation = lookRotation * offsetRotation;

    }
}
